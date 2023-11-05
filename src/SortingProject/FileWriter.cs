namespace SortingProject
{
    public class FileWriter : IFileWriter
    {
        public async Task WriteBytesAsync(string filePath, byte[] data, int bufferSize)
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize, useAsync: true);
            await fs.WriteAsync(data, 0, data.Length);
        }
    }
}
