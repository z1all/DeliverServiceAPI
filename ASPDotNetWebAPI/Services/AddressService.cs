using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using ASPDotNetWebAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ASPDotNetWebAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _dbContext;

        public AddressService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<SearchAddressDTO> GetChildObjects(int parentObjectId, string? name)
        {
            IEnumerable<SearchAddressDTO> addressElement =
                from fromParentTo in _dbContext.Hierarchys
                join child in _dbContext.AddressElements
                on fromParentTo.Objectid equals child.Objectid
                where fromParentTo.Parentobjid == parentObjectId && (name != null && child.Name.Contains(name) || name == null)
                select new SearchAddressDTO(child);

            IEnumerable<SearchAddressDTO> houses =
                from fromParentTo in _dbContext.Hierarchys
                join child in _dbContext.Houses
                on fromParentTo.Objectid equals child.Objectid
                where fromParentTo.Parentobjid == parentObjectId && (name != null && GetFullName(child).Contains(name) || name == null)
                select new SearchAddressDTO(child, GetFullName(child));

            return addressElement.Concat(houses);
        }

        public async Task<IEnumerable<SearchAddressDTO>> GetPathFromRootToObject(Guid ObjectGuid)
        {
            var ObjectId = await GetObjectIdAsync(ObjectGuid);
            if (ObjectId == null)
            {
                return Enumerable.Empty<SearchAddressDTO>();
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