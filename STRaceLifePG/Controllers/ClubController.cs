using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using STRaceLifePG.Interface;
using STRaceLifePG.Models;
using STRaceLifePG.ViewModel;
using StreetRaceLifeVK.Data;

namespace STRaceLifePG.Controllers
{
    public class ClubController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppContextDb _appContextDb;
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoSerice _photoSerice;
        private readonly ILogger<ClubController> _logger;

        public ClubController(AppContextDb appContext, SignInManager<User> signInManager, UserManager<User> userManager, IClubRepository clubRepository, IPhotoSerice photoSerice, ILogger<ClubController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appContextDb = appContext;
            _clubRepository = clubRepository;
            _photoSerice = photoSerice;
            _logger = logger;
        }

        public IActionResult CreateNewClub()
        {
            return View(new Club());
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewClub(Club club)
        {
            if (ModelState.IsValid)
            {
                var photoresult = await _photoSerice.AddPhotoAsync(club.Image);
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var model = new Club()
                {
                    ClubId = Guid.NewGuid(),
                    Title = club.Title,
                    Description = club.Description,
                    ImageUrl = photoresult.Url.ToString(),
                    UserId = user.Id
                };
                _appContextDb.Clubs.Add(model);
                await _appContextDb.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(club);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var club = await _clubRepository.GetByIdAsync(id);

            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        [HttpPost]
        public async Task<IActionResult> Like(Guid clubId)
        {
            var userId = Guid.Parse(_userManager.GetUserId(User));
            var existingLike = await _appContextDb.ClubLikes.FirstOrDefaultAsync(l => l.ClubId == clubId && l.UserId == userId);

            if (existingLike == null)
            {
                var like = new ClubLike
                {
                    Id = Guid.NewGuid(),
                    ClubId = clubId,
                    UserId = userId
                };

                _appContextDb.ClubLikes.Add(like);
                await _appContextDb.SaveChangesAsync();
            }
            else
            {
                _appContextDb.ClubLikes.Remove(existingLike);
                await _appContextDb.SaveChangesAsync();
            }

            var likeCount = await _appContextDb.ClubLikes.CountAsync(cl => cl.ClubId == clubId);

            return Json(new { likeCount });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club is null)
            {
                return View("Error not found");
            }
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                Id = club.ClubId
            };
            return View(clubVM);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditClubViewModel clubmodel)
        {
            if (ModelState.IsValid)
            {
                var club = await _clubRepository.GetByIdAsync(clubmodel.Id);
                if (clubmodel is null)
                {

                    return View("Error not found");
                }
                club.Title = clubmodel.Title;
                club.Description = clubmodel.Description;
                if (clubmodel.Image is not null)
                {
                    var photoresult = await _photoSerice.AddPhotoAsync(clubmodel.Image);
                    if (photoresult.Error is not null)
                    {
                        ModelState.AddModelError("Image", "Image upload failed");
                        return View(clubmodel);
                    }
                    club.ImageUrl = photoresult.SecureUri.ToString();
                }
                await _clubRepository.UpdateAsync(club);
                return RedirectToAction("Detail", new { id = clubmodel.Id });

            }
            return View(clubmodel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club is  null)
            {
                _logger.LogError($"Club with ID {id} not found.");
                return View("NotFound");
            }
            return View(club);
        }
        [HttpPost, ActionName("DeleteClub")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _logger.LogInformation($"Attempting to delete club with ID {id}.");
            var clubdetails = await _clubRepository.GetByIdAsync(id);
            if (clubdetails is null)
            {
                _logger.LogError($"Club with ID {id} not found.");
                return View("NotFound");
            }
            await _clubRepository.DeleteAsync(clubdetails);
            _logger.LogInformation($"Successfully deleted club with ID {id}.");
            return RedirectToAction("Index");
        }
    }
}
