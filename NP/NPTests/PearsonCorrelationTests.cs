using System.Collections.Generic;
using System.Linq;
using NP.Data;
using NP.Models;
using NP.Recommender;
using NuGet.ContentModel;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections;
using System.ComponentModel;

namespace NPTests
{
    public class PearsonCorrelationTests
    {
        [Fact]
            public void CreateBookRatingsDictionary_ReturnsCorrectDictionary()
            {
                // Arrange

                var options = new DbContextOptionsBuilder<NPDbContext>()
                   .UseInMemoryDatabase(databaseName: "Test")
                   .Options;
                var mockContext = new NPDbContext(options);
                var mockDataPreprocessor = new Mock<DataPreprocessor>();
                var correlation = new PearsonCorrelation(mockContext, new DataPreprocessor(mockContext));

                var allRatings = new List<Rating>()
        {
            new Rating() { BookId = "1", CategoryId = 1, RatingValue = 4 },
            new Rating() { BookId = "1", CategoryId = 2, RatingValue = 5 },
            new Rating() { BookId = "2", CategoryId = 1, RatingValue = 3 },
            new Rating() { BookId = "2", CategoryId = 3, RatingValue = 2 },
            new Rating() { BookId = "3", CategoryId = 2, RatingValue = 5 },
            new Rating() { BookId = "3", CategoryId = 3, RatingValue = 3 },
            new Rating() { BookId = "4", CategoryId = 1, RatingValue = 2 },
            new Rating() { BookId = "4", CategoryId = 2, RatingValue = 1 },
        };

                // Act
                var result = correlation.CreateBookRatingsDictionary(allRatings);

                // Assert
                Assert.Equal(4, result.Count);

                var book1 = result["1"];
                Assert.Equal(2, book1.Count);
                Assert.Equal(4, book1["Feature1"]);
                Assert.Equal(5, book1["Feature2"]);

                var book2 = result["2"];
                Assert.Equal(2, book2.Count);
                Assert.Equal(3, book2["Feature1"]);
                Assert.Equal(2, book2["Feature3"]);

                var book3 = result["3"];
                Assert.Equal(2, book3.Count);
                Assert.Equal(5, book3["Feature2"]);
                Assert.Equal(3, book3["Feature3"]);
                var book4 = result["4"];
                Assert.Equal(2, book4.Count);
                Assert.Equal(2, book4["Feature1"]);
                Assert.Equal(1, book4["Feature2"]);
            }
        }
    }

