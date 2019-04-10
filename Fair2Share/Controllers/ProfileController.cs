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

namespace Fair2Share.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class ProfileController : ControllerBase
    {
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
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<Profile> GetProfile() {
            return _profileRepository.GetBy(User.Identity.Name);
        }
        //[HttpGet]
        //public ActionResult<IEnumerable<Profile>> GetFriendsOf(int id) {
        //    IEnumerable<Profile> friends = new HashSet<Profile>();
        //    _profileRepository.GetBy(id).Friends.ToList().ForEach(e => friends.Append(e.Friend));
        //    return Ok(friends);
        //}
    }
}