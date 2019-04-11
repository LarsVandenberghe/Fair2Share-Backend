using Fair2Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class FriendDTO {
        public long ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PathToImage { get; set; }
        public FriendRequestState? FriendRequestState { get; set; }

        //public string Email { get; set; }
    }
}
