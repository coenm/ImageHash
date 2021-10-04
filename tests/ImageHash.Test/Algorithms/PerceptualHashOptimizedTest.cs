namespace CoenM.ImageHash.Test.Algorithms
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using CoenM.ImageHash.HashAlgorithms;
    using CoenM.ImageHash.Test.Internal;
    using FluentAssertions;
    using Xunit;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.PixelFormats;
    using System.Diagnostics;
    using Xunit.Abstractions;

    public class PerceptualHashOptimizedTest
    {
        private readonly PerceptualHashOptimized sut;
        private readonly ITestOutputHelper _logger;

        private readonly Dictionary<string, ulong> expectedHashes = new Dictionary<string, ulong>
        {
            { "Alyson_Hannigan_500x500_0.jpg", 17839858461443178030 },
            { "Alyson_Hannigan_500x500_1.jpg", 17839823311430827566 },
            { "Alyson_Hannigan_200x200_0.jpg", 17839858461443178030 },
            { "Alyson_Hannigan_4x4_0.jpg", 17409736169497899465 },
            { "github_1.jpg", 13719320793338945348 },
            { "github_2.jpg", 13783795072850083657 },
        };

        public PerceptualHashOptimizedTest(ITestOutputHelper logger)
        {
            _logger = logger;
            sut = new PerceptualHashOptimized();
        }

        [Theory]
        [InlineData("Alyson_Hannigan_500x500_0.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_500x500_1.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_200x200_0.jpg", 17839823311430827566)]
        [InlineData("Alyson_Hannigan_4x4_0.jpg", 17409736169531453642)]
        [InlineData("github_1.jpg", 13719320793338945348)]
        [InlineData("github_2.jpg", 13783795072850083657)]
        public void HashImagesTest(string filename, ulong expectedHash)
        {
            // arrange
            ulong result;

            // act
            using (var stream = TestHelper.OpenStream(filename))
                result = sut.Hash(stream);

            // assert
            result.Should().Be(expectedHash);
        }

        [Fact]
        [SuppressMessage("ReSharper", "AccessToDisposedClosure", Justification = "Manually reviewed")]
        public void NotAnImageShouldThrowExceptionTest()
        {
            // arrange
            const string filename = "Not_an_image.txt";

            // act
            using (var stream = TestHelper.OpenStream(filename))
            {
                Action act = () => sut.Hash(stream);

                // assert
                act.Should().Throw<SixLabors.ImageSharp.UnknownImageFormatException>();
            }
        }

        [Fact]
        public void NullArgumentShouldThrowArgumentNullExceptionTest()
        {
            // arrange

            // act
            Action act = () => sut.Hash(null);

            // assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ImageWithFilterShouldHaveAlmostOrExactly100Similarity1Test()
        {
            // arrange
            var hash1 = expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = expectedHashes["Alyson_Hannigan_500x500_1.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(96.875);
        }

        [Fact]
        public void ResizedImageShouldHaveAlmostOrExactly100Similarity2Test()
        {
            // arrange
            var hash1 = expectedHashes["Alyson_Hannigan_500x500_0.jpg"];
            var hash2 = expectedHashes["Alyson_Hannigan_200x200_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(100);
        }

        [Fact]
        public void ComparingExtreamlySmallImageShouldDecreaseSimilarityTest()
        {
            // arrange
            var hash1 = expectedHashes["Alyson_Hannigan_4x4_0.jpg"];
            var hash2 = expectedHashes["Alyson_Hannigan_500x500_0.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(59.375);
        }

        [Fact]
        public void TwoDifferentImagesOfGithubArePrettySimilarTests()
        {
            // arrange
            var hash1 = expectedHashes["github_1.jpg"];
            var hash2 = expectedHashes["github_2.jpg"];

            // act
            var result = CompareHash.Similarity(hash1, hash2);

            // assert
            result.Should().Be(71.875);
        }

        private static Image<Rgba32>[] GetImageSuite()
        {
            // Yes, this is beefy but we want everything in RAM before we do stuff
            return Directory.GetFiles(Path.Combine("Data", "image_suite"), "*.jpg")
                        .Select(fp => Image.Load<Rgba32>(fp))
                        .ToArray();
        }

        [Fact]
        public void CompareAgainstUnoptimized()
        {
            // Do optimized first in case the second run gets a boost we bias towards the incumbent
            GC.Collect();
            var optimized = new PerceptualHashOptimized();
            var optimizedImages = GetImageSuite();
            var optimizedResults = new List<UInt64>();
            var clock = Stopwatch.StartNew();
            foreach (var i in optimizedImages)
            {
                var hash = optimized.Hash(i);
                optimizedResults.Add(hash);
            }
            var optimizedTime = clock.ElapsedMilliseconds;

            // To ensure we have similar RAM pressure
            foreach(var i in optimizedImages)
            {
                i.Dispose();
            }
            optimizedImages = null;
            GC.Collect();


            var unoptimized = new PerceptualHash();
            var unoptimizedResults = new List<UInt64>();
            var unoptimizedImages = GetImageSuite();
            clock.Restart();
            foreach (var i in unoptimizedImages)
            {
                var hash = unoptimized.Hash(i);
                unoptimizedResults.Add(hash);
            }
            var unoptimizedTime = clock.ElapsedMilliseconds;


            foreach (var i in unoptimizedImages)
            {
                i.Dispose();
            }
            unoptimizedImages = null;

            Assert.Equal(unoptimizedResults.Count, optimizedResults.Count);
            for(int i=0; i<optimizedResults.Count; i++)
            {
                Assert.Equal(unoptimizedResults[i], optimizedResults[i]);
            }

            _logger.WriteLine($"Unoptimized: {unoptimizedTime}, Optimized: {optimizedTime}");
        }
    }
}
