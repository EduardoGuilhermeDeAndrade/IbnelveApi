using Microsoft.AspNetCore.Http;

namespace IbnelveApi.Application.Interfaces
{


    public interface IFotoStorageService
    {
        Task<string> SalvarFotoAsync(IFormFile file);
    }
}
