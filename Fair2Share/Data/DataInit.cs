using Fair2Share.Data.Repositories;
using Fair2Share.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Data {
    public class DataInit {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public DataInit(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager) {
            _dbContext = dbContext;
            _userManager = userManager;
        }
    

        public async Task InitializeData() {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated()) {
                await InitializeProfiles();
                _dbContext.SaveChanges();
            }
        }


         private async Task InitializeProfiles() {

            //lars
            string eMailAddress = "lars@hogent.be";
            IdentityUser user = new IdentityUser { UserName = eMailAddress, Email = eMailAddress };
            await _userManager.CreateAsync(user, "testPassword1");
            //await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "admin"));

            var profile = new Profile {
                Email = eMailAddress,
                Firstname = "Lars",
                Lastname = "Vandenberghe"
            };
            Activity a = new Activity { Name = "testActiviteit", Description = "", CurrencyType = CurrencyType.EURO };
 
            _dbContext.Profiles.Add(profile);
            profile.Activities.Add(new ProfileActivityIntersection { Activity = a, Profile = profile});

            //jef
            eMailAddress = "jef@Test.be";
            user = new IdentityUser { UserName = eMailAddress, Email = eMailAddress };
            await _userManager.CreateAsync(user, "someOtherPassword2");

            var profile2 = new Profile {
                Email = eMailAddress,
                Firstname = "Jef",
                Lastname = "Jefferson"
            };

            profile2.AddFriend(profile);
            _dbContext.Profiles.Add(profile2);

            a.Participants.Add(new ProfileActivityIntersection { Activity = a, Profile = profile2 });

            Transaction t = new Transaction { Name = "kayak", PaidBy = profile2, Payment = 10 };
            t.ProfilesInTransaction = new ProfileTransactionIntersection[] { new ProfileTransactionIntersection { Profile = profile, Transaction = t } };

            Transaction t2 = new Transaction { Name = "kayak2", PaidBy = profile2, Payment = 30 };
            t2.ProfilesInTransaction = new ProfileTransactionIntersection[] { new ProfileTransactionIntersection { Profile = profile, Transaction = t2 }, new ProfileTransactionIntersection { Profile = profile2, Transaction = t2 } };

            a.Transactions = new Transaction[] {}.ToList();
            a.Transactions.Add(t);
            a.Transactions.Add(t2);
            
            _dbContext.SaveChanges();
            //mei
            eMailAddress = "mei@mail.be";
            user = new IdentityUser { UserName = eMailAddress, Email = eMailAddress };
            await _userManager.CreateAsync(user, "someOtherPassword3");

            var profile3 = new Profile {
                Email = eMailAddress,
                Firstname = "Mei",
                Lastname = "Akasuki"
            };
            profile3.SendFriendRequest(profile);
            _dbContext.Profiles.Add(profile3);


            _dbContext.SaveChanges();
        }
    }
}
