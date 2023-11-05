namespace UnitTests
{
    public class FileGeneratorTests
    {
        // [ConfigurationOptions] Unit tests
        [Fact]
        public void DefaultValuesShouldBeSet()
        {
            var options = new ConfigurationOptions();
            Assert.Equal("output.txt", options.OutputFilePath);
            Assert.Equal(1024, options.FileSizeInMB);
            Assert.Equal(5, options.RandomStringMinLength);
            Assert.Equal(15, options.RandomStringMaxLength);
            Assert.Equal(1, options.RandomNumberMin);
            Assert.Equal(1000, options.RandomNumberMax);
        }

        // [FileGenerator] Unit tests
        [Fact]
        public void GenerateFile_ThrowsExceptionWhenSizeIsZero()
        {
            var fileGenerator = new FileGenerator.FileGenerator();
            var options = new ConfigurationOptions { FileSizeInMB = 0, OutputFilePath = "output.txt" };
            Assert.Throws<ArgumentException>(() => fileGenerator.GenerateFile(options));
        }

        [Fact]
        public void GenerateFile_ThrowsExceptionWhenPathIsEmpty()
        {
            var generator = new FileGenerator.FileGenerator();
            var options = new ConfigurationOptions { FileSizeInMB = 1, OutputFilePath = "" };
            Assert.Throws<ArgumentException>(() => generator.GenerateFile(options));
        }

        [Fact]
        public void GeneratedFileShouldExist()
        {
            var generator = new FileGenerator.FileGenerator();
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "testfile.txt");
            var options = new ConfigurationOptions { FileSizeInMB = 1, OutputFilePath = outputPath };
            generator.GenerateFile(options);
            Assert.True(File.Exists(outputPath));
            FileInfo fileInfo = new FileInfo(outputPath);
            Assert.Equal(1024L * 1024L, fileInfo.Length);
            File.Delete(outputPath); // Cleanup after the test
        }

        // [RandomLineGenerator] Unit tests
        [Fact]
        public void GenerateRandomLine_ThrowsExceptionWhenMaxNumberLessThanMin()
        {
            var options = new ConfigurationOptions { RandomNumberMin = 10, RandomNumberMax = 5 };
            var generator = new RandomLineGenerator(options);
            Assert.Throws<ArgumentException>(() => generator.GenerateRandomLine());
        }

        [Fact]
        public void GenerateRandomLine_ReturnsRandomLine()
        {
            var options = new ConfigurationOptions
            {
                RandomNumberMin = 1,
                RandomNumberMax = 10,
                RandomStringMinLength = 5,
                RandomStringMaxLength = 10,
            };
            var generator = new RandomLineGenerator(options);
            var randomLine = generator.GenerateRandomLine();
            Assert.NotNull(randomLine);
        }

    }
}
