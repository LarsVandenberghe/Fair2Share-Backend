using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class FriendRequestController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public FriendRequestController(IProfileRepository profileRepository) {
            _profileRepository = profileRepository;
        }

        //[HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //public ActionResult<Profile> GetProfile() {
        //    return _profileRepository.GetBy(User.Identity.Name);
        //}
    }
}