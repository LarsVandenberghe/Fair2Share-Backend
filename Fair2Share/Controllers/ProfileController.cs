using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fair2Share.DTOs;
using Fair2Share.Models;
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

        [HttpGet]
        public ActionResult<Profile> GetBy(int id) {
            //FriendsDTO friend = new FriendsDTO { id = _profileRepository.GetBy(id).ProfileId };
            return _profileRepository.GetBy(id);
        }

        //[HttpGet]
        //public ActionResult<IEnumerable<Profile>> GetFriendsOf(int id) {
        //    IEnumerable<Profile> friends = new HashSet<Profile>();
        //    _profileRepository.GetBy(id).Friends.ToList().ForEach(e => friends.Append(e.Friend));
        //    return Ok(friends);
        //}
    }
}