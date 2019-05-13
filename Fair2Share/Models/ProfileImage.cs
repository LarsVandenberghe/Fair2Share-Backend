using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class ProfileImage {
        public long ProfileId { get; private set; }
        public byte[] Image { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public Profile Profile { get; set; }
    }
}
