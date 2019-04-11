using Fair2Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.DTOs {
    public class SimpleProfileDTO {
        public long ProfileId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PathToImage { get; set; }
        public virtual ICollection<FriendDTO> Friends { get; set; }

        public SimpleProfileDTO() {

        }

        public SimpleProfileDTO(Profile profile) {
            ProfileId = profile.ProfileId;
            Firstname = profile.Firstname;
            Lastname = profile.Lastname;
            Email = profile.Email;
            PathToImage = profile.PathToImage;
            Friends = profile.Friends.ToList().Select(p => {
                Profile profile_temp = p.Friend;
                return new FriendDTO {
                    ProfileId = profile_temp.ProfileId,
                    Firstname = profile_temp.Firstname,
                    Lastname = profile_temp.Lastname,
                    PathToImage = profile_temp.PathToImage
                };
            }).ToList();
        }
    }
}
