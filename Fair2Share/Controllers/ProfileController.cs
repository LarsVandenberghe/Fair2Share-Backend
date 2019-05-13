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
            Profile profile = _profileRepository.GetProfileWithImage(User.Identity.Name);
            using (var ms = new MemoryStream()) {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                profile.ProfileImage = new ProfileImage { Image = fileBytes, Profile = profile };
                //string s = Convert.ToBase64String(fileBytes);
                // act on the Base64 data
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
            return File(image.Image, "image/jpg", "profile_picture.jpg");
        }
    }
}