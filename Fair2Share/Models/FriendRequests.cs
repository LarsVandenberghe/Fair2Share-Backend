using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class FriendRequests {
        public long UserId { get; set; }
        public long FutureFriendId { get; set; }
        public DateTime TimeStamp { get; set; }
        //public int State { get; set; }
    }
}
