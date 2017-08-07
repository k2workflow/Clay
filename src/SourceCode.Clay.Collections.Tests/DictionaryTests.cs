using Xunit;

namespace SourceCode.Clay.Collections.Generic.Tests
{
    public static class DictionaryTests
    {
        [Trait("Type", "Unit")]
        [Fact(DisplayName = "Dictionary.Empty")]
        public static void Use_Dictionary_Empty()
        {
            var empty = Dictionary.Empty<string, int>();

            Assert.Equal(empty.Count, 0);
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = "ReadOnlyDictionary.Empty")]
        public static void Use_ReadOnlyDictionary_Empty()
        {
            var empty = ReadOnlyDictionary.Empty<string, int>();

            Assert.Equal(empty.Count, 0);
        }
    }
}
