using EMarketWeb.Services.Interfaces;

namespace EMarketWeb.Services;

public class WebRootImageService : IImageService
{
    private readonly string[] _validFileExts = ["jpg", "jpeg", "png"];
    private readonly string[] CONST_EXCLUSIONS = ["no-image.jpg"];
    private const string WEBROOT_FOLDER = "images";
    private IWebHostEnvironment _webHostEnv;

    /// <summary>
    /// Path of root directory
    /// </summary>
    public string RootDirectory => Path.Combine(_webHostEnv.WebRootPath, WEBROOT_FOLDER);

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
        while (IsExisting(finalizedFileName, RootDirectory))
        {
            counter++;
            finalizedFileName = $"{cleanFileName}({counter})";
        }

        finalizedFileName += Path.GetExtension(fileName);

        return finalizedFileName;
    }

    /// <summary>
    /// Checks if there is already an existing file in the <paramref name="directory"/> with the same file name as <paramref name="fileName"/>
    /// </summary>
    /// <param name="fileName">Filename to check</param>
    /// <param name="directory">Directory to check</param>
    /// <returns><c>true</c> if there is already an existing file name. Otherwise, <c>false</c>.</returns>
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

        using (var stream = new FileStream(Path.Combine(RootDirectory, finalizedFileName), FileMode.Create))
        {
            file.CopyTo(stream);
        }

        return true;
    }

    public int RemoveExcept(IEnumerable<string> fileNames)
    {
        int rowsAffected = 0;
        IEnumerable<string> allFileNames = Directory.GetFiles(RootDirectory)
            .Select(Path.GetFileName)
            .Except(CONST_EXCLUSIONS)
            .OfType<string>();

        IEnumerable<string> exceptFileNames = fileNames.Select(Path.GetFileName).OfType<string>();

        IEnumerable<string> toRemove = allFileNames.Except(exceptFileNames);

        foreach (var forRemovalFileName in toRemove)
        {
            string fullFilePath = Path.Combine(RootDirectory, forRemovalFileName);
            try
            {
                File.Delete(fullFilePath);
                rowsAffected++;
            }
            catch
            {
                // FALLTHROUGH
            }
        }

        return rowsAffected;
    }
}
