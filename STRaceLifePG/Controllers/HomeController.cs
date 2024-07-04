using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STRaceLifePG.Models;
using STRaceLifePG.ViewModel;
using StreetRaceLifeVK.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace STRaceLifePG.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppContextDb _appContextDb;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, AppContextDb appContextDb, UserManager<User> userManager)
        {
            _logger = logger;
            _appContextDb = appContextDb;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var club = await _appContextDb.Clubs.FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (club != null)
                {
                    ViewBag.HasClub = true;
                    ViewBag.ClubId = club.ClubId;
                }
                else
                {
                    ViewBag.HasClub = false;
                }
            }
            else
            {
                ViewBag.HasClub = false;
            }

            var clubs = await _appContextDb.Clubs
                .Include(c => c.User)
                .Include(c => c.Races)
                .ToListAsync();

            var clubViewModels = clubs
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ClubViewModel
                {
                    Club = c,
                    LikeCount = _appContextDb.ClubLikes.Count(cl => cl.ClubId == c.ClubId)
                }).ToList();

            var viewModel = new ClubListViewModel
            {
                Clubs = clubViewModels,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(clubs.Count / (double)pageSize)
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
