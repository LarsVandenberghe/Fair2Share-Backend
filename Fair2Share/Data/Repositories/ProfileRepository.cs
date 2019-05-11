using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Repositories {
    public class ProfileRepository : IProfileRepository {
        private readonly ApplicationDbContext _dbContext;

        public ProfileRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public void Add(Profile profile) {
            _dbContext.Profiles.Add(profile);
        }

        public Profile GetBy(long id) {
            return Get().Include(pi => pi.ProfileImage).SingleOrDefault(p => p.ProfileId == id);
        }

        public Profile GetBy(string email) {
            return Get().Include(p => p.ProfileImage).SingleOrDefault(p => p.Email == email);
        }

        public void SaveChanges() {
            _dbContext.SaveChanges();
        }

        public void Update(Profile profile) {
            _dbContext.Profiles.Update(profile);
        }

        private IIncludableQueryable<Profile, Activity> Get() {
            return _dbContext.Profiles
            //Friends
            .Include(p => p.Friends).ThenInclude(v => v.Friend)
            .Include(p => p.Friends).ThenInclude(v => v.Profile)
            .Include(p => p.FriendOf).ThenInclude(v => v.Friend)
            .Include(p => p.FriendOf).ThenInclude(v => v.Profile)

            //FriendRequests
            .Include(p => p.ReceivedFriendRequests).ThenInclude(v => v.User)
            .Include(p => p.ReceivedFriendRequests).ThenInclude(v => v.FutureFriend)
            .Include(p => p.SentFriendRequests).ThenInclude(v => v.User)
            .Include(p => p.SentFriendRequests).ThenInclude(v => v.FutureFriend)

            //Activities
            .Include(p => p.Activities).ThenInclude(v => v.Profile)
            .Include(p => p.Activities).ThenInclude(v => v.Activity);//.ThenInclude(q => q.Transactions).ThenInclude(l => l.ProfilesInTransaction)
            //.Include(p => p.Activities).ThenInclude(v => v.Activity).ThenInclude(q => q.Participants);

        }
    }
}
