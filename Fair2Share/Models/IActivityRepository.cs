using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fair2Share.Models {
    public interface IActivityRepository{
        Activity GetBy(long id);
        void Add(Activity activity);
        void Update(Activity activity);
        void SaveChanges();
    }
}
