using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class FriendRequests {
        public Profile User { get; set; }
        public long UserId { get; set; }
        public Profile FutureFriend { get; set; }
        public long FutureFriendId { get; set; }
        public DateTime TimeStamp { get; set; }
        public FriendRequestState State { get; set; }

        public FriendRequests() {
            State = FriendRequestState.NEW;
        }
    }
}
