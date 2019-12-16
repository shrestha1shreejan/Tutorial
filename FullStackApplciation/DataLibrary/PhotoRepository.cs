using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DataLayerAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary
{
    public class PhotoRepository : IPhotoRepository
    {        
       
        #region Implementation


        public async Task<PhotoForCreationDto> UploadPhotoToCloud(PhotoForCreationDto photo, IOptions<CloudinarySettings> cloudconfig)
        {
            var cloudinary =  CreateCloudinaryInstance(cloudconfig);
            var file = photo.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = await cloudinary.UploadAsync(uploadParams);
                }
            }

            photo.Url = uploadResult.Uri.ToString();
            photo.PublicId = uploadResult.PublicId;

            return photo;
        }

        public async Task<bool> DeleteCloudPhoto(string publicId, IOptions<CloudinarySettings> cloudconfig)
        {

            var cloudinary = CreateCloudinaryInstance(cloudconfig);
            var deleteionParams = new DeletionParams(publicId);
            var result = await cloudinary.DestroyAsync(deleteionParams);

            if (result.Result == "ok")
            {
                return true;
            }

            return false;
        }

        #endregion


        #region Private methods

        private Cloudinary CreateCloudinaryInstance (IOptions<CloudinarySettings> cloudconfig)
        {
            Account acc = new Account
            (
                cloudconfig.Value.CloudName,
                cloudconfig.Value.APIKey,
                cloudconfig.Value.APISecret
            );


            Cloudinary cloudinary = new Cloudinary(acc);
            return cloudinary;
        }

        #endregion
    }
}
