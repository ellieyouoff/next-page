using System.Collections.Generic;
using NP.Data;
using NP.Models;
using NP.Search;
using Xunit;

namespace NPTests
{
    public class KMPTests
    {
        [Fact]
        public void KMPSearch_ReturnsTrue_WhenPatternExistsInText()
        {
            // Arrange
            string text = "The quick brown fox jumps over the lazy dog";
            string pattern = "brown fox";

            // Act
            bool result = KMP.KMPSearch(text, pattern);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void KMPSearch_ReturnsFalse_WhenPatternDoesNotExistInText()
        {
            // Arrange
            string text = "The quick brown fox jumps over the lazy dog";
            string pattern = "pink elephant";

            // Act
            bool result = KMP.KMPSearch(text, pattern);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ComputeLPSArray_ReturnsCorrectArray()
        {
            // Arrange
            string pattern = "ABABC";

            // Act
            int[] result = KMP.ComputeLPSArray(pattern, pattern.Length);

            // Assert
            Assert.Equal(new int[] { 0, 0, 1, 2, 0 }, result);
        }



    }
}



