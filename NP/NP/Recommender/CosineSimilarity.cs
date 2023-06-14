using System;
using NP.Data;
using System.Linq;
using NP.Models;

namespace NP.Recommender
{
    public class CosineSimilarity
    {
        private readonly NPDbContext _context;
        private readonly DataPreprocessor _dataPreprocessor;

        public CosineSimilarity(NPDbContext context, DataPreprocessor dataPreprocessor)
        {
            _context = context;
            _dataPreprocessor = dataPreprocessor;
        }

        public List<Book> GetRecommendations(string userId, int numRecommendations)
        {
            var userRatings = _context.Ratings.Where(r => r.UserId == userId).ToList();
            var allRatings = _dataPreprocessor.GetSVDData();

            var bookRatings = CreateBookRatingsDictionary(allRatings);

            var userVector = GetUserVector(userRatings, bookRatings);
            var bookVectors = GetBookVectors(allRatings, bookRatings);

            var cosineSimilarities = CalculateCosineSimilarities(userVector, bookVectors);
            var topSimilarities = cosineSimilarities.OrderByDescending(x => x.Value).Take(numRecommendations);

            var recommendedBookIds = topSimilarities.Select(x => x.Key);
            var recommendedBooks = _context.Books.Where(b => recommendedBookIds.Contains(b.BookId)).ToList()
                                                  .Where(b => !userRatings.Any(r => r.BookId == b.BookId)).ToList();

            return recommendedBooks;
        }

        private Dictionary<string, double> GetUserVector(List<Rating> userRatings, Dictionary<string, Dictionary<string, double>> bookRatings)
        {
            var userVector = new Dictionary<string, double>();

            foreach (var rating in userRatings)
            {
                var bookId = rating.BookId;

                if (bookRatings.ContainsKey(bookId))
                {
                    foreach (var feature in bookRatings[bookId])
                    {
                        if (userVector.ContainsKey(feature.Key))
                        {
                            userVector[feature.Key] += feature.Value * rating.RatingValue;
                        }
                        else
                        {
                            userVector[feature.Key] = feature.Value * rating.RatingValue;
                        }
                    }
                }
            }

            return userVector;
        }

        private Dictionary<string, Dictionary<string, double>> CreateBookRatingsDictionary(List<Rating> allRatings)
        {
            var bookRatings = new Dictionary<string, Dictionary<string, double>>();

            foreach (var rating in allRatings)
            {
                var bookId = rating.BookId;

                if (!bookRatings.ContainsKey(bookId))
                {
                    bookRatings[bookId] = new Dictionary<string, double>();
                }

                var feature = $"Feature{rating.CategoryId}";
                bookRatings[bookId][feature] = rating.RatingValue;
            }

            return bookRatings;
        }

        private Dictionary<string, Dictionary<string, double>> GetBookVectors(List<Rating> allRatings, Dictionary<string, Dictionary<string, double>> bookRatings)
        {
            var bookVectors = new Dictionary<string, Dictionary<string, double>>();

            foreach (var rating in allRatings)
            {
                if (!bookVectors.ContainsKey(rating.BookId))
                {
                    bookVectors[rating.BookId] = new Dictionary<string, double>();
                }

                var feature = $"Feature{rating.Id}";
                bookVectors[rating.BookId][feature] = rating.RatingValue;

                foreach (var otherRating in bookRatings[rating.BookId])
                {
                    if (otherRating.Key != feature)
                    {
                        var otherFeature = otherRating.Key;
                        var bookId = rating.BookId;
                        if (!bookVectors.ContainsKey(bookId))
                        {
                            bookVectors[bookId] = new Dictionary<string, double>();
                        }
                        bookVectors[bookId][otherFeature] = otherRating.Value * rating.RatingValue;
                    }
                }
            }

            return bookVectors;
        }


        private Dictionary<string, double> CalculateCosineSimilarities(Dictionary<string, double> userVector, Dictionary<string, Dictionary<string, double>> bookVectors)
        {
            var cosineSimilarities = new Dictionary<string, double>();

            foreach (var bookId in bookVectors.Keys)
            {
                var dotProduct = 0.0;
                var bookVector = bookVectors[bookId];

                // Calculate dot product
                foreach (var feature in userVector.Keys)
                {
                    if (bookVector.ContainsKey(feature))
                    {
                        dotProduct += userVector[feature] * bookVector[feature];
                    }
                }

                // Calculate magnitudes
                var userMagnitude = Math.Sqrt(userVector.Values.Sum(v => v * v));
                var bookMagnitude = Math.Sqrt(bookVector.Values.Sum(v => v * v));

                // Calculate cosine similarity
                var cosineSimilarity = dotProduct / (userMagnitude * bookMagnitude);

                cosineSimilarities[bookId] = cosineSimilarity;
            }

            return cosineSimilarities;
        }

    }
}