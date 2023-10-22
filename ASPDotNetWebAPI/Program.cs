using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using ASPDotNetWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Modification of Swagger for authentication
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string> ()
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// Configuring Authentication
var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");
builder.Services.AddAuthentication(authOptions =>
{
    authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwtOptions =>
    {
        jwtOptions.RequireHttpsMetadata = false; // Develop 
        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
            ValidateIssuer = true,
            ValidIssuer = "HITs",
            ValidateAudience = false
        };
        
    });

// Connecting the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Adding services
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Creating auto migrations
using var serviceScope = app.Services.CreateScope();
var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
dbContext?.Database.Migrate();


IEnumerable<SearchAddressDTO> query = from fromParentTo in dbContext.Hierarchys
            join child in dbContext.Houses
            on fromParentTo.Objectid equals child.Objectid
            where fromParentTo.Parentobjid == 1289631 // && (child.Housenum != null? child.Housenum.Contains("То") : true)
            select new SearchAddressDTO
            {
                ObjectId = child.Objectid,
                ObjectGuid = child.Objectguid,
                Text = (child.Housenum != null? child.Housenum : "") + " " + (child.Housetype != null ? ((HouseType)child.Housetype).GetDescription() : "") + " " + (child.Addnum1 != null ? child.Addnum1 : ""),
                ObjectLevel = GarAddressLevel.Building,
            };

foreach (var number in query)
{
    Console.WriteLine();
    Console.WriteLine(number.ObjectId);
    Console.WriteLine(number.ObjectGuid);
    Console.WriteLine(number.Text);
    Console.WriteLine(number.ObjectLevel);
    Console.WriteLine();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
