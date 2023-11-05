using CommandLine;

namespace FileGenerator
{
    public class ConfigurationOptions
    {
        [Option(shortName: 'f', longName: "file-path", Default = "output.txt", Required = false, HelpText = "Output file to be generated.")]
        public string? OutputFilePath { get; set; } = "output.txt";
        [Option(shortName: 's', longName: "file-size", Default = 1024, Required = false, HelpText = "File size in MB.")]
        public int FileSizeInMB { get; set; } = 1024;
        [Option(shortName: 't', longName: "string-min", Default = 5, Required = false, HelpText = "Random string minimum length.")]
        public int RandomStringMinLength { get; set; } = 5;
        [Option(shortName: 'T', longName: "text-max", Default = 15, Required = false, HelpText = "Random string maximum length.")]
        public int RandomStringMaxLength { get; set; } = 15;
        [Option(shortName: 'n', longName: "number-min", Default = 1, Required = false, HelpText = "Random number minimum length.")]
        public int RandomNumberMin { get; set; } = 1;
        [Option(shortName: 'N', longName: "number-max", Default = 1000, Required = false, HelpText = "Random number maximum length.")]
        public int RandomNumberMax { get; set; } = 1000;
    }
}
