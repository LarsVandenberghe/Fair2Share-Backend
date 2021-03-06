﻿using System;
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

        //TODO: commentaar

        /* Activitties */

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<ActivityDTO>> GetAll() {
            return _profileRepository.GetBy(User.Identity.Name).Activities.Select(p => new ActivityDTO { ActivityId = p.Activity.ActivityId, Name = p.Activity.Name, CurrencyType = p.Activity.CurrencyType, Description = p.Activity.Description }).ToList();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<long> AddActivity(InitActivityDTO activityDTO) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Activity activity = new Activity { Name = activityDTO.Name, Description = activityDTO.Description, CurrencyType = activityDTO.CurrencyType };
            profile.Activities.Add(new ProfileActivityIntersection { Activity = activity, Profile = profile });
            _profileRepository.SaveChanges();
            return activity.ActivityId;
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateActivity(long id, ActivityDTO activity) {
            if (id != activity.ActivityId) {
                return BadRequest();
            }
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            ProfileActivityIntersection pai = profile.Activities.Where(p => p.ActivityId == activity.ActivityId).FirstOrDefault();
            if (pai == null) {
                return BadRequest();
            }
            try { pai.Activity.Update(activity); } catch (Exception e) { return BadRequest(e.Message); }

            _profileRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ActivityDTO> GetBy(long id) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity is not valid.");
            }

            return new ActivityDTO(activity);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteBy(long id) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity is not valid.");
            }

            if (activity.Transactions.Count == 0) {
                _activityRepository.Delete(activity);
            } else {
                return BadRequest("Activity cannot be deleted because it has transactions.");
            }

            _activityRepository.SaveChanges();
            return NoContent();
        }


        /* Participants */

        [HttpPost("{id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddFriends(long id, IEnumerable<long> friend_ids) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            ICollection<Profile> friends = new List<Profile>();

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid.");
            }
            Profile friend;
            foreach (var friend_id in friend_ids) {
                friend = profile.Friends.Where(f => f.FriendId == friend_id).Select(f => f.Friend).FirstOrDefault();
                if (friend == null) {
                    if (friend_id != profile.ProfileId) {
                        return BadRequest("You cannot add this profile as it's not a friend of yours.");
                    } else {
                        friend = profile;
                    }
                }
                if (activity.Participants.Where(p => p.Profile.Email == friend.Email).SingleOrDefault() != null) {
                    return BadRequest("Friend is already in activity.");
                }
                friends.Add(friend);
            }

            foreach (var friend2 in friends) {
                activity.Participants.Add(new ProfileActivityIntersection { Activity = activity, Profile = friend2 });
            }

            _activityRepository.Update(activity);
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ActivityDTO> GetParticipants(long id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null) {
                return NotFound("Activity id not valid.");
            }

            if (profile.Activities.Where(a => a.ActivityId == id).SingleOrDefault() == null) {
                return BadRequest("You are not in this activity.");
            }
            return new ActivityDTO(activity);
        }

        [HttpDelete("{id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ActivityDTO> DeleteParticipants(long id, [FromQuery]IEnumerable<long> friend_ids) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Activity activity = _activityRepository.GetBy(id);
            ICollection<ProfileActivityIntersection> friends = new List<ProfileActivityIntersection>();
            if (activity == null) {
                return NotFound("Activity id not valid.");
            }

            if (profile.Activities.Where(a => a.ActivityId == id).SingleOrDefault() == null) {
                return BadRequest("You are not in this activity.");
            }
            ProfileActivityIntersection friend;
            foreach (var friend_id in friend_ids) {
                friend = activity.Participants.Where(p => p.ProfileId == friend_id).SingleOrDefault();
                if (friend == null) {
                    return BadRequest($"You cannot remove this profile ({friend_id}), as it's not in the activity.");
                }

                if (activity.Transactions.FirstOrDefault(t => t.PaidBy.ProfileId == friend_id) != null) {
                    return BadRequest($"Profile with id {friend_id}, is in a transaction.");
                }
                if (activity.Transactions.FirstOrDefault(t => t.ProfilesInTransaction.FirstOrDefault(q => q.ProfileId == friend_id) != null) != null) {
                    return BadRequest($"Profile with id {friend_id}, is in a transaction.");
                }

                friends.Add(friend);
            }


            foreach (var friend2 in friends) {
                activity.Participants.Remove(friend2);
            }
            _activityRepository.SaveChanges();
            return NoContent();
        }

        /* Transactions */

        [HttpPost("{id}/transactions/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<long> CreateTransaction(long id, TransactionDTO transaction) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile paidBy = _profileRepository.GetBy(transaction.PaidBy.ProfileId);
            if (paidBy == null) {
                return BadRequest("PaidBy is not valid.");
            }

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            if (transaction.Payment < 0) {
                return BadRequest("Payment cannot be negative.");
            }
            Transaction t = new Transaction(transaction, paidBy);
            activity.Transactions.Add(t);
            _activityRepository.Update(activity);
            _activityRepository.SaveChanges();
            return t.TransactionId;
        }

        [HttpPut("{id}/transactions/{transaction_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult UpdateTransaction(long id, long transaction_id, TransactionDTO transactionDTO) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return NotFound("Activity id not valid");
            }

            if (transaction_id != transactionDTO.TransactionId) {

            }
            Transaction transaction = activity.Transactions.Where(t => t.TransactionId == transaction_id).SingleOrDefault();
            ProfileActivityIntersection paidBy = activity.Participants.Where(p => p.ProfileId == transactionDTO.PaidBy.ProfileId).SingleOrDefault();
            if (transaction == null) {
                return BadRequest("transaction does not exist for activity.");
            }
            if (paidBy == null) {
                return BadRequest("PaidBy is not a member of the activity.");
            }
            if (transactionDTO.Payment < 0) {
                return BadRequest("Payment cannot be negative.");
            }

            transaction.Update(transactionDTO, paidBy.Profile);
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/transactions/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<TransactionDTO>> GetTransactions(long id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
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

        [HttpGet("{id}/transactions/{transaction_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<TransactionDTO> GetTransaction(long id, long transaction_id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            Transaction t = _activityRepository.GetTransactionFromActivity(id, transaction_id);

            if (t == null) {
                return BadRequest("Transaction id not valid");
            }
            return new TransactionDTO {
                TransactionId = t.TransactionId,
                Name = t.Name,
                Description = t.Description,
                Payment = t.Payment,
                PaidBy = new FriendDTO(t.PaidBy),
                TimeStamp = t.TimeStamp,
                ProfilesInTransaction = t.ProfilesInTransaction.Select(p => new FriendDTO(p.Profile)).ToList()
            };
        }

        [HttpDelete("{id}/transactions/{transaction_id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult DeleteTransaction(long id, long transaction_id) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            Transaction transaction = _activityRepository.GetTransactionFromActivity(id, transaction_id);//activity.Transactions.Where(t => t.TransactionId == transaction_id).SingleOrDefault();
            if (transaction == null) {
                return BadRequest("Transaction id does not exist for activity.");
            }

            //if (transaction.ProfilesInTransaction.Count != 0) {
            //    return BadRequest("Transaction has Participants.");
            //}

            activity.Transactions.Remove(transaction);
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpPost("{id}/transactions/{transaction_id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult AddFriendsToTransaction(long id, long transaction_id, IEnumerable<long> friend_ids) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            ICollection<Profile> friends = new List<Profile>();

            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity id not valid");
            }

            Transaction transaction = null;
            try {
                transaction = _activityRepository.GetTransactionFromActivity(id, transaction_id);
            } catch (Exception e) {
                return BadRequest("Activity id not valid");
            }

            Profile friend;
            ProfileActivityIntersection profileActintersection;
            foreach (var friend_id in friend_ids) {
                profileActintersection = activity.Participants.Where(f => f.ProfileId == friend_id).SingleOrDefault();

                if (profileActintersection == null) {
                    if (friend_id != profile.ProfileId) {
                        return BadRequest($"You cannot add this profile with id {friend_id}, as he is not in this activity.");
                    } else {
                        friend = profile;
                    }
                } else {
                    friend = profileActintersection.Profile;
                }
                if (transaction.ProfilesInTransaction.Where(p => p.ProfileId == friend_id).SingleOrDefault() != null) {
                    return BadRequest($"Profile {friend_id} is already in transaction.");
                }

                friends.Add(friend);
            }


            foreach (var friend2 in friends) {
                transaction.ProfilesInTransaction.Add(new ProfileTransactionIntersection { Profile = friend2, Transaction = transaction });
            }

            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}/transactions/{transaction_id}/participants/")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult RemoveParticipantsFromTransaction(long id, long transaction_id, [FromQuery]IEnumerable<long> friend_ids) {
            ICollection<ProfileTransactionIntersection> friends = new List<ProfileTransactionIntersection>();

            Transaction transaction = null;
            try {
                transaction = _activityRepository.GetTransactionFromActivity(id, transaction_id);
            } catch (Exception e) {
                return BadRequest("Activity id not valid");
            }
            
            if (transaction == null) {
                return BadRequest("Transaction id not valid");
            }

            foreach (var friend_id in friend_ids) {
                ProfileTransactionIntersection pti = transaction.ProfilesInTransaction.Where(p => p.ProfileId == friend_id).SingleOrDefault();
                if (pti == null) {
                    return BadRequest($"Profile with id {friend_id}, does not belong to this transaction.");
                }
                friends.Add(pti);
            }

            foreach (var friend in friends) {
                transaction.ProfilesInTransaction.Remove(friend);
            }
            _activityRepository.SaveChanges();
            return NoContent();
        }

        [HttpGet("{id}/summary")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<IDictionary<long, decimal>> GetSummary(long id) {
            Activity activity = _activityRepository.GetBy(id);
            if (activity == null || activity.Participants.Where(a => a.Profile.Email == User.Identity.Name).SingleOrDefault() == null) {
                return BadRequest("Activity is not valid.");
            }
            IDictionary<long, decimal> dict = new Dictionary<long, decimal>();

            foreach (var user in activity.Participants) {
                dict.Add(user.ProfileId, activity.Transactions.Where(t => t.PaidBy.ProfileId == user.ProfileId).Sum(t => t.Payment));
            }
            decimal toBePaid;
            int amoutnOfMembers;
            Transaction transaction;
            foreach (var transactiontemp in activity.Transactions) {
                transaction = _activityRepository.GetTransactionFromActivity(id, transactiontemp.TransactionId);
                amoutnOfMembers = transaction.ProfilesInTransaction.Count;
                if (amoutnOfMembers != 0) {
                    toBePaid = transaction.Payment / amoutnOfMembers;
                    transaction.ProfilesInTransaction.ToList().ForEach(p => {
                        dict[p.ProfileId] -= toBePaid;
                    });
                } else {
                    dict[transaction.PaidBy.ProfileId] -= transaction.Payment;
                }
            }
            return Ok(dict);
        }
    }
}