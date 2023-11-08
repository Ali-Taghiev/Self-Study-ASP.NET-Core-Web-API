using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repostories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        //POST /api/Images/Upload
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto ,
            IImageRepository imageRepository)
        {
            ValidateFileUpload(imageUploadRequestDto);

            if(ModelState.IsValid)
            {

                //Convert Dto to DomainModel 
                var ImageDomainModel = new Image
                {
                    File = imageUploadRequestDto.File,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.File.FileName).ToLower(),
                    FileSizeInBytes = imageUploadRequestDto.File.Length,
                    FileName = imageUploadRequestDto.FileName,
                    FileDescription = imageUploadRequestDto.FileDescription,



                };



                //User repository to upload image
                await imageRepository.Upload(ImageDomainModel);

                return Ok(ImageDomainModel);

            }

            return BadRequest(ModelState);

        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtension.Contains(Path.GetExtension(imageUploadRequestDto.File.FileName)))
            {

                ModelState.AddModelError("file", "Unsupported file extension");
            }
            if(imageUploadRequestDto.File.Length> 10485760) {

                ModelState.AddModelError("file","File size more than 10MB");
            
            }

        }
    }
}
