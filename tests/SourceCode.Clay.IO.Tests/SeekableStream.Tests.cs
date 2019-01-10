using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SourceCode.Clay.IO.Tests
{
    public static class SeekableStreamTests
    {
        private const long WindowSize = 4097;

        private static byte ByteAt(long i) => (byte)(i + i * 2 + i / 101);

        private static Stream CreateMemoryStream(int length)
        {
            var ba = new byte[length];
            for (var i = 0L; i < length; i++)
                ba[i] = ByteAt(i);
            return new MemoryStream(ba);
        }

        private static async ValueTask ValidateCopyAsync(Stream stream)
        {
            await ValidateAsync(stream).ConfigureAwait(false);

            stream.Position = 0;
            using (var ms = new MemoryStream())
            {
                await stream.CopyToAsync(ms).ConfigureAwait(false);
                await ValidateAsync(ms).ConfigureAwait(false);
            }

            stream.Position = 0;
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                await ValidateAsync(ms).ConfigureAwait(false);
            }
        }

        private static async ValueTask ValidateAsync(Stream stream)
        {
            var ba = new byte[4096];

            stream.Position = 0;
            var total = 0L;
            while (true)
            {
                var read = await stream.ReadAsync(ba, 0, ba.Length).ConfigureAwait(false);
                if (read == 0) break;
                for (var i = 0; i < read; i++)
                    Assert.Equal(ByteAt(total++), ba[i]);
            }

            stream.Position = 0;
            total = 0L;
            while (true)
            {
                var read = stream.Read(ba, 0, ba.Length);
                if (read == 0) break;
                for (var i = 0; i < read; i++)
                    Assert.Equal(ByteAt(total++), ba[i]);
            }
        }

        [Fact]
        public static async Task SeekableStream_Read_UnknownLengthAsync()
        {
            using (Stream source = CreateMemoryStream((int)(WindowSize * 3) / 2))
            using (SeekableStream sut = await SeekableStream.CreateAsync(new StreamWrapper(source), WindowSize))
            {
                await ValidateCopyAsync(sut);
            }
        }

        [Fact]
        public static async Task SeekableStream_ReadOptional_UnknownLengthAsync()
        {
            using (Stream source = CreateMemoryStream((int)(WindowSize * 3) / 2))
            using (Stream sut = await SeekableStream.OptionalCreateAsync(new StreamWrapper(source), WindowSize))
            {
                await ValidateCopyAsync(sut);
            }
        }

        [Fact]
        public static async Task SeekableStream_Read_KnownLengthAsync()
        {
            using (Stream source = CreateMemoryStream((int)(WindowSize * 3) / 2))
            using (SeekableStream sut = await SeekableStream.CreateAsync(source, WindowSize))
            {
                await ValidateCopyAsync(sut);
            }
        }

        [Fact]
        public static async Task SeekableStream_ReadOptional_KnownLengthAsync()
        {
            using (Stream source = CreateMemoryStream((int)(WindowSize * 3) / 2))
            using (Stream sut = await SeekableStream.OptionalCreateAsync(source, WindowSize))
            {
                await ValidateCopyAsync(sut);
            }
        }
    }
}
