using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Transaction {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Payment { get; set; }
        public ICollection<Profile> ProfilesInTransaction { get; set; }
        public Profile PaidBy { get; set; }

        private void method() {
            //Enum.GetName(CurrencyType, CurrencyType.DOLLAR);
        }
    }
}
