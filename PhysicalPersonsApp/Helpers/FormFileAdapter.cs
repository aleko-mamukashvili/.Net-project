using BAL.Infrastructure;

namespace PhysicalPersonsApp.Helpers;

public class FormFileAdapter(IFormFile? formFile) : IFile
{
    private readonly IFormFile? _formFile = formFile ?? throw new ArgumentNullException(nameof(formFile));
    public string FileName => _formFile!.FileName;
    public Stream Content => _formFile!.OpenReadStream();
}
