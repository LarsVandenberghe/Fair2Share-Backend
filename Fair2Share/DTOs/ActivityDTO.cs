﻿using Fair2Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class ActivityDTO {
        [Required]
        public long ActivityId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public CurrencyType CurrencyType { get; set; }
        public ICollection<FriendDTO> Participants { get; set; }
        public ICollection<TransactionDTO> Transactions { get; set; }

        public ActivityDTO() {

        }

        public ActivityDTO(Activity activity) {
            ActivityId = activity.ActivityId;
            Name = activity.Name;
            Description = activity.Description;
            CurrencyType = activity.CurrencyType;
            Participants = activity.Participants.Select(p => new FriendDTO(p.Profile)).ToList();
        }
    }
}
