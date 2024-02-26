using EMarketWeb.Services.Interfaces;

namespace EMarketWeb.Services;

public class WebRootImageService : IImageService
{
    private readonly string[] _validFileExts = ["jpg", "jpeg", "png"];
    private readonly string[] CONST_EXCLUSIONS = ["no-image.jpg"];
    private const string WEBROOT_FOLDER = "images";
    private IWebHostEnvironment _webHostEnv;
    public string WebRootDirectory => Path.Combine(_webHostEnv.WebRootPath, WEBROOT_FOLDER);

    public WebRootImageService(IWebHostEnvironment webHostEnv)
    {
        _webHostEnv = webHostEnv;
    }

    public string GetFinalizedFileName(string fileName)
    {
        // check if the filename is already used
        int counter = 1;
        string cleanFileName = Path.GetFileNameWithoutExtension(fileName);
        string finalizedFileName = cleanFileName;
        while (IsExisting(finalizedFileName, WebRootDirectory))
        {
            counter++;
            finalizedFileName = $"{cleanFileName}({counter})";
        }

        finalizedFileName += Path.GetExtension(fileName);

        return finalizedFileName;
    }

    private bool IsExisting(string fileName, string directory)
    {
        var existingFiles = Directory.GetFiles(directory).Select(Path.GetFileNameWithoutExtension);
        return existingFiles.Contains(Path.GetFileNameWithoutExtension(fileName));
    }

    public bool IsValid(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            return false;
        }

        if (!file.ContentType.StartsWith("image"))
        {
            return false;
        }

        if (!_validFileExts.Any(file.ContentType.EndsWith))
        {
            return false;
        }

        return true;
    }

    public bool Upload(IFormFile file, out string finalizedFileName)
    {
        finalizedFileName = string.Empty;
        if (!IsValid(file))
        {
            return false;
        }

        finalizedFileName = GetFinalizedFileName(file.FileName);

        using (var stream = new FileStream(Path.Combine(WebRootDirectory, finalizedFileName), FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return true;
    }

    public int RemoveExcept(IEnumerable<string> fileNames)
    {
        int rowsAffected = 0;
        IEnumerable<string> allFileNames = Directory.GetFiles(WebRootDirectory)
            .Select(Path.GetFileName)
            .Except(CONST_EXCLUSIONS)
            .OfType<string>();

        IEnumerable<string> exceptFileNames = fileNames.Select(Path.GetFileName).OfType<string>();

        IEnumerable<string> toRemove = allFileNames.Except(exceptFileNames);

        foreach (var forRemovalFileName in toRemove)
        {
            string fullFilePath = Path.Combine(WebRootDirectory, forRemovalFileName);
            try
            {
                File.Delete(fullFilePath);
                rowsAffected++;
            }
            catch { }
        }

        return rowsAffected;
    }
}
