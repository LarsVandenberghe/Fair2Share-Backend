using Fair2Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class ProfileDTO {
        public long ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public virtual ICollection<FriendDTO> Friends { get; set; }
        public virtual ICollection<ActivityDTO> Activities { get; set; }
        public int AmountOfFriendRequests { get; set; }

        public ProfileDTO() {

        }

        public ProfileDTO(Profile profile) {
            ProfileId = profile.ProfileId;
            Firstname = profile.Firstname;
            Lastname = profile.Lastname;
            Email = profile.Email;
            Friends = profile.Friends.ToList().Select(p => {
                Profile profile_temp = p.Friend;
                return new FriendDTO {
                    ProfileId = profile_temp.ProfileId,
                    Firstname = profile_temp.Firstname,
                    Lastname = profile_temp.Lastname,
                };
            }).ToList();
            Activities = profile.Activities.ToList().Select(a => {
                Activity activity = a.Activity;
                return new ActivityDTO {
                    ActivityId = activity.ActivityId,
                    Name = activity.Name,
                    Description = activity.Description
                };
            }).ToList();
            AmountOfFriendRequests = profile.ReceivedFriendRequests.Count( p => p.State == FriendRequestState.NEW);
        }
    }
}
