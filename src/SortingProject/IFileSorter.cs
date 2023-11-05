namespace SortingProject
{
    public interface IFileSorter
    {
        Task<IList<string>> SortFilesAsync(SortingOptions options, IList<string> files);
    }
}