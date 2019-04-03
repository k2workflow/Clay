namespace SourceCode.Clay.Distributed
{
    /// <summary>
    /// Represents a factory for <see cref="DistributedId"/> values.
    /// </summary>
    public interface IDistributedIdFactory
    {
        /// <summary>
        /// Creates a new <see cref="DistributedId"/>.
        /// </summary>
        /// <returns>The <see cref="DistributedId"/>.</returns>
        DistributedId Create();
    }
}
