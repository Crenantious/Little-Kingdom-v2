namespace LittleKingdom
{
    /// <summary>
    /// Automatically calls <see cref="Validate"/> when created with the primary constructor (positional record).
    /// Also, does not work when created using the "with" keyword.
    /// </summary>
    public abstract record ValidatableRecord
    {
        protected ValidatableRecord() => Validate();

        public abstract void Validate();
    }
}