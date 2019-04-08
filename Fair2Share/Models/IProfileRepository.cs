namespace Fair2Share.Models {
    public interface IProfileRepository {
        Profile GetBy(int id);
        Profile GetBy(string email);
        void Add(Profile profile);
        void SaveChanges();
    }
}
