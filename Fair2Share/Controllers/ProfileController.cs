using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fair2Share.DTOs;
using Fair2Share.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fair2Share.Controllers {
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProfileController : ControllerBase {
        private readonly IProfileRepository _profileRepository;
        public static string[] ALLOWED_IMAGE_EXT = { "jpg", "jpeg", "gif", "tiff", "bmp", "png" };
        public ProfileController(IProfileRepository profileRepository) {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ProfileDTO> GetProfileDTO() {
            ProfileDTO profile = new ProfileDTO(_profileRepository.GetBy(User.Identity.Name));
            return profile;
        }

        [HttpPut("simple")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateProfile(SimpleProfileDTO profileDTO) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            try {
                profile.Update(profileDTO);
            } catch (Exception e) {
                return BadRequest(e.Message);
            }
            _profileRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("simple")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<SimpleProfileDTO> GetSimpleProfileDTO() {
            SimpleProfileDTO profile = new SimpleProfileDTO(_profileRepository.GetBy(User.Identity.Name));
            return profile;
        }

        [HttpPost("image")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PostProfileImage([FromForm]IFormFile file) {
            if (file.Length > 20000000) {
                return BadRequest("Your file is too big.");
            }
            var fileNameSplit = file.FileName.Split(".");
            string extension = fileNameSplit[fileNameSplit.Length - 1];
            if (!ALLOWED_IMAGE_EXT.Contains(extension.ToLower())) {
                return BadRequest("Your file is not an image.");
            }
            Profile profile = _profileRepository.GetBy(User.Identity.Name);

            string name = file.FileName.Substring(0, file.FileName.Length - extension.Length - 2);
            using (var ms = new MemoryStream()) {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                profile.ProfileImage = new ProfileImage { Image = fileBytes, Profile = profile, FileName = name, Extension = extension };
            }
            _profileRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("image/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfileImage(long id) {
            Profile profile = _profileRepository.GetProfileWithImage(id);
            var image = profile.ProfileImage;
            if (image == null) {
                return NotFound();
            }
            return File(image.Image, $"image/{image.Extension.ToLower()}", $"{image.FileName}.{image.Extension}");
        }

        [HttpDelete("image")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult ResetProfileImage() {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            profile.ProfileImage = null;
            _profileRepository.SaveChanges();
            return NoContent();
        }

    }
}