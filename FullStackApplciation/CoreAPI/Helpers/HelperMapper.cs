using ModelsLibrary.DataModels;
using ModelsLibrary.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Helpers
{
    public static class HelperMapper
    {
      
        #region Mappings

        public static UserForDetailDto MapUserToUserForDetailDto(Person person)
        {
            var usersToReturn = new UserForDetailDto();             
            usersToReturn.Id = person.Id;
            usersToReturn.Username = person.Username;
            usersToReturn.Gender = person.Gender;
            usersToReturn.Age = CalculateAge(person.DateOfBirth);
            usersToReturn.KnownAs = person.KnownAs;
            usersToReturn.Created = person.Created;
            usersToReturn.LastActive = person.LastActive;
            usersToReturn.Introduction = person.Introduction;
            usersToReturn.LookingFor = person.LookingFor;
            usersToReturn.Interests = person.Interests;
            usersToReturn.City = person.City;
            usersToReturn.Country = person.Country;
            usersToReturn.PhotoUrl = person.Photos != null ? person.Photos.FirstOrDefault(x => x.IsMain).Url : default;
            if (person.Photos != null)
            {
                usersToReturn.Photos = MapPhotos(person.Photos);
            }           

            return usersToReturn;
        }


        public static Person MapUserToUserForUpdateDto(Person person, UserForUpdateDtos updatedUser)
        {
            person.City = updatedUser.City;
            person.Country = updatedUser.Country;
            person.Interests = updatedUser.Interests;
            person.Introduction = updatedUser.Introduction;
            person.LookingFor = updatedUser.LookingFor;

            return person;
        }

        public static Photo MapPhotoForCreationDtoToPhoto(PhotoForCreationDto photoForCreationDto)
        {
            var photo = new Photo
            {
                Description = photoForCreationDto.Description,
                Url = photoForCreationDto.Url,
                DateAdded = photoForCreationDto.DateAdded,
                PublicId = photoForCreationDto.PublicId
            };

            return photo;
        }

        public static PhotoForReturnDto MapPhotoToPhotoForReturnDto(Photo photo)
        {
            var photoForReturnDto = new PhotoForReturnDto { 
                DateAdded = photo.DateAdded,
                Description = photo.Description,
                Url = photo.Url,
                IsMain = photo.IsMain,
                PublicId = photo.PublicId,
                Id = photo.Id
            };

            return photoForReturnDto;
        }

        public static List<UserForListDto> MapUserToUserForListDto(IEnumerable<Person> persons)
        {
            List<UserForListDto> usersToReturn = new List<UserForListDto>();
            foreach (var person in persons)
            {
                var p = new UserForListDto
                {
                    Id = person.Id,
                    Username = person.Username,
                    Gender = person.Gender,
                    Age = CalculateAge(person.DateOfBirth),
                    KnownAs = person.KnownAs,
                    Created = person.Created,
                    LastActive = person.LastActive,
                    Introduction = person.Introduction,
                    LookingFor = person.LookingFor,
                    Interests = person.Interests,
                    City = person.City,
                    Country = person.Country,
                    PhotoUrl = person.Photos != null ? person.Photos.FirstOrDefault(x => x.IsMain).Url : default                  
                };

                usersToReturn.Add(p);
            }
           

            return usersToReturn;
        }

        public static UserForLSDto MapUserToUserForLStDto(Person person)
        {
            var user = new UserForLSDto();

            user.Id = person.Id;
            user.Username = person.Username;
            user.Gender = person.Gender;
            user.Age = CalculateAge(person.DateOfBirth);
            user.KnownAs = person.KnownAs;
            user.Created = person.Created;
            user.LastActive = person.LastActive;
            user.Introduction = person.Introduction;
            user.LookingFor = person.LookingFor;
            user.Interests = person.Interests;
            user.City = person.City;
            user.Country = person.Country;
            user.PhotoUrl = person.Photos != null ? person.Photos.FirstOrDefault(x => x.IsMain).Url : default;

            return user;
        }

        public static UserForCreatedDtos MapPersonToUserForCreatedDto(Person person)
        {
            var user = new UserForCreatedDtos
            {
                Id = person.Id,
                Username = person.Username,
                Gender = person.Gender,
                Age = CalculateAge(person.DateOfBirth),
                KnownAs = person.KnownAs,
                Created = person.Created,
                LastActive = person.LastActive,                
                City = person.City,
                Country = person.Country
            };

            return user;
        }

        private static List<PhotosForDetailDto> MapPhotos(ICollection<Photo> photo)
        {
            List<PhotosForDetailDto> p = new List<PhotosForDetailDto>();
            foreach (var item in photo)
            {
                var signlePhoto = new PhotosForDetailDto
                {
                    DateAdded = item.DateAdded,
                    Description = item.Description,
                    Id = item.Id,
                    Url = item.Url,
                    IsMain = item.IsMain
                };

                p.Add(signlePhoto);
            }

            return p;
        }


        public static Person MapPersonForRegistrationDtoToPerson(UserForRegisterDto user)
        {
            var person = new Person {
                Username = user.Username,
                Gender = user.Gender,
                DateOfBirth = user.DateOfBirth,
                Created = user.Created,
                LastActive = user.LastActive,
                City = user.City,
                Country = user.Country,
                KnownAs = user.KnownAs,                
            };

            return person;
        }

        private static int CalculateAge(this DateTime dateTime)
        {
            var age = DateTime.Today.Year - dateTime.Year;
            if (dateTime.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }

        #endregion
    }
}
