using Fair2Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class InitActivityDTO {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public CurrencyType CurrencyType { get; set; }
    }
}
