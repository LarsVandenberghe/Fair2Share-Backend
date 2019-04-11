using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class ProfileTransactionIntersection {
        public Transaction Transaction { get; set; }
        public long TransactionId { get; set; }
        public Profile Profile { get; set; }
        public long ProfileId { get; set; }
    }
}
