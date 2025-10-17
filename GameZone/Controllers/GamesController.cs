

namespace GameZone.Controllers;

public class GamesController : Controller
{

    private readonly IDevicesService _devicesService;
    private readonly ICategoriesService _categoriesService;
    private readonly IGameService _gamesService;

    public GamesController(ICategoriesService categoriesService,
        IGameService gamesService,
        IDevicesService devicesService)
    {

        _categoriesService = categoriesService;
        _gamesService = gamesService;
        _devicesService = devicesService;
    }

    public IActionResult Index()
    {
        var games = _gamesService.GetAll();
        return View(games);
    }
    public IActionResult Details(int id)
    {
        var game = _gamesService.GetById(id);
        if (game is null)
            return NotFound();
        return View(game);
    }

    [HttpGet]
    public IActionResult Create()
    {

        CreateGameFromViewModel viewModel = new()
        {
            Categories = _categoriesService.GetSelectLists(),
            Devices = _devicesService.GetSelectLists(),
        };
        return View(viewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken] //safe from attack
    public async Task<IActionResult> Create(CreateGameFromViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categories = _categoriesService.GetSelectLists();

            model.Devices = _devicesService.GetSelectLists();
            return View(model);
        }

        await _gamesService.create(model);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id) 
    {
		var game = _gamesService.GetById(id);
		if (game is null)
			return NotFound();

        EditGameFormViewModel viewModel = new()
        {
            Id= id,
            Name= game.Name,
            Description= game.Description,
            CategoryId= game.CategoryId,
            SelectedDevices= game.Devices.Select(d => d.DeviceId).ToList(),
            Categories= _categoriesService.GetSelectLists(),
            Devices= _devicesService.GetSelectLists(),
            CurrentCover= game.Cover,
        };
        return View(viewModel);
	}

	[HttpPost]
	[ValidateAntiForgeryToken] //safe from attack
	public async Task<IActionResult> Edit(EditGameFormViewModel model)
	{
		if (!ModelState.IsValid)
		{
			model.Categories = _categoriesService.GetSelectLists();

			model.Devices = _devicesService.GetSelectLists();
			return View(model);
		}

		var game =await _gamesService.Update(model);
        if (game is null)
            return BadRequest();
		return RedirectToAction(nameof(Index));
	}

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        var isDeleted = _gamesService.Delete(id);  
        return isDeleted? Ok() : BadRequest();
    }
}

