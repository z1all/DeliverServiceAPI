using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IAddressService
    {
        IEnumerable<SearchAddressDTO> GetObjectChild(int parentObjectId, string? name);
        IEnumerable<SearchAddressDTO> GetPathFromRootToObject(Guid ObjectGuid);
    }
}