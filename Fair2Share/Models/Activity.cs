using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Activity {
        public long ActivityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public ICollection<ProfileActivityIntersection> Participants { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

        public Activity() {
            Participants = new HashSet<ProfileActivityIntersection>();
            Transactions = new HashSet<Transaction>();
        }
    }
}
