using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;

namespace ASPDotNetWebAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressService(ApplicationDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SearchAddressDTO> GetObjectChild(int parentObjectId, string? name)
        {
            var addressElement = 
                from fromParentTo in _dbContext.Hierarchys
                join child in _dbContext.AddressElements
                on fromParentTo.Objectid equals child.Objectid
                where fromParentTo.Parentobjid == parentObjectId && name != null && child.Name.Contains(name)
                select new SearchAddressDTO
                {
                    ObjectId = child.Objectid,
                    ObjectGuid = child.Objectguid,
                    Text = child.Typename + " " + child.Name,
                    ObjectLevel = (GarAddressLevel)int.Parse(child.Level),
                };

            var houses = 
                from fromParentTo in _dbContext.Hierarchys
                join child in _dbContext.Houses
                on fromParentTo.Objectid equals child.Objectid
                where fromParentTo.Parentobjid == parentObjectId && name != null && GetFullName(child).Contains(name)
                select new SearchAddressDTO
                {
                    ObjectId = child.Objectid,
                    ObjectGuid = child.Objectguid,
                    Text = GetFullName(child),
                    ObjectLevel = GarAddressLevel.Building,
                };

            return addressElement.Concat(houses);
        }

        public IEnumerable<SearchAddressDTO> GetPathFromRootToObject(Guid ObjectGuid)
        {
            throw new NotImplementedException();
        }

        private string GetFullName(House house)
        {
            string name = "";

            if(house.Housenum != null)
            {
                name += house.Housenum;
            }

            if(house.Addnum1 != null)
            {
                if(house.Housetype != null)
                {
                    name += " " + (HouseType)house.Housetype + " " + house.Addnum1;
                }
                else if(house.Housenum == null)
                {
                    name += house.Addnum1;
                }
            }

            return name;
        }
    }
}