namespace SortingProject
{
    /// <summary>
    /// Compares two byte arrays, assuming each array consist of number followed by (. ) followed by text.
    /// It compares based on the text part first, when equal, it compares based on the number part.
    /// </summary>
    public class CustomComparer : IComparer<byte[]>
    {
        public int Compare(byte[]? x, byte[]? y)
        {
            //if (ReferenceEquals(x, y)) return 0; // If both are null or reference the same array.
            if (x == null) return -1; // If x is null, it's considered less.
            if (y == null) return 1; // If y is null, it's considered greater.
            int xTextIndex = 0;
            int yTextIndex = 0;
            // Find the text index after the dot and whitespace for x.
            while (xTextIndex < x.Length && x[xTextIndex] != (byte)'.')
            {
                xTextIndex++;
            }
            if (xTextIndex < x.Length) xTextIndex += 2; // Skip the dot and whitespace.
            // Find the text index after the dot and whitespace for y.
            while (yTextIndex < y.Length && y[yTextIndex] != (byte)'.')
            {
                yTextIndex++;
            }
            if (yTextIndex < y.Length) yTextIndex += 2; // Skip the dot and whitespace.
            int xNumLength = xTextIndex;
            int yNumLength = yTextIndex;
            int result;
            while (xTextIndex < x.Length && yTextIndex < y.Length)
            {
                result = ToLowerCase(x[xTextIndex]).CompareTo(ToLowerCase(y[yTextIndex]));
                if (result != 0)
                    return result;
                xTextIndex++;
                yTextIndex++;
            }
            result = (x.Length - xNumLength).CompareTo(y.Length - yNumLength);
            if (result != 0)
                return result;
            result = xNumLength.CompareTo(yNumLength);
            if (result != 0)
                return result;
            int i = 0;
            while (i <= xNumLength && i <= yNumLength)
            {
                result = x[i].CompareTo(y[i]);
                if (result != 0)
                    return result;
                i++;
            }
            return 0;
        }

        private byte ToLowerCase(byte b)
        {
            if (b >= 65 && b <= 90) // ASCII uppercase letter
            {
                return (byte)(b + 32); // Convert to lowercase
            }
            return b; // Not an uppercase letter, return as is
        }
    }
}
