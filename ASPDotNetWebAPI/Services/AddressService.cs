using ASPDotNetWebAPI.Exceptions;
using ASPDotNetWebAPI.Models;
using ASPDotNetWebAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ASPDotNetWebAPI.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly int _skipAddress;
        private readonly int _skipHouses;

        public AddressService(ApplicationDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _skipAddress = configuration.GetValue("GlobalConstant:SkipAddress", 15);
            _skipHouses = configuration.GetValue("GlobalConstant:SkipHouse", 15);
        }

        public async Task<List<SearchAddressDTO>> GetChildObjectsAsync(int parentObjectId, string? name)
        {
            string nameElement = name != null ? name.ToLower() : string.Empty;

            var addressElement = await
                _dbContext.Hierarchys
                .Join(_dbContext.AddressElements,
                    fromParentTo => fromParentTo.Objectid,
                    child => child.Objectid,
                    (fromParentTo, child) => new { fromParentTo, child })
                .Where(x => x.fromParentTo.Parentobjid == parentObjectId && (x.child.Typename + x.child.Name).ToLower().Contains(nameElement))
                .Take(_skipAddress)
                .Select(x => new SearchAddressDTO(x.child))
                .ToListAsync();

            var houses = await
                _dbContext.Hierarchys
                .Join(_dbContext.Houses,
                    fromParentTo => fromParentTo.Objectid,
                    child => child.Objectid,
                    (fromParentTo, child) => new { fromParentTo, child })
                .Where(x => x.fromParentTo.Parentobjid == parentObjectId && x.child.FullName.ToLower().Contains(nameElement))
                .Take(_skipHouses)
                .Select(x => new SearchAddressDTO(x.child))
                .ToListAsync();

            return addressElement.Concat(houses).DistinctBy(element => element.ObjectId).ToList();
        }

        public async Task<List<SearchAddressDTO>> GetPathFromRootToObjectAsync(Guid ObjectGuid)
        {
            var elementInfo = await GetObjectIdAsync(ObjectGuid);
            if (elementInfo.Item1 == null)
            {
                throw new NotFoundException($"Not found address element with ObjectGuid={ObjectGuid}");
            }

            var path = await GetPathAsArrayAsync((int)elementInfo.Item1);  
            var addressElementsPath = await _dbContext.AddressElements
                .Where(addressElements => path.Contains(addressElements.Objectid.ToString()))
                .ToListAsync();

            var addressElementsPathOrded = new List<SearchAddressDTO>();
            foreach (var objectId in path)
            {
                var element = addressElementsPath.FirstOrDefault(addressElement => addressElement.Objectid == int.Parse(objectId));
                if (element != null)
                {
                    addressElementsPathOrded.Add(new SearchAddressDTO(element));
                }
            }

            if (elementInfo.Item2 != null)
            {
                addressElementsPathOrded.Add(new SearchAddressDTO(elementInfo.Item2));
            }

            return addressElementsPathOrded;
        }

        public async Task<RegionTimeZoneDTO> GetRegionTimeZoneAsync(Guid ObjectGuid)
        {
            var elementInfo = await GetObjectIdAsync(ObjectGuid);
            if (elementInfo.Item1 == null)
            {
                throw new NotFoundException($"Not found address element with ObjectGuid={ObjectGuid}");
            }

            var path = await GetPathAsArrayAsync((int)elementInfo.Item1);
            var regionId = int.Parse(path[0]);

            var region = await _dbContext.RegionTimeZones
                .FirstOrDefaultAsync(regionTimeZone => regionTimeZone.RegionId == regionId);
            if (region == null)
            {
                throw new NotFoundException($"Region with ObjectGuid={ObjectGuid} not found!");
            }

            return new RegionTimeZoneDTO()
            {
                Region = region.Region,
                TimeDifferenceWithMoscow = region.TimeDifferenceWithMoscow
            };
        }

        private async Task<Tuple<int?, House?>> GetObjectIdAsync(Guid ObjectGuid)
        {
            var addressElement = await _dbContext.AddressElements.FirstOrDefaultAsync(addressElement => addressElement.Objectguid == ObjectGuid);
            if (addressElement == null)
            {
                var house = await _dbContext.Houses.FirstOrDefaultAsync(house => house.Objectguid == ObjectGuid);
                if (house != null)
                {
                    return new Tuple<int?, House?>(house.Objectid, house);
                }

                return new Tuple<int?, House?>(null, null);
            }
            else
            {
                return new Tuple<int?, House?>(addressElement.Objectid, null);
            }
        }

        private async Task<string[]> GetPathAsArrayAsync(int ObjectId)
        {
            var hierarchys = await _dbContext.Hierarchys.FirstAsync(hierarchys => hierarchys.Objectid == ObjectId);

            return hierarchys.Path.Split(".");
        } 
    }
}