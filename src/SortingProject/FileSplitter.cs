using System.IO.MemoryMappedFiles;

namespace SortingProject
{
    public class FileSplitter : IFileSplitter
    {
        private readonly IFileWriter _fileWriter;

        public FileSplitter(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        public async Task<IList<string>> SplitFileAsync(SortingOptions options)
        {
            if (string.IsNullOrEmpty(options.InputFilePath)) throw new ArgumentNullException(options.InputFilePath);
            if (string.IsNullOrEmpty(options.OutputFileName)) throw new ArgumentNullException(options.OutputFileName);
            if (string.IsNullOrEmpty(options.OutputDirectory)) throw new ArgumentNullException(options.OutputDirectory);
            if (options.ChunkSizeInMB <= 0) throw new ArgumentException("Chunk size must be a positive value.");
            var chunkSizeInBytes = options.ChunkSizeInMB * 1024 * 1024; // Convert Megabytes to bytes.
            using var sourceStream = File.OpenRead(options.InputFilePath);
            return await SplitFileInternal(options.OutputDirectory, chunkSizeInBytes, sourceStream);
        }

        private async Task<IList<string>> SplitFileInternal(string outputDirectory, int chunkSizeInBytes, FileStream sourceStream)
        {
            IList<string> result = new List<string>();
            using (var mmf = MemoryMappedFile.CreateFromFile(sourceStream, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, false))
            {
                var fileSize = sourceStream.Length;
                var offset = 0L;
                var lastPosition = 0L;
                var sequence = 0;
                while (offset < fileSize)
                {
                    var remainingSize = fileSize - offset;
                    var chunkSize = Math.Min(chunkSizeInBytes, remainingSize + 1);
                    lastPosition = offset + chunkSize;
                    using var mmView = mmf.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
                    while (lastPosition < fileSize && mmView.ReadByte(lastPosition) != 10)
                    {
                        lastPosition++;
                        chunkSize++;
                    }
                    var chunk = new byte[chunkSize];
                    _ = mmView.ReadArray(offset, chunk, 0, chunk.Length);
                    sequence++;
                    offset = lastPosition + 1;
                    chunk[chunkSize - 1] = 10; // Set last byte in the array as a New Line
                    var chunkFilePath = Path.Combine(outputDirectory, $"chunk_{sequence:D4}.txt");
                    result.Add(chunkFilePath);
                    await _fileWriter.WriteBytesAsync(chunkFilePath, chunk, bufferSize: 8192);
                }
            }
            return result;
        }
    }
}
