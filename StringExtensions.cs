public static class StringExtensions
{
    public static int IndexOfWholeWord(this string source, string value, int startIndex, StringComparison comparison)
    {
        int index = source.IndexOf(value, startIndex, comparison);

        while (index != -1)
        {
            if ((index == 0 || !char.IsLetterOrDigit(source[index - 1]))
                && (index + value.Length == source.Length || !char.IsLetterOrDigit(source[index + value.Length])))
            {
                return index;
            }

            index = source.IndexOf(value, index + 1, comparison);
        }

        return -1;
    }
}
