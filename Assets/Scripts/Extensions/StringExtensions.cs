namespace LittleKingdom
{
    public static class StringExtensions
    {
        public static string FormatConst(this string format, params string[] args) =>
            string.Format(format, args);
    }
}