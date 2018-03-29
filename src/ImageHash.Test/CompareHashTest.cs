namespace CoenM.ImageSharp.ImageHash.Test
{
    using System;

    using Xunit;

    using Sut = CoenM.ImageSharp.CompareHash;

    public class CompareHashTest
    {
        [Fact]
        public void SimilarityThrowsExceptionOnNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => Sut.Similarity(null, null));
            Assert.Throws<ArgumentNullException>(() => Sut.Similarity(null, new byte[8]));
            Assert.Throws<ArgumentNullException>(() => Sut.Similarity(new byte[8], null));
        }

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
            Assert.Equal(98.4375, result);
        }

        [Fact]
        public void SameHashShouldResultIn100ProcentSimilarityTest()
        {
            // arrange
            var hash = new byte[] { 255, 255, 255, 255, 255, 255, 255, 255 };

            // act
            var result = Sut.Similarity(hash, hash);

            // assert
            Assert.Equal(100, result);
        }
    }
}