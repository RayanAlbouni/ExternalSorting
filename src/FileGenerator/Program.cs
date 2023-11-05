using CommandLine;
using System.Diagnostics;

namespace FileGenerator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            var options = Parser.Default.ParseArguments<ConfigurationOptions>(args).Value;
            var largeFileGenerator = new FileGenerator();
            largeFileGenerator.GenerateFile(options);
            Console.WriteLine("File generated successfully!");
            Console.WriteLine($"Time taken: {stopwatch.Elapsed.TotalSeconds} seconds.");
            Console.WriteLine($"File path: {options.OutputFilePath}");
            Console.ReadLine();
        }
    }

}