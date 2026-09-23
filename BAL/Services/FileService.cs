using BAL.Infrastructure;

namespace BAL.Services;

public interface IFileService
{

    Task<string> SaveFileAsync(IFile file, string[] allowedFileExtensions);
    Task<string> UpdateFileAsync(string oldFileName, IFile file, string[] allowedFileExtensions);
    void DeleteFile(string fileName);

}
public class FileService(string contentRootPath) : IFileService
{
    public async Task<string> SaveFileAsync(IFile file, string[] allowedFileExtensions)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        var contentPath = contentRootPath;
        var uploadsPath = Path.Combine(contentPath, "Media/Person");

        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        var extensions = Path.GetExtension(file.FileName);
        if(!allowedFileExtensions.Contains(extensions))
        {
            throw new ArgumentException("Invalid file extension.");
        }

        var fileName = Guid.NewGuid() + extensions;
        var filePath = Path.Combine(uploadsPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.Content.CopyToAsync(stream);

        return fileName;
    }

    public async Task<string> UpdateFileAsync(string oldFileName, IFile file, string[] allowedFileExtensions)
    {
        if (file == null)
        {
            throw new ArgumentNullException(nameof(file));
        }

        DeleteFile(oldFileName);

        return await SaveFileAsync(file, allowedFileExtensions);
    }

    public void DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        var contentPath = contentRootPath;
        var filePath = Path.Combine(contentPath, "Media/Person", fileName);

        if (!File.Exists(filePath))
        {
           throw new FileNotFoundException("File not found.");
        }
        File.Delete(filePath);
    }
}
