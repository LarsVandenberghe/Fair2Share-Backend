using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
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
            return _dbContext.Profiles
                .Include(p => p.Friends).ThenInclude(v => v.Friend)
                .Include(p => p.Friends).ThenInclude(v => v.Profile)
                .Include(p => p.FriendOf).ThenInclude(v => v.Friend)
                .Include(p => p.FriendOf).ThenInclude(v => v.Profile)
                .SingleOrDefault(p => p.ProfileId == id);
        }

        public Profile GetBy(string email) {
            return _dbContext.Profiles
                .Include(p => p.Friends).ThenInclude(v => v.Friend)
                .Include(p => p.Friends).ThenInclude(v => v.Profile)
                .Include(p => p.FriendOf).ThenInclude(v => v.Friend)
                .Include(p => p.FriendOf).ThenInclude(v => v.Profile)
                .SingleOrDefault(p => p.Email == email);
        }

        public void SaveChanges() {
            _dbContext.SaveChanges();
        }
    }
}
