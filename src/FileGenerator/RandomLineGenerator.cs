namespace FileGenerator
{
    public class RandomLineGenerator : ILineGenerator
    {
        private readonly Random _random;
        private readonly ConfigurationOptions _options;

        public RandomLineGenerator(ConfigurationOptions options)
        {
            _options = options;
            _random = new Random();
        }

        public byte[] GenerateRandomLine(string optionalText = "")
        {
            if (_options.RandomNumberMin >= _options.RandomNumberMax) throw new ArgumentException("Maximum Random Number value must be greater than Minimum Random Number value.");
            if (_options.RandomStringMinLength >= _options.RandomStringMaxLength) throw new ArgumentException("Maximum String Length value must be greater than Minimum String Length value.");

            using var memoryStream = new MemoryStream();
            var randomNumber = _random.Next(_options.RandomNumberMin, _options.RandomNumberMax).ToString();
            var stringLength = _random.Next(_options.RandomStringMinLength, _options.RandomStringMaxLength);

            // Write the generated random number to the stream
            foreach (var b in randomNumber)
            {
                memoryStream.WriteByte((byte)b);
            }
            memoryStream.WriteByte(Constants.PeriodCharacter); // Adding (.) after the number array
            memoryStream.WriteByte(Constants.SpaceCharacter); // Adding white space after the (.)

            // Generate random characters within the specified range between 'A' to 'Z'
            if (string.IsNullOrEmpty(optionalText))
            {
                GenerateRandomByteArray(memoryStream, stringLength, Constants.MinCharacterRange, Constants.MaxCharacterRange + 1);
            }
            else
            {
                // Write the provided optional text to the stream
                foreach (var c in optionalText)
                {
                    memoryStream.WriteByte((byte)c);
                }
            }
            memoryStream.WriteByte(Constants.NewLineCharacter); // Adding New Line
            return memoryStream.ToArray();
        }

        private void GenerateRandomByteArray(MemoryStream memoryStream, int length, int minValue, int maxValue)
        {
            for (var i = 0; i < length; i++)
            {
                memoryStream.WriteByte((byte)_random.Next(minValue, maxValue));
            }
        }
    }
}
