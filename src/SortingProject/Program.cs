using CommandLine;
using System.Diagnostics;

namespace SortingProject
{
    public static class Program
    {
        private static async Task Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var totalTime = 0D;
            var options = Parser.Default.ParseArguments<SortingOptions>(args).Value;

            if (string.IsNullOrEmpty(options.OutputFileName)) throw new ArgumentNullException(options.OutputFileName);
            if (string.IsNullOrEmpty(options.OutputDirectory)) throw new ArgumentNullException(options.OutputDirectory);
            if (!Directory.Exists(options.OutputDirectory)) _ = Directory.CreateDirectory(options.OutputDirectory);

            var outputFile = Path.Combine(options.OutputDirectory, options.OutputFileName);
            IFileWriter fileWriter = new FileWriter();
            IFileSplitter fileSplitter = new FileSplitter(fileWriter);
            FileSorter fileSorter = new(fileWriter);
            FileMerger fileMerger = new();

            stopwatch.Restart();
            var chunks = await fileSplitter.SplitFileAsync(options);
            totalTime += stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"Split File done in {stopwatch.Elapsed.TotalSeconds} seconds");
            stopwatch.Restart();
            var sortedFiles = await fileSorter.SortFilesAsync(options, chunks);
            Thread.Sleep(2000);
            totalTime += stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"Sort Files done in {stopwatch.Elapsed.TotalSeconds} seconds");
            stopwatch.Restart();
            fileMerger.MergeFiles(sortedFiles, outputFile);
            totalTime += stopwatch.Elapsed.TotalSeconds;
            Console.WriteLine($"Merge Files done in {stopwatch.Elapsed.TotalSeconds} seconds");

            Console.WriteLine("File sorted successfully!");
            Console.WriteLine($"Time taken: {totalTime} seconds.");
            Console.WriteLine($"File path: {outputFile}");
            _ = Console.ReadLine();
        }
    }
}