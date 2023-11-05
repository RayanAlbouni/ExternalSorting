namespace SortingProject
{
    public interface IFileMerger
    {
        void MergeFiles(IList<string> filesToMerge, string outputFile);
    }
}