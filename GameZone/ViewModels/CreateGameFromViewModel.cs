namespace GameZone.ViewModels;

public class CreateGameFromViewModel : GameFormViewModel
{
    [AllowedExtensions(FileSettings.AllowedExtensions),
            MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    public IFormFile Cover { get; set; } = default!;
}

