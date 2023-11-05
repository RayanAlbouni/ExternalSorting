namespace FileGenerator
{
    public interface ILineGenerator
    {
        byte[] GenerateRandomLine(string optionalText = "");
    }

}
