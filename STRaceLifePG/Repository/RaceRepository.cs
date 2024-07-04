using Microsoft.EntityFrameworkCore;
using STRaceLifePG.Interface;
using STRaceLifePG.Models;
using StreetRaceLifeVK.Data;

namespace STRaceLifePG.Repository
{
    public class RaceRepository : IRaceRepository, IDASU<Race>
    {
        private readonly AppContextDb _repository;
        public RaceRepository(AppContextDb repository)
        {
            _repository = repository;
        }

        public bool Add(Race entity)
        {
            _repository.Add(entity);
            return Save();
        }

        public bool Delete(Race entity)
        {
            _repository.Remove(entity);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAllAsync()
       => await _repository.Races.ToListAsync();


        public async Task<Race> GetByIdAsync(Guid id)
        {
            var race = await _repository.Races
                .Include(r => r.User)
                .Include(r => r.Club)
                .FirstOrDefaultAsync(r => r.RaceId == id);

            return race ?? throw new Exception("Race not found");
        }



        public async Task<IEnumerable<Race>> GetByUserIdAsync(Guid userid)
       => await _repository.Races
        .Include(r => r.User)
        .Where(r => r.UserId ==userid)
        .ToListAsync();


        public bool Save() =>
        _repository.SaveChanges() > 0 ? true : false;


        public bool Update(Race entity)
        {
            _repository.Update(entity);
            return Save();
        }

        public async Task UpdateAsync(Race race)
        {
            _repository.Races.Update(race);
            await _repository.SaveChangesAsync();
        }


        public async Task DeleteAsync(Race race)
        { 
         _repository.Remove(race);  
            await _repository.SaveChangesAsync();
            
        }
    }
}
