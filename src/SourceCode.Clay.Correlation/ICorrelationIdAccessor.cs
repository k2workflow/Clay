namespace SourceCode.Clay.Correlation
{
    /// <summary>
    /// Interface that is implemented by classes that expose the correlation identifer getter.
    /// </summary>
    public interface ICorrelationIdAccessor
    {
        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        string CorrelationId { get; }
    }
}
