using Fair2Share.DTOs;
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

        public static string[] IMGEXTENSIONS = new string[]{"jpg", "gif", "jpeg", "png" };

        public Profile() {
            Friends = new HashSet<Friends>();
            FriendOf = new HashSet<Friends>();
            Activities = new HashSet<ProfileActivityIntersection>();
            ReceivedFriendRequests = new HashSet<FriendRequests>();
            SentFriendRequests = new HashSet<FriendRequests>();
            Transactions = new HashSet<ProfileTransactionIntersection>();
        }

        public void AddFriend(Profile friend) {
            if (friend == null) {
                throw new ArgumentException("Argument friend is null.");
            }
            //has friend not in friendlist
            if (Friends.Where(p => p.FriendId == friend.ProfileId).SingleOrDefault() == null) {
                DateTime timeStamp = DateTime.Now;
                Friends friends1 = new Friends { Profile = this, Friend = friend, TimeStamp = timeStamp };
                Friends friends2 = new Friends { Profile = friend, Friend = this, TimeStamp = timeStamp };
                Friends.Add(friends1);
                FriendOf.Add(friends2);
            } else {
                throw new ArgumentException("You already have this person as a friend.");
            }
        }

        public void SendFriendRequest(Profile futureFriend) {
            if (futureFriend == null) {
                throw new ArgumentException("Argument futureFriend is null.");
            }

            if (Friends.SingleOrDefault(f => f.FriendId == futureFriend.ProfileId) != null) {
                throw new ArgumentException("You can't send a friend request to someone you are already friends with.");
            }

            if (futureFriend.ProfileId == this.ProfileId) {
                throw new ArgumentException("You can't send yourself a friend request.");
            }

            if (SentFriendRequests.Where(p => p.FutureFriendId == futureFriend.ProfileId).SingleOrDefault() == null) {
                if (ReceivedFriendRequests.Where(p => p.FutureFriendId == this.ProfileId).SingleOrDefault() == null) {
                    DateTime timeStamp = DateTime.Now;
                    FriendRequests friendRequest = new FriendRequests { FutureFriend = futureFriend, User = this, TimeStamp = timeStamp };
                    SentFriendRequests.Add(friendRequest);
                    //ReceivedFriendRequests
                } else {
                    throw new ArgumentException("You already have a friendrequest form this person.");
                }
            } else {
                throw new ArgumentException("You already sent this person a friend request.");
            }
        }

        public void HandleFriendRequest(FriendRequests request, bool accept) {
            if (request == null) {
                throw new ArgumentException("Argument friend requests is null.");
            }
            if (accept) {
                if (ReceivedFriendRequests.Where(p => p.UserId == request.UserId).SingleOrDefault() != null) {
                    ReceivedFriendRequests.Remove(ReceivedFriendRequests.Where(p => p.UserId == request.UserId).SingleOrDefault());
                    AddFriend(request.User);
                } else {
                    throw new ArgumentException("Friendrequest is not valid.");
                }
            } else {
                ReceivedFriendRequests.Where(p => p.UserId == request.UserId).SingleOrDefault().State = FriendRequestState.IGNORE;
            }
            
        }

        public void Update(SimpleProfileDTO profileDTO) {
            if (profileDTO.ProfileId != ProfileId) {
                throw new ArgumentException("ProfileDTO is not valid.");
            }
            if (!(profileDTO.PathToImage == null || profileDTO.PathToImage == "")) {
                string extension = profileDTO.PathToImage.Split(".").LastOrDefault();
                if (!Profile.IMGEXTENSIONS.Contains(extension.ToLower())) {
                    throw new ArgumentException("PathToImage is not valid.");
                }
            }

            this.Firstname = profileDTO.Firstname;
            this.Lastname = profileDTO.Lastname;
            this.PathToImage = profileDTO.PathToImage;
        }
    }
}
