namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents the different styles for a parameter.
    /// </summary>
    public enum ParameterStyle : byte
    {
        /// <summary>
        /// The default style will be used.
        /// </summary>
        Default = 0,

        /// <summary>
        /// The matrix style will be used.
        /// </summary>
        Matrix = 1,

        /// <summary>
        /// The label style will be used.
        /// </summary>
        Label = 2,

        /// <summary>
        /// The form style will be used.
        /// </summary>
        Form = 3,

        /// <summary>
        /// The simple style will be used.
        /// </summary>
        Simple = 4,

        /// <summary>
        /// The space-delimited style will be used.
        /// </summary>
        SpaceDelimited = 5,

        /// <summary>
        /// The pipe-delimited style will be used.
        /// </summary>
        PipeDelimited = 6,

        /// <summary>
        /// The deep object style will be used.
        /// </summary>
        DeepObject = 7
    }
}
