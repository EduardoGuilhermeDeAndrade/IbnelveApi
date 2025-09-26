using IbnelveApi.Application.Interfaces;


namespace IbnelveApi.Api.Services
{

    public class FotoStorageService : IFotoStorageService
    {
        private readonly IWebHostEnvironment _env;
        // private readonly BlobServiceClient _blobServiceClient; // Para produção

        public FotoStorageService(IWebHostEnvironment env /*, BlobServiceClient blobServiceClient */)
        {
            _env = env;
            // _blobServiceClient = blobServiceClient;
        }

        public async Task<string> SalvarFotoAsync(IFormFile file)
        {
            // --- Blob Storage (produção) ---
            /*
            var container = _blobServiceClient.GetBlobContainerClient("utensilios");
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var blobClient = container.GetBlobClient(fileName);
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
            return blobClient.Uri.ToString();
            */

            // --- Local (desenvolvimento/teste) ---
            var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads", "utensilios");
            Directory.CreateDirectory(uploadsFolder);
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            // Caminho relativo para servir via API
            return Path.Combine("uploads", "utensilios", fileName).Replace("\\", "/");
        }
    }
}

