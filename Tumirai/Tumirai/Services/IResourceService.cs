namespace Tumirai.Services;

public interface IResourceService
{
    Stream GetResourceStream(string name);

    long GetResourceSize(string resourceName);

    IEnumerable<string> GetResourceNamesFromFolder(string v);
}
