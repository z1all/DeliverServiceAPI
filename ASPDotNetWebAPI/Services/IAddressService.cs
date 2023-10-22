using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IAddressService
    {
        IEnumerable<SearchAddressDTO> GetChildObjects(int parentObjectId, string? name);
        Task<IEnumerable<SearchAddressDTO>> GetPathFromRootToObject(Guid ObjectGuid);
    }
}