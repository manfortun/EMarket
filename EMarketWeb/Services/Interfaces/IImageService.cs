namespace EMarketWeb.Services.Interfaces;

public interface IImageService
{
    /// <summary>
    /// Checks if the file is valid image file.
    /// </summary>
    /// <param name="file">File</param>
    /// <returns><c>true</c> if the file is valid image file. Otherwise, <c>false</c></returns>
    bool IsValid(IFormFile file);

    /// <summary>
    /// Obtains the final name of a filename.
    /// <para>When there are other files with the same name as <paramref name="fileName"/>,
    /// a counter is added on the suffix to make it unique.</para>
    /// Example:
    /// <code>myImage.jpg -> myImage(2).jpg</code>
    /// </summary>
    /// <param name="fileName">Original filename</param>
    /// <returns>Finalized name.</returns>
    string GetFinalizedFileName(string fileName);

    /// <summary>
    /// Sends <paramref name="file"/> to root destination.
    /// </summary>
    /// <param name="file">The file to upload</param>
    /// <param name="finalizedFileName">The finalized filename</param>
    /// <returns></returns>
    bool Upload(IFormFile file, out string finalizedFileName);

    /// <summary>
    /// Remove images from the root directory except for enumerable of filenames
    /// </summary>
    /// <param name="fileNames">The filenames to retain.</param>
    /// <returns>Number of items removed</returns>
    int RemoveExcept(IEnumerable<string> fileNames);
}
