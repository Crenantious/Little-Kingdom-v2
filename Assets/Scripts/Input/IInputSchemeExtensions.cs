namespace LittleKingdom.Input
{
    public static class IInputSchemeExtensions
    {
        /// <inheritdoc cref="ActiveInputScheme.Set(IInputScheme)"/>
        public static void SetActive(this IInputScheme inputScheme) =>
            ActiveInputScheme.Set(inputScheme);
    }
}