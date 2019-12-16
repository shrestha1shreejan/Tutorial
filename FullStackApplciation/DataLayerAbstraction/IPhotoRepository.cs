using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;
using System.Threading.Tasks;

namespace DataLayerAbstraction
{
    public interface IPhotoRepository
    {
        Task<PhotoForCreationDto> UploadPhotoToCloud(PhotoForCreationDto photo, IOptions<CloudinarySettings> cloudconfig);
        Task<bool> DeleteCloudPhoto(string publicId, IOptions<CloudinarySettings> cloudconfig);
    }
}
