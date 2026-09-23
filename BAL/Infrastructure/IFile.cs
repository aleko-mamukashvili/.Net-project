namespace BAL.Infrastructure;

public interface IFile
{
    public string FileName { get; }
    public Stream Content { get; }
}