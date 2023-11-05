using System.IO.MemoryMappedFiles;

namespace SortingProject
{
    /// <summary>
    /// Represents a class for sorting files.
    /// </summary>
    public class FileSorter : IFileSorter
    {
        private readonly IFileWriter _fileWriter;
        public FileSorter(IFileWriter fileWriter)
        {
            _fileWriter = fileWriter;
        }

        /// <summary>
        /// Asynchronously sorts and deletes input files.
        /// </summary>
        /// <param name="options">The sorting options to use.</param>
        /// <param name="files">The list of file paths to be sorted.</param>
        /// <returns>A list of sorted file paths.</returns>
        public async Task<IList<string>> SortFilesAsync(SortingOptions options, IList<string> files)
        {
            var result = new List<string>();
            var parallelOptions = new ParallelOptions()
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };
            // Use parallel processing to sort files asynchronously.
            // We may process the sorting synchronously to improve the memory usage, however, it affects the speed of execution.
            await Parallel.ForEachAsync(files, parallelOptions, async (chunkFile, ct) =>
            {
                await SortFileAsync(chunkFile, result);
            });
            foreach (var file in files)
            {
                File.Delete(file);
            }
            return result;
        }

        private async Task SortFileAsync(string chunkFile, List<string> result)
        {
            using var sourceStream = File.OpenRead(chunkFile);
            using var mmf = MemoryMappedFile.CreateFromFile(sourceStream, null, 0, MemoryMappedFileAccess.Read, HandleInheritability.None, false);
            using var mmView = mmf.CreateViewAccessor(0, sourceStream.Length, MemoryMappedFileAccess.Read);
            byte[] fileBytes = new byte[sourceStream.Length];
            _ = mmView.ReadArray(0, fileBytes, 0, (int)sourceStream.Length);
            int linesCount = File.ReadAllLines(chunkFile).Length;
            byte[][] split = SplitBytesByNewLine(fileBytes, linesCount);
            Array.Sort(split, new CustomComparer());
            var merged = MergeBytesByNewLine(split, (int)sourceStream.Length);
            string sortedChunkFile = chunkFile.Replace("chunk_", "sorted_");
            result.Add(sortedChunkFile);
            await _fileWriter.WriteBytesAsync(sortedChunkFile, merged, bufferSize: 8192);
        }

        private byte[][] SplitBytesByNewLine(byte[] chunk, int linesCount)
        {
            byte[][] result = new byte[linesCount][];
            int start = 0;
            int lineIndex = 0;
            for (int i = 0; i < chunk.Length; i++)
            {
                if (chunk[i] == (byte)'\n')
                {
                    int length = i - start + 1;
                    result[lineIndex] = new byte[length];
                    Array.Copy(chunk, start, result[lineIndex], 0, length);
                    lineIndex++;
                    start = i + 1;
                }
            }
            if (start < chunk.Length)
            {
                int length = chunk.Length - start;
                result[lineIndex] = new byte[length];
                Array.Copy(chunk, start, result[lineIndex], 0, length);
            }
            return result;
        }

        private byte[] MergeBytesByNewLine(byte[][] chunk, int chumkSize)
        {
            var result = new byte[chumkSize];
            int offset = 0;
            for (int i = 0; i < chunk.Length; i++)
            {
                Buffer.BlockCopy(chunk[i], 0, result, offset, chunk[i].Length);
                offset += chunk[i].Length;
            }
            return result;
        }
    }
}
