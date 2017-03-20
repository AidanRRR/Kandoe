using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using kandoe_service_image.Models;
using Microsoft.Extensions.Options;

namespace kandoe_service_image.Services
{
    public interface IImageSender
    {
        Task<Models.ImageBlobResult> PostImage(IFormFile file);
    }

    public class ImageSender: IImageSender {

        private CloudBlockBlob _blockBlob;
        private CloudBlobContainer _container;

        public ImageSender(IOptions<AzureConfig> azureConfig)
        {
            var connectionString = azureConfig.Value.BlobConnectionString;

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            _container = blobClient.GetContainerReference(azureConfig.Value.ContainerReference);
        }

        public ImageSender(string connectionString, string containerReference)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            _container = blobClient.GetContainerReference(containerReference);

        }

        public async Task<Models.ImageBlobResult> PostImage(IFormFile file) {

            _blockBlob = _container.GetBlockBlobReference(file.FileName);

            using (var fileStream = file.OpenReadStream()) {
                    await _blockBlob.UploadFromStreamAsync(fileStream);
                }

            return new Models.ImageBlobResult(_blockBlob.Name, _blockBlob.Uri.ToString(), _blockBlob.Properties.Length.ToString());
        }

        
    }
}