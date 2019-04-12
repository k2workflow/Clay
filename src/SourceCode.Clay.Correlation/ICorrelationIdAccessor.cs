namespace SourceCode.Clay.Correlation
{
    /// <summary>
    /// Interface that is implemented by classes that expose the correlation identifer getter.
    /// </summary>
    /// <typeparam name="T">The type of value to use as the correlation identifier.</typeparam>
    public interface ICorrelationIdAccessor<T>
    {
        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        T CorrelationId { get; }
    }
}
