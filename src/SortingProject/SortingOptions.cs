using CommandLine;

namespace SortingProject
{
    public class SortingOptions
    {
        [Option(shortName: 'i', longName: "input-file-path", Default = "temp/input_1000.txt", Required = false, HelpText = "Input file path to be sorted.")]
        public string? InputFilePath { get; set; }
        [Option(shortName: 'o', longName: "output-file-name", Default = "output.txt", Required = false, HelpText = "Sorted output file name.")]
        public string? OutputFileName { get; set; }
        [Option(shortName: 'd', longName: "output-directory", Default = "temp", Required = false, HelpText = "Output directory.")]
        public string? OutputDirectory { get; set; }
        [Option(shortName: 'c', longName: "chunk-size", Default = 100, Required = false, HelpText = "Chunk size in MB.")]
        public int ChunkSizeInMB { get; set; }
    }
}
