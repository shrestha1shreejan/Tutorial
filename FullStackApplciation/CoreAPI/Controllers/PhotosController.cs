using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayerAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;

namespace CoreAPI.Controllers
{
    [Authorize]
    [Route("Data/{userId}/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _clodinaryConfig;
        private readonly IPhotoRepository _photoRepo;
        private readonly ICosmosManager _repo;

        #region Constructor

        public PhotosController(IOptions<CloudinarySettings> cloudinaryConfig, ICosmosManager repo, IPhotoRepository photoRepo)
        {
            _clodinaryConfig = cloudinaryConfig;
            _photoRepo = photoRepo;
            _repo = repo;
        }

        #endregion

        /// <summary>
        /// Gets user photo based on photo id     
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhotos(string userId, int id)
        {

            var photos = await _repo.GetPhotoById(userId, id);
            var photoToReturn = Helpers.HelperMapper.MapPhotoToPhotoForReturnDto(photos);
            return Ok(photoToReturn);
        }

        /// <summary>
        /// Add photos
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="photoForCreationDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(string userId, [FromForm]  PhotoForCreationDto photoForCreationDto)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }
            
            var userFromRepo = await _repo.GetPersonById(userId);

            var file = photoForCreationDto.File;

            var photoForCreation = await _photoRepo.UploadPhotoToCloud(photoForCreationDto, _clodinaryConfig);

            var photo = Helpers.HelperMapper.MapPhotoForCreationDtoToPhoto(photoForCreation);

            // check if there is no main Photo for the person
            if (userFromRepo.Photos != null)
            {
                if (!userFromRepo.Photos.Any(p => p.IsMain))
                {
                    photo.IsMain = true;
                }
                var currentPhototCount = userFromRepo.Photos.Count();
                photo.Id = userFromRepo.Photos.Last().Id + 1;
            } 
            else
            {
                photo.Id = 0;
                photo.IsMain = true;
            }

            if (userFromRepo.Photos == null)
            {
                userFromRepo.Photos = new List<Photo>();
            }
            
            userFromRepo.Photos.Add(photo);

            var response = await _repo.UpdatePersonsData(userId, userFromRepo);

            if (response == HttpStatusCode.OK)
            {
                var phototToReturn = Helpers.HelperMapper.MapPhotoToPhotoForReturnDto(photo);
                return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, phototToReturn);
            }


            return BadRequest("Adding photo failed");
        }


        /// <summary>
        /// Set main photo
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(string userId, int id)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetPersonById(userId);

            var requestedPhoto = userFromRepo.Photos.FirstOrDefault(x => x.Id == id);
            if (requestedPhoto == null)
            {
                return Unauthorized();
            }           

            if (requestedPhoto.IsMain)
            {
                return BadRequest("Requested photo is already the main photo");
            }

            foreach (var photo in userFromRepo.Photos)
            {
                if (photo.Id == id)
                {
                    photo.IsMain = true;
                } else
                {
                    photo.IsMain = false;
                }
            }

            var response = await _repo.UpdatePersonsData(userId, userFromRepo);

            if (response == HttpStatusCode.OK)
            {
                return NoContent();
            }

            return BadRequest("Updating the main photo failed");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(string userId, int id)
        {
            if (userId != User.FindFirst(ClaimTypes.NameIdentifier).Value)
            {
                return Unauthorized();
            }

            var userFromRepo = await _repo.GetPersonById(userId);

            var requestedPhoto = userFromRepo.Photos.FirstOrDefault(x => x.Id == id);
            if (requestedPhoto == null)
            {
                return Unauthorized();
            }            

            if (requestedPhoto.IsMain)
            {
                return BadRequest("Main photo cannot be deleted");
            }



            if (requestedPhoto.PublicId != null)
            {
                var isPhotoDeleted = await _photoRepo.DeleteCloudPhoto(requestedPhoto.PublicId, _clodinaryConfig);

                if (isPhotoDeleted)
                {
                    List<Photo> photos = new List<Photo>();
                    foreach (var photo in userFromRepo.Photos)
                    {
                        if (photo.Id != id)
                        {
                            photos.Add(photo);
                        }
                    }

                    userFromRepo.Photos = photos;
                    var response = await _repo.UpdatePersonsData(userId, userFromRepo);
                    if (true)
                    {
                        return Ok();
                    }
                }
            }

            if (requestedPhoto.PublicId == null)
            {
                List<Photo> photos = new List<Photo>();
                foreach (var photo in userFromRepo.Photos)
                {
                    if (photo.Id != id)
                    {
                        photos.Add(photo);
                    }
                }

                userFromRepo.Photos = photos;
                var response = await _repo.UpdatePersonsData(userId, userFromRepo);
                if (response == HttpStatusCode.OK)
                {
                    return Ok();
                }
            }


            return BadRequest("Failed to delete the photo");
        }

    }
}