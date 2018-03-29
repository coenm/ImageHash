namespace CoenM.ImageSharp.ImageHash.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using CoenM.ImageSharp.ImageHash.Test.Internal;

    using FakeItEasy;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    using Xunit;

    using Sut = ImageHashExtensions;

    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Testing static extensionmethod class")]
    public class ImageHashExtensionsTest
    {
        private readonly IImageHash _hashAlgorithm;

        public ImageHashExtensionsTest()
        {
            _hashAlgorithm = A.Fake<IImageHash>();
        }

        [Fact]
        public void HashNullShouldThrowArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>("hashImplementation", () => Sut.Hash(null, new MemoryStream()));
        }

        [Fact]
        public void HashStreamNullShouldThrowArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>("stream", () => Sut.Hash(_hashAlgorithm, null));
        }

        [Fact]
        public void HashStreamShouldReadStreamAsImageAndPassDataToHashAlgorithmTest()
        {
            // arrange
            const string FILENAME = "Alyson_Hannigan_500x500_0.jpg";
            A.CallTo(() => _hashAlgorithm.Hash(A<Image<Rgba32>>._)).Returns(0UL);

            using (var stream = TestHelper.OpenStream(FILENAME))
            {
                // act
                var result = Sut.Hash(_hashAlgorithm, stream);

                // assert
                A.CallTo(() => _hashAlgorithm.Hash(A<Image<Rgba32>>._)).MustHaveHappenedOnceExactly();
                Assert.Equal(0UL, result);
            }
        }
    }
}