using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class ProfileActivityIntersection {
        public long ProfileId { get; set; }
        public Profile Profile { get; set; }
        public long ActivityId { get; set; }
        public Activity Activity { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
    }
}
