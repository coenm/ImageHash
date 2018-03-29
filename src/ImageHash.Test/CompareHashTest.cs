namespace CoenM.ImageSharp.ImageHash.Test
{
    using System;

    using FluentAssertions;

    using Xunit;

    using Sut = CompareHash;

    public class CompareHashTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(9)]
        public void SimilarityThrowsExceptionOnWrongInputSizeArgument(int size)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Similarity(new byte[size], new byte[8]));
            Assert.Throws<ArgumentOutOfRangeException>(() => Sut.Similarity(new byte[8], new byte[size]));
        }

        [Fact]
        public void OneBitDifferenceShouldResultInAlmost99ProcentSimilarityTest()
        {
            // arrange
            var hash1 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };
            var hash2 = new byte[] { 255, 255, 255, 255, 255, 255, 255, 254 };

            // act
            var result = Sut.Similarity(hash1, hash2);

            // assert
            result.Should().Be(98.4375);
        }

        [Fact]
        public void SameHashShouldResultIn100ProcentSimilarityTest()
        {
            // arrange
            var hash = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };

            // act
            var result = Sut.Similarity(hash, hash);

            // assert
            result.Should().Be(100);
        }
    }
}