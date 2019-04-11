using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fair2Share.DTOs;
using Fair2Share.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fair2Share.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public class FriendRequestController : ControllerBase
    {
        private readonly IProfileRepository _profileRepository;

        public FriendRequestController(IProfileRepository profileRepository) {
            _profileRepository = profileRepository;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult<ICollection<FriendDTO>> GetAll() {
            return _profileRepository.GetBy(User.Identity.Name).ReceivedFriendRequests.Select(p => {
                return new FriendDTO {
                    ProfileId = p.User.ProfileId,
                    Firstname = p.User.Firstname,
                    Lastname = p.User.Lastname,
                    PathToImage = p.User.PathToImage,
                    FriendRequestState = p.State
                };
            }).ToList();
        }

        [HttpPost("email/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SendRequest(string email) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile futureFriend = _profileRepository.GetBy(email);
            if (futureFriend == null) {
                return BadRequest();
            } else {
                try {
                    profile.SendFriendRequest(futureFriend);
                    _profileRepository.SaveChanges();
                    return NoContent();
                } catch (ArgumentException e) {
                    return BadRequest(e.Message);
                }
                
            }
        }

        [HttpPost("id/{email}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult SendRequest(long id) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            Profile futureFriend = _profileRepository.GetBy(id);
            if (futureFriend == null) {
                return BadRequest();
            } else {
                try {
                    profile.SendFriendRequest(futureFriend);
                    _profileRepository.SaveChanges();
                    return NoContent();
                } catch (ArgumentException e) {
                    return BadRequest(e.Message);
                }

            }
        }

        [HttpPost("{id}/{accept}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult HandleRequest(long id, bool accept) {
            Profile profile = _profileRepository.GetBy(User.Identity.Name);
            FriendRequests friendRequest = profile.ReceivedFriendRequests.Where(p => p.UserId == id).SingleOrDefault();
            if (friendRequest == null) {
                return BadRequest("No friendRequests with that id.");
            } else {
                try {
                    profile.HandleFriendRequest(friendRequest, accept);
                    _profileRepository.SaveChanges();
                    return NoContent();
                } catch (ArgumentException e) {
                    return BadRequest(e.Message);
                }
            }
        }

    }
}