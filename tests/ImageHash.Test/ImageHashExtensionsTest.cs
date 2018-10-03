namespace CoenM.ImageHash.Test
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    using CoenM.ImageHash.Test.Internal;

    using FakeItEasy;

    using FluentAssertions;

    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;

    using Xunit;

    using Sut = ImageHashExtensions;

    [SuppressMessage("ReSharper", "InvokeAsExtensionMethod", Justification = "Testing static extension method class")]
    public class ImageHashExtensionsTest
    {
        private readonly IImageHash _hashAlgorithm;

        public ImageHashExtensionsTest()
        {
            _hashAlgorithm = A.Fake<IImageHash>();
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
                result.Should().Be(0UL);
            }
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void NullArgumentShouldThrowArgumentNullExceptionTest(bool hashImplIsNull, bool streamIsNull)
        {
            // arrange
            IImageHash imageHashImplementation = hashImplIsNull ? null : A.Dummy<IImageHash>();
            Stream stream = streamIsNull ? null : new MemoryStream();

            // act
            Action act = () => Sut.Hash(imageHashImplementation, stream);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}