namespace CoenM.ImageSharp.ImageHash.Test.Algorithms
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using CoenM.ImageSharp.HashAlgorithms;
    using CoenM.ImageSharp.ImageHash.Test.Internal;

    using FluentAssertions;

    using Xunit;

    public class PerceptualHashTest
    {
        private readonly PerceptualHash _sut;

        private readonly Dictionary<string, ulong> _expectedHashes = new Dictionary<string, ulong>
                                                                         {
                                                                             { "Alyson_Hannigan_500x500_0.jpg", 17839858461443178030 },
                                                                             { "Alyson_Hannigan_500x500_1.jpg", 17839823311430827566 },
                                                                             { "Alyson_Hannigan_200x200_0.jpg", 17839858461443178030 },
                                                                             { "Alyson_Hannigan_4x4_0.jpg", 17409736169497899465 },
                                                                             { "github_1.jpg", 13719320793338945348 },
                                                                             { "github_2.jpg", 13783795072850083657 }
                                                                         };

        public PerceptualHashTest()
        {
            _sut = new PerceptualHash();
        }

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 17409736169632116938)]
        [InlineData("github_1.jpg", 13719320793338945348)]
        [InlineData("github_2.jpg", 13783795072850083657)]
        public void HashImagesTest(string filename, ulong expectedHash)
        {
            // arrange
            ulong result;

            // act
            using (var stream = TestHelper.OpenStream(filename))
                result = _sut.Hash(stream);

            // assert
            result.Should().Be(expectedHash);
        }

        [Fact]
        [SuppressMessage("ReSharper", "AccessToDisposedClosure", Justification = "Manually reviewed")]
        public void NotAnImageShouldThrowExceptionTest()
        {
            // arrange
            const string FILENAME = "Not_an_image.txt";

            // act
            using (var stream = TestHelper.OpenStream(FILENAME))
            {
                Action act = () => _sut.Hash(stream);

                // assert
                act.Should().Throw<NotSupportedException>();
            }
        }

        [Fact]
        public void NullArgumentShouldThrowArgumentNullExceptionTest()
        {
            // arrange

            // act
            Action act = () => _sut.Hash(null);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ImageWithFilterShouldHaveAlmostOrExactly100Similarity1Test()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_500x500_1.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(96.875);
        }

        [Fact]
        public void ResizedImageShouldHaveAlmostOrExactly100Similarity2Test()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_200x200_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(100);
        }

        [Fact]
        public void ComparingExtreamlySmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = _expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = _expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(59.375);
        }

        [Fact]
        public void TwoDifferentImagesOfGithubArePrettySimilarTests()
        {
            // arrange
            var hash1 = _expectedHashes["github_1.jpg"];
            var hash2 = _expectedHashes["github_2.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(71.875);
        }
    }
}