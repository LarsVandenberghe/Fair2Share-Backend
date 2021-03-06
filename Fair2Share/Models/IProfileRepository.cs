﻿namespace Fair2Share.Models {
    public interface IProfileRepository {
        Profile GetBy(long id);
        Profile GetBy(string email);
        Profile GetProfileWithImage(long id);
        Profile GetProfileWithImage(string email);
        void Add(Profile profile);
        void Update(Profile profile);
        void SaveChanges();
    }
}
