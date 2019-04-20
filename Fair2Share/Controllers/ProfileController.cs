using System;
using System.Collections.Generic;
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

        //[HttpGet("email")]
        //[Authorize]
        //public ActionResult<Profile> GetBy(string email) {
        //    //FriendsDTO friend = new FriendsDTO { id = _profileRepository.GetBy(id).ProfileId };
        //    return _profileRepository.GetBy(email);
        //}


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
    }
}