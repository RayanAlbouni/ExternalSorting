using SortingProject;

namespace UnitTests
{
    // Mock class for IFileWriter to avoid actual file writes in tests.
    internal class FileWriterMock : IFileWriter
    {
        public Task WriteBytesAsync(string filePath, byte[] data, int bufferSize)
        {
            // Implement mock logic to avoid actual file writes
            return Task.CompletedTask;
        }
    }
}
