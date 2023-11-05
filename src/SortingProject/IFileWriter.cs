namespace SortingProject
{
    public interface IFileWriter
    {
        Task WriteBytesAsync(string filePath, byte[] data, int bufferSize);
    }
}