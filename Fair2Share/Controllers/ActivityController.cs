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
        private readonly IActivityRepository _activityRepository;

        public ActivityController(IProfileRepository profileRepository, IActivityRepository activityRepository) {
            _profileRepository = profileRepository;
            _activityRepository = activityRepository;
        }

        //TODO: controlleer op randgevallen wat kan er mis gaan? (activity null?)
        //TODO: bulk add friends
        //TODO: Update methodes
        //TODO: verwijderen
        //TODO: controleer CRRUD
        //TODO: commentaar

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<ActivityDTO>> GetAll() {
            return _profileRepository.GetBy(User.Identity.Name).Activities.Select(p => new ActivityDTO { ActivityId = p.Activity.ActivityId, Name = p.Activity.Name, CurrencyType = p.Activity.CurrencyType, Description = p.Activity.Description }).ToList();
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

        //TODO UPDATE?
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

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ActivityDTO> GetBy(long id) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }
            return new ActivityDTO(activity);
        }

        [HttpPost("{id}/participants/{friend_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddFriend(long id, long friend_id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile friend = profile.Friends.Where(f => f.FriendId == friend_id).Select(f => f.Friend).FirstOrDefault();
            if (friend == null) {
                return BadRequest("You cannot add this profile as it's not a friend of yours.");
            }
            Activity activity = _activityRepository.GetBy(id);
            if (activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }
            activity.Participants.Add(new ProfileActivityIntersection { Activity = activity, Profile = friend });
            _activityRepository.Update(activity);
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ActivityDTO> GetParticipants(long id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Activity activity = _activityRepository.GetBy(id);

            if (profile.Activities.Where(a => a.ActivityId == id).SingleOrDefault() == null) {
                return BadRequest("You are not in this activity.");
            }
            return new ActivityDTO(activity);
        }

        [HttpPost("{id}/transactions/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult CreateTransaction(long id, TransactionDTO transaction) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile paidBy = _profileRepository.GetBy(transaction.PaidBy.ProfileId);

            Activity activity = _activityRepository.GetBy(id);
            if (activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            activity.Transactions.Add(new Transaction(transaction, paidBy));
            _activityRepository.Update(activity);
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/transactions/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<TransactionDTO>> GetTransactions(long id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);

            Activity activity = _activityRepository.GetBy(id);
            if (activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            return _activityRepository.GetBy(id).Transactions.Select(t => new TransactionDTO {
                TransactionId = t.TransactionId,
                Name = t.Name,
                Description = t.Description,
                Payment = t.Payment,
                PaidBy = new FriendDTO(t.PaidBy),
                TimeStamp = t.TimeStamp,
                ProfilesInTransaction = t.ProfilesInTransaction.Select(p => new FriendDTO(p.Profile)).ToList()
            }).ToList();
        }

        [HttpPost("{id}/transactions/{transaction_id}/participants/{friend_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddFriendToTransaction(long id, long transaction_id, long friend_id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile friend = profile.Friends.Where(f => f.FriendId == friend_id).SingleOrDefault().Friend;

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            if (friend == null) {
                return BadRequest("You cannot add this profile as it's not a friend of yours.");
            }
            Transaction transaction = activity.Transactions.Where(t => t.TransactionId == transaction_id).SingleOrDefault();

            transaction.ProfilesInTransaction.Add(new ProfileTransactionIntersection { Profile = friend, Transaction = transaction });
            _activityRepository.SaveChanges();
            return NoContent();
        }
    }
}