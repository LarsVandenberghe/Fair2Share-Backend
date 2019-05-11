using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Fair2Share.Models;

namespace Fair2Share.DTOs {
    public class SimpleProfileDTO {

        public SimpleProfileDTO(Profile profile) {
            ProfileId = profile.ProfileId;
            Firstname = profile.Firstname;
            Lastname = profile.Lastname;
            //PathToImage = profile.PathToImage;
        }

        public SimpleProfileDTO() { }

        [Required]
        public long ProfileId { get; set; }
        [Required]
        [StringLength(200)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(250)]
        public string Lastname { get; set; }

        [DataType(DataType.Url)]
        public string PathToImage { get; set; }
    }
}
