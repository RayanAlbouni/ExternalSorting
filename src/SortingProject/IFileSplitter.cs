namespace SortingProject
{
    public interface IFileSplitter
    {
        Task<IList<string>> SplitFileAsync(SortingOptions options);
    }
}