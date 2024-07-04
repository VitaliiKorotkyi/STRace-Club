using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using STRaceLifePG.Controllers;
using STRaceLifePG.Interface;
using STRaceLifePG.Models;
using StreetRaceLifeVK.Data;

namespace STRaceLifePG.Repository
{
    public class ClubRapository : IClubRepository, IDASU<Club>
    {
        private readonly AppContextDb _appContextDb;
        private readonly ILogger<ClubRapository> _logger;
        public ClubRapository(AppContextDb appContextDb,ILogger<ClubRapository> logger)
        {
            _appContextDb = appContextDb;
            _logger = logger;
        }

        public bool Add(Club entity)
        {
            _appContextDb.Add(entity);
            return Save();
        }

        public bool Delete(Club entity)
        {
            _appContextDb.Remove(entity);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAllAsync()
        => await _appContextDb.Clubs.ToListAsync();



        public async Task<Club> GetByIdAsync(Guid id)
        {
            var club = await _appContextDb.Clubs
        .Include(c => c.User)
        .Include(c => c.Races)
        .FirstOrDefaultAsync(c => c.ClubId == id);

            if (club is null)
            {
                _logger.LogError($"Club with ID {id} not found in the database.");
                throw new Exception("Club not found");
            }

            club.LikeCount = await _appContextDb.ClubLikes.CountAsync(cl => cl.ClubId == id); // Устанавливаем количество лайков

            return club;
        }


        public async Task<IEnumerable<Club>> GetByUserIdAsync(User user) =>
        await _appContextDb.Clubs.Include(u => u.User).Where(i => i.ClubId == user.Id).ToListAsync();

        //public async Task<Club> GetByUserIdAsync(User user)
        //{
        //    return await _appContextDb.Clubs
        //        .Include(c => c.User)
        //        .FirstOrDefaultAsync(c => c.UserId == user.Id);
        //}
        public bool Save()=>
        _appContextDb.SaveChanges()>0?true:false;

        

        public bool Update(Club entity)
        {
           _appContextDb.Clubs.Update(entity);
            return Save();
        }
        public async Task UpdateAsync(Club club)
        {
            _appContextDb.Clubs.Update(club);
            await _appContextDb.SaveChangesAsync();
        }

        public async Task DeleteAsync(Club club)
        { 
        _appContextDb.Remove(club);
          await  _appContextDb.SaveChangesAsync();   
        }
    }
}
