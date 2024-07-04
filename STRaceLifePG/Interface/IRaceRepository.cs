using STRaceLifePG.Models;

namespace STRaceLifePG.Interface
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllAsync();
        Task<Race> GetByIdAsync(Guid id);
        Task<IEnumerable<Race>> GetByUserIdAsync(Guid userid);
        Task UpdateAsync(Race race);
        Task DeleteAsync(Race race);
    }
}
