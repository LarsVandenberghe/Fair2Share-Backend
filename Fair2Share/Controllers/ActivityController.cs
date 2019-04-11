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
    public class ActivityController : ControllerBase {

        private readonly IProfileRepository _profileRepository;

        public ActivityController(IProfileRepository profileRepository) {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<ActivityDTO>> GetAll() {
            return _profileRepository.GetBy(User.Identity.Name).Activities.Select(p => new ActivityDTO(p.Activity)).ToList();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddActivity(InitActivityDTO activityDTO) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Activity activity = new Activity { Name = activityDTO.Name, Description = activityDTO.Description, CurrencyType = activityDTO.CurrencyType };
            profile.Activities.Add(new ProfileActivityIntersection { Activity = activity, Profile = profile });
            _profileRepository.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Put(long id, ActivityDTO activity) {
            if (id != activity.ActivityId) {
                return BadRequest();
            }
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            ProfileActivityIntersection pai = profile.Activities.Where(p => p.ActivityId == activity.ActivityId).FirstOrDefault();
            if (pai == null) {
                return BadRequest();
            }
            pai.Activity.Update(activity);
            _profileRepository.SaveChanges();
            return NoContent();
        }
    }
}