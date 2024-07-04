using STRaceLifePG.Models;

namespace STRaceLifePG.Interface
{
    public interface IClubRepository
    {
       Task<IEnumerable<Club>> GetAllAsync();
        Task<Club> GetByIdAsync(Guid id);
        Task<IEnumerable<Club>> GetByUserIdAsync(User user);
        // Task<Club> GetByUserIdAsync(User user);
        Task UpdateAsync(Club club);
        Task DeleteAsync(Club club);
    }
}
