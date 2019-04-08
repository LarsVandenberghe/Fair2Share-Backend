using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Profile {
        public int ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PathToImage { get; set; }
        
        public virtual ICollection<Friends> Friends { get; set; }
        public virtual ICollection<Friends> FriendOf { get; set; }

        public void AddFriend(Profile friend) {
            //has friend not in friendlist
            //if (Friends.Where(p => p.ProfileId == friend.ProfileId).SingleOrDefault() == null) {
            //    Friends friends = new Friends { Profile = this, Friend = friend };
            //    Friends.Add(friends);
            //    FriendOf.Add(friends);
            //}
            //if (friend.Friends.Where(p => p.ProfileId == this.ProfileId).SingleOrDefault() == null) {
            //    friend.AddFriend(this);
            //}
        }

        public Profile GetFriend(string email) {
            return Friends.SingleOrDefault(e => e.Friend.Email.Equals(email)).Friend;
        }
    }
}
