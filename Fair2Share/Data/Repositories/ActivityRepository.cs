using Fair2Share.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data.Repositories {
    public class ActivityRepository : IActivityRepository {
        private readonly ApplicationDbContext _dbContext;

        public ActivityRepository(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }
        public void Add(Activity activity) {
            _dbContext.Activities.Add(activity);
        }

        public Activity GetBy(long id) {
            return _dbContext.Activities
                .Include(q => q.Transactions)/*.ThenInclude(l => l.Transaction)*/.ThenInclude( a => a.ProfilesInTransaction)
                .Include(q => q.Participants).ThenInclude(l => l.Profile)
                .Where(a => a.ActivityId == id).FirstOrDefault();
        }

        public void SaveChanges() {
            _dbContext.SaveChanges();
        }

        public void Update(Activity activity) {
            _dbContext.Activities.Update(activity);
        }
    }
}
