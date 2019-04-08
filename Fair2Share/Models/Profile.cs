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
        
        //public ICollection<Friends> Friends { get; set; }

        //public void AddFriend(Profile friend) {
        //    //Friends.Add(new Friends { User = this, Friend = friend });
        //    Friends.Add(friend);
        //}

        //public Profile GetFriend(string email) {
        //    //return Friends.SingleOrDefault(e => e.Friend.Email.Equals(email)).Friend;
        //    return Friends.SingleOrDefault(e => e.Email.Equals(email));
        //}
    }
}
