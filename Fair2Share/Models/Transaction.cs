using Fair2Share.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public class Transaction {
        public long TransactionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Payment { get; set; }
        public ICollection<ProfileTransactionIntersection> ProfilesInTransaction { get; set; }
        public Profile PaidBy { get; set; }

        public Transaction() {
            ProfilesInTransaction = new HashSet<ProfileTransactionIntersection>();
        }

        public Transaction(TransactionDTO transactionDTO, Profile paidBy) : this(){
            Name = transactionDTO.Name;
            Description = transactionDTO.Description;
            TimeStamp = DateTime.Now;
            Payment = transactionDTO.Payment;
            PaidBy = paidBy;
        }
    }
}
