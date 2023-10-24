using ASPDotNetWebAPI.Models.DTO;

namespace ASPDotNetWebAPI.Services
{
    public interface IAddressService
    {
        Task<List<SearchAddressDTO>> GetChildObjectsAsync(int parentObjectId, string? name);
        Task<List<SearchAddressDTO>> GetPathFromRootToObjectAsync(Guid ObjectGuid);
    }
}