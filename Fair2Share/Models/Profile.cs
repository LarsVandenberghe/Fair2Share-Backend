using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Profile {
        public long ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PathToImage { get; set; }
        
        public virtual IEnumerable<Friends> Friends { get; set; }
        public virtual IEnumerable<Friends> FriendOf { get; set; }
        public virtual IEnumerable<ProfileActivityIntersection> Activities { get; set; }

        public Profile() {
            Friends = new HashSet<Friends>();
            FriendOf = new HashSet<Friends>();
        }

        public void AddFriend(Profile friend) {
            //has friend not in friendlist
            if (Friends.Where(p => p.ProfileId == friend.ProfileId).SingleOrDefault() == null) {
                DateTime timeStamp = DateTime.Now;
                Friends friends1 = new Friends { ProfileId = this.ProfileId, Profile = this, FriendId=friend.ProfileId, Friend = friend, TimeStamp = timeStamp };
                Friends friends2 = new Friends { ProfileId = friend.ProfileId, Profile = friend, FriendId = this.ProfileId, Friend = this, TimeStamp = timeStamp };
                Friends.Append(friends1);
                FriendOf.Append(friends2);
            }
            //if (friend.Friends.Where(p => p.ProfileId == this.ProfileId).SingleOrDefault() == null) {
            //    friend.AddFriend(this);
            //}
        }
    }
}
