using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Friends {
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }
        public int FriendId { get; set; }
        public virtual Profile Friend { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
