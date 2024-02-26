namespace EMarketWeb.Services.Interfaces;

public interface IImageService
{
    bool IsValid(IFormFile file);

    string GetFinalizedFileName(string fileName);

    bool Upload(IFormFile file, out string finalizedFileName);

    int RemoveExcept(IEnumerable<string> fileNames);
}
