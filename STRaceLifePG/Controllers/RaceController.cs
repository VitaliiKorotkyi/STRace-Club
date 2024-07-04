using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using STRaceLifePG.Interface;
using STRaceLifePG.Models;
using STRaceLifePG.ViewModel;
using StreetRaceLifeVK.Data;

namespace STRaceLifePG.Controllers
{
    public class RaceController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppContextDb _appContextDb;
        private readonly IRaceRepository _raceRepository;
        private readonly IPhotoSerice _photoSerice;
        private readonly ILogger<RaceController> _logger;

        public RaceController(UserManager<User> userManager, SignInManager<User> signInManager, AppContextDb appContextDb, IRaceRepository raceRepository, IPhotoSerice photoSerice, ILogger<RaceController> logger)
        {
            _appContextDb = appContextDb;
            _userManager = userManager;
            _signInManager = signInManager;
            _raceRepository = raceRepository;
            _photoSerice = photoSerice;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            var races = await _raceRepository.GetAllAsync();
            var sortedRaces = races.OrderBy(r => r.StartDate < DateTime.Now && (DateTime.Now - r.StartDate).TotalHours >= 5)
                                   .ThenBy(r => r.StartDate)
                                   .ToList();

            var raceViewModels = sortedRaces
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RaceViewModel
                {
                    Race = r
                }).ToList();

            var viewModel = new RaceListViewModel
            {
                Races = raceViewModels,
                PageNumber = page,
                TotalPages = (int)Math.Ceiling(sortedRaces.Count / (double)pageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Detail(Guid id)
        {
            var race = await _raceRepository.GetByIdAsync(id);
            if (race == null)
            {
                return NotFound();
            }
            return View(race);
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid clubId)
        {
            var club = await _appContextDb.Clubs.FindAsync(clubId);
            var userId = Guid.Parse(_userManager.GetUserId(User));

            if (club == null || club.UserId != userId)
            {
                return Forbid();
            }

            var race = new Race { ClubId = clubId };
            return View(race);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Race race)
        {
            if (ModelState.IsValid)
            {
                race.UserId = Guid.Parse(_userManager.GetUserId(User)); // Set the user ID

                var club = await _appContextDb.Clubs.FindAsync(race.ClubId);
                if (club == null || club.UserId != race.UserId)
                {
                    return Forbid();
                }

                // Check if the image is provided
                if (race.Image == null || race.Image.Length == 0)
                {
                    ModelState.AddModelError("Image", "Please select an image to upload.");
                    return View(race);
                }

                // Try uploading the photo
                var photoResult = await _photoSerice.AddPhotoAsync(race.Image);
                if (photoResult.Error != null)
                {
                    // Log the error
                    _logger.LogError($"Image upload failed: {photoResult.Error.Message}");
                    ModelState.AddModelError("Image", "Image upload failed");
                    return View(race);
                }

                // Set the image URL after successful upload
                race.ImageUrl = photoResult.SecureUrl.ToString();

                // Save the race details to the database
                _appContextDb.Races.Add(race);
                await _appContextDb.SaveChangesAsync();

                return RedirectToAction("Detail", "Club", new { id = race.ClubId });
            }
            return View(race);


        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid raceId)
        {
            var race = await _raceRepository.GetByIdAsync(raceId);
            if (race == null)
            {
                return View("NotFound");
            }
            var raceVM = new RaceEditViewModel
            {
                Description = race.Description,
                Capital = race.Capital,
                City = race.City,
                Street = race.Street,
                Category = race.Category,
                Id = race.RaceId,
                StartDate = race.StartDate
            };
            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RaceEditViewModel racemodel)
        {
            if (ModelState.IsValid)
            {
                var race = await _raceRepository.GetByIdAsync(racemodel.Id);
                if (race == null)
                {
                    return View("NotFound");
                }
                race.Description = racemodel.Description;
                race.Capital = racemodel.Capital;
                race.City = racemodel.City;
                race.Street = racemodel.Street;
                race.Category = racemodel.Category;
                race.StartDate = racemodel.StartDate;
                if (racemodel.Image != null)
                {
                    var photoResult = await _photoSerice.AddPhotoAsync(racemodel.Image);
                    if (photoResult.Error != null)
                    {
                        ModelState.AddModelError("Image", "Image upload failed");
                        return View(racemodel);
                    }
                    race.ImageUrl = photoResult.SecureUri.ToString();
                }
                await _raceRepository.UpdateAsync(race);
                return RedirectToAction("Detail", new { id = racemodel.Id });
            }
            return View(racemodel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            
                var race = await _raceRepository.GetByIdAsync(id);
                if (race is null)
                {
                    return View("NotFound");
                }
            
            return View(race);
        }
        [HttpPost, ActionName("DeleteRace")]
        public async Task<IActionResult> DeleteRace(Guid id)
        { 
        var racedetails=await _raceRepository.GetByIdAsync(id);
            if (racedetails is null)
            {
               return View("NotFound");
            }
            await _raceRepository.DeleteAsync(racedetails);
           return RedirectToAction("Index");
        }
    }
}
