using SortingProject;

namespace UnitTests
{
    public class SortingProjectTests
    {
        // [FileSplitter] Unit tests
        [Fact]
        public async Task SplitFileAsync_InvalidInput_ThrowsException()
        {
            // Arrange
            var fileWriter = new FileWriterMock();
            var splitter = new FileSplitter(fileWriter);
            var options = new SortingOptions(); // Missing required properties

            // Act and Assert
            _ = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return splitter.SplitFileAsync(options);
            });
        }

        [Fact]
        public async Task SplitFileAsync_ChunkSizeZero_ThrowsException()
        {
            // Arrange
            var fileWriter = new FileWriterMock();
            var splitter = new FileSplitter(fileWriter);
            var options = new SortingOptions
            {
                InputFilePath = "input.txt",
                OutputFileName = "output.txt",
                OutputDirectory = "output",
                ChunkSizeInMB = 0 // Invalid chunk size
            };

            // Act and Assert
            _ = await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                return splitter.SplitFileAsync(options);
            });
        }

        [Fact]
        public async Task SplitFileAsync_InputFileNotFound_ThrowsException()
        {
            // Arrange
            var fileWriter = new FileWriterMock();
            var splitter = new FileSplitter(fileWriter);
            var options = new SortingOptions
            {
                InputFilePath = "", // Nonexistent file
                OutputFileName = "output.txt",
                OutputDirectory = "output",
                ChunkSizeInMB = 1
            };

            // Act and Assert
            _ = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return splitter.SplitFileAsync(options);
            });
        }

        [Fact]
        public async Task SplitFileAsync_OutputDirectoryNotFound_ThrowsException()
        {
            // Arrange
            var fileWriter = new FileWriterMock();
            var splitter = new FileSplitter(fileWriter);
            var options = new SortingOptions
            {
                InputFilePath = "input.txt",
                OutputFileName = "output.txt",
                OutputDirectory = "", // Nonexistent directory
                ChunkSizeInMB = 1
            };

            // Act and Assert
            _ = await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                return splitter.SplitFileAsync(options);
            });
        }

        // [FileSorter] Unit tests
        [Fact]
        public async Task SortFilesAsync_NoFilesToSort_ReturnsEmptyList()
        {
            // Arrange
            var fileWriter = new FileWriterMock();
            var fileSorter = new FileSorter(fileWriter);
            var sortingOptions = new SortingOptions();
            var files = new List<string>();

            // Act
            var result = await fileSorter.SortFilesAsync(sortingOptions, files);

            // Assert
            Assert.Empty(result); // The result should be an empty list.
        }
    }
}

