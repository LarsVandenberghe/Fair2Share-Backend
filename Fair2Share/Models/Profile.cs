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
        
        public virtual ICollection<Friends> Friends { get; set; }
        public virtual ICollection<Friends> FriendOf { get; set; }
        public virtual ICollection<ProfileActivityIntersection> Activities { get; set; }
        public virtual ICollection<FriendRequests> ReceivedFriendRequests { get; set; }
        public virtual ICollection<FriendRequests> SentFriendRequests { get; set; }
        public virtual ICollection<ProfileTransactionIntersection> Transactions { get; set; }

        public Profile() {
            Friends = new HashSet<Friends>();
            FriendOf = new HashSet<Friends>();
            Activities = new HashSet<ProfileActivityIntersection>();
            ReceivedFriendRequests = new HashSet<FriendRequests>();
            SentFriendRequests = new HashSet<FriendRequests>();
            Transactions = new HashSet<ProfileTransactionIntersection>();
        }

        public void AddFriend(Profile friend) {
            //has friend not in friendlist
            if (friend == null) {
                throw new ArgumentException("Argument friend is null.");
            }
            if (Friends.Where(p => p.ProfileId == friend.ProfileId).SingleOrDefault() == null) {
                DateTime timeStamp = DateTime.Now;
                Friends friends1 = new Friends { ProfileId = this.ProfileId, Profile = this, FriendId = friend.ProfileId, Friend = friend, TimeStamp = timeStamp };
                Friends friends2 = new Friends { ProfileId = friend.ProfileId, Profile = friend, FriendId = this.ProfileId, Friend = this, TimeStamp = timeStamp };
                Friends.Add(friends1);
                FriendOf.Add(friends2);
            } else {
                throw new ArgumentException();
            }
        }

        public void SendFriendRequest(Profile futureFriend) {
            if (SentFriendRequests.Where(p => p.FutureFriendId == futureFriend.ProfileId).SingleOrDefault() == null) {
                if (ReceivedFriendRequests.Where(p => p.FutureFriendId == this.ProfileId).SingleOrDefault() == null) {
                    DateTime timeStamp = DateTime.Now;
                    FriendRequests friendRequest1 = new FriendRequests {FutureFriend = futureFriend, FutureFriendId = futureFriend.ProfileId, User = this, UserId = this.ProfileId, TimeStamp = timeStamp };
                    //FriendRequests friendRequest2 = new FriendRequests { FutureFriend = this, FutureFriendId = this.ProfileId, User = futureFriend, UserId = futureFriend.ProfileId, TimeStamp = timeStamp};
                    SentFriendRequests.Append(friendRequest1);
                    //ReceivedFriendRequests
                }
            }
        }
    }
}
