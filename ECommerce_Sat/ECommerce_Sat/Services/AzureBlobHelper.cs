using ECommerce_Sat.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ECommerce_Sat.Services
{
    public class AzureBlobHelper : IAzureBlobHelper
    {
        private readonly CloudBlobClient _cloudBlobClient;

        public AzureBlobHelper(IConfiguration configuration)
        {
            //Aquí capturo mi cadena de conexión desde el appsettings.json
            string keys = configuration["Blob:AzureStorage"];

            //Con esta propiedade envío mi Cadena de conexión al azure para poder acceder a él.
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(keys);

            //Con esta propiedad, ya puedo acceder a subir y bajar fotos desde mi storage.
            _cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<Guid> UploadAzureBlobAsync(IFormFile file, string containerName)
        {
            Stream stream = file.OpenReadStream(); // Arreglo en memoria de un archivo
            return await UploadAzureBlobAsync(stream, containerName);
        }

        public async Task<Guid> UploadAzureBlobAsync(string image, string containerName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadAzureBlobAsync(stream, containerName);
        }

        public async Task DeleteAzureBlobAsync(Guid id, string containerName)
        {
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{id}");
            await blockBlob.DeleteAsync();
        }

        private async Task<Guid> UploadAzureBlobAsync(Stream stream, string containerName)
        {
            Guid nameGuid = Guid.NewGuid();

            //Accedo al container
            CloudBlobContainer container = _cloudBlobClient.GetContainerReference(containerName);

            //Crea el blob con mi nombre
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{nameGuid}");

            //Subo la foto al blob
            await blockBlob.UploadFromStreamAsync(stream);

            return nameGuid;
        }
    }
}
