using System.IO.MemoryMappedFiles;

namespace FileGenerator
{
    public class FileGenerator : IFileGenerator
    {
        public void GenerateFile(ConfigurationOptions options)
        {
            if (options.FileSizeInMB == 0) throw new ArgumentException("File size must be a positive value.");
            if (string.IsNullOrEmpty(options.OutputFilePath)) throw new ArgumentException("File path must not be empty.");

            // Calculate the target file size in bytes
            var targetFileSize = options.FileSizeInMB * 1024L * 1024L; // Convert MB to bytes

            using var mmf = MemoryMappedFile.CreateFromFile(options.OutputFilePath, FileMode.Create, null, targetFileSize, MemoryMappedFileAccess.ReadWrite);
            using var accessor = mmf.CreateViewAccessor();
            var lineGenerator = new RandomLineGenerator(options);
            long position = 0;
            int lineCount = 0;
            while (position < targetFileSize)
            {
                var randomLine = lineGenerator.GenerateRandomLine();

                if (position + randomLine.Length > targetFileSize)
                {
                    // When we reach the end of the file, we trim the random line to fit in the file bytes.
                    accessor.WriteArray(position, randomLine, 0, (int)(targetFileSize - position));
                    break;
                }

                accessor.WriteArray(position, randomLine, 0, randomLine.Length);
                position += randomLine.Length;
                lineCount++;
            }
        }
    }
}
