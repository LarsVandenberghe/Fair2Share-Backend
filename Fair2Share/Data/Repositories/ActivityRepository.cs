﻿using Fair2Share.Models;
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

        public void Delete(Activity activity) {
            _dbContext.Activities.Remove(activity);
        }

        public Activity GetBy(long id) {
            return _dbContext.Activities
                .Include(q => q.Transactions)//.ThenInclude( a => a.ProfilesInTransaction)
                .Include(q => q.Transactions).ThenInclude(a => a.PaidBy)
                .Include(q => q.Participants).ThenInclude(l => l.Profile)
                .Where(a => a.ActivityId == id).FirstOrDefault();
        }

        public Transaction GetTransactionFromActivity(long id, long transactionId) {
            Activity acti = _dbContext.Activities.Include(q => q.Transactions).ThenInclude(a => a.ProfilesInTransaction)
                .Include(q => q.Transactions).ThenInclude(a => a.PaidBy)
                .Where(a => a.ActivityId == id).SingleOrDefault();
            if (acti == null) {
                throw new ArgumentException("activity is not valid");
            }

            return acti.Transactions.Where(t => t.TransactionId == transactionId).SingleOrDefault();
        }

        public void SaveChanges() {
            _dbContext.SaveChanges();
        }

        public void Update(Activity activity) {
            _dbContext.Activities.Update(activity);
        }
    }
}
