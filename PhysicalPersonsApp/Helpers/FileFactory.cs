using BAL.Infrastructure;

namespace PhysicalPersonsApp.Helpers;

public interface IFileFactory
{
    IFile CreateFile(IFormFile formFile);
}


public class FileFactory : IFileFactory
{
    public IFile CreateFile(IFormFile formFile)
    {
        return new FormFileAdapter(formFile);
    }
}