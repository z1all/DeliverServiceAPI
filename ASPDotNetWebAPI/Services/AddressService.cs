using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<List<SearchAddressDTO>> GetChildObjectsAsync(int parentObjectId, string? name)
        {
            var addressElement = await
                _dbContext.Hierarchys
                .Join(_dbContext.AddressElements,
                    fromParentTo => fromParentTo.Objectid,
                    child => child.Objectid,
                    (fromParentTo, child) => new { fromParentTo, child })
                .Where(x => x.fromParentTo.Parentobjid == parentObjectId)
                .Select(x => new SearchAddressDTO(x.child)).Take(15).ToListAsync();

            var houses = await
                _dbContext.Hierarchys
                .Join(_dbContext.Houses,
                    fromParentTo => fromParentTo.Objectid,
                    child => child.Objectid,
                    (fromParentTo, child) => new { fromParentTo, child })
                .Where(x => x.fromParentTo.Parentobjid == parentObjectId)
                .Select(x => new SearchAddressDTO(x.child, GetFullName(x.child))).Take(15).ToListAsync();

            var elements = addressElement.Concat(houses).ToList();

            if(name == null)
            { 
                return elements;
            }

            var answer = new List<SearchAddressDTO>();
            foreach (var element in elements)
            {
                if (element.Text.Contains(name, StringComparison.OrdinalIgnoreCase))
                {
                    answer.Add(element);
                }
            }

            return answer;
        }

        public async Task<List<SearchAddressDTO>> GetPathFromRootToObjectAsync(Guid ObjectGuid)
        {
            var ObjectId = await GetObjectIdAsync(ObjectGuid);
            if (ObjectId == null)
            {
                return new List<SearchAddressDTO>();
            }

            var hierarchys = await _dbContext.Hierarchys.FirstAsync(hierarchys => hierarchys.Objectid == ObjectId);

            var path = hierarchys.Path.Split(".");
            var addressElementsPath = _dbContext.AddressElements.Where(addressElements => path.Contains(addressElements.Objectid.ToString()));

            var addressElementsPathOrded = new List<SearchAddressDTO>();
            foreach (var objectId in path)
            {
                var element = await addressElementsPath.FirstOrDefaultAsync(addressElement => addressElement.Objectid == int.Parse(objectId));
                if (element != null)
                {
                    addressElementsPathOrded.Add(new SearchAddressDTO(element));
                }
            }

            if (path.Length != addressElementsPathOrded.Count)
            {
                var house = await _dbContext.Houses.FirstAsync(house => house.Objectid == ObjectId);
                addressElementsPathOrded.Add(new SearchAddressDTO(house, GetFullName(house)));
            }

            return addressElementsPathOrded;
        }

        private async Task<int?> GetObjectIdAsync(Guid ObjectGuid)
        {
            var addressElement = await _dbContext.AddressElements.FirstOrDefaultAsync(addressElement => addressElement.Objectguid == ObjectGuid);
            if (addressElement == null)
            {
                var house = await _dbContext.Houses.FirstOrDefaultAsync(house => house.Objectguid == ObjectGuid);
                if (house != null)
                {
                    return house.Objectid;
                }

                return null;
            }
            else
            {
                return addressElement.Objectid;
            }
        }

        private static string GetFullName(House house)
        {
            string name = "";

            if (house.Housenum != null)
            {
                name += house.Housenum;
            }

            if (house.Addnum1 != null)
            {
                if (house.Housetype != null)
                {
                    name += " " + ((HouseType)house.Housetype).GetDescription() + " " + house.Addnum1;
                }
                else if (house.Housenum == null)
                {
                    name += house.Addnum1;
                }
            }

            return name;
        }
    }
}