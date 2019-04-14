using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class TransactionDTO {
        public long? TransactionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? TimeStamp { get; set; }
        [Required]
        public decimal Payment { get; set; }
        public ICollection<FriendDTO> ProfilesInTransaction { get; set; }
        [Required]
        public FriendDTO PaidBy { get; set; }
    }
}
