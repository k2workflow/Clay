using System.Collections.Generic;

namespace SourceCode.Clay.Randoms
{
    public interface IRandom
    {
        /// <summary>
        /// Returns the next random number within the specified range.
        /// </summary>
        double NextDouble();

        /// <summary>
        /// Returns a sequence of random numbers within the specified range.
        /// </summary>
        /// <param name="count">The number of samples to generate.</param>
        IEnumerable<double> Sample(int count);
    }
}
