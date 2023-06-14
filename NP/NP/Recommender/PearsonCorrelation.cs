using System;
using NP.Data;
using NP.Models;

namespace NP.Recommender
{
    public class PearsonCorrelation
    {
        private readonly NPDbContext _context;
        private readonly DataPreprocessor _dataPreprocessor;

        public PearsonCorrelation(NPDbContext context, DataPreprocessor dataPreprocessor)
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

            var pearsonSimilarities = CalculatePearsonSimilarities(userVector, bookVectors);
            var topSimilarities = pearsonSimilarities.OrderByDescending(x => x.Value).Take(numRecommendations);

            var recommendedBookIds = topSimilarities.Select(x => x.Key);
            var recommendedBooks = _context.Books.Where(b => recommendedBookIds.Contains(b.BookId)).ToList()
                                                  .Where(b => !userRatings.Any(r => r.BookId == b.BookId)).ToList();

            return recommendedBooks;
        }

        public Dictionary<string, double> CalculatePearsonSimilarities(Dictionary<string, double> userVector, Dictionary<string, Dictionary<string, double>> bookVectors)
        {
            var pearsonSimilarities = new Dictionary<string, double>();

            foreach (var bookId in bookVectors.Keys)
            {
                var bookVector = bookVectors[bookId];

                // Calculate the intersection of features between the user and the book
                var commonFeatures = userVector.Keys.Intersect(bookVector.Keys);

                if (commonFeatures.Any())
                {
                    var userMean = userVector.Values.Average();
                    var bookMean = bookVector.Values.Average();
                    var numerator = 0.0;
                    var userDenominator = 0.0;
                    var bookDenominator = 0.0;

                    foreach (var feature in commonFeatures)
                    {
                        var userValue = userVector[feature];
                        var bookValue = bookVector[feature];

                        numerator += (userValue - userMean) * (bookValue - bookMean);
                        userDenominator += Math.Pow(userValue - userMean, 2);
                        bookDenominator += Math.Pow(bookValue - bookMean, 2);
                    }

                    var denominator = Math.Sqrt(userDenominator) * Math.Sqrt(bookDenominator);

                    // Handle cases where denominator is zero
                    if (denominator == 0)
                    {
                        pearsonSimilarities[bookId] = 0;
                    }
                    else
                    {
                        var similarity = numerator / denominator;
                        pearsonSimilarities[bookId] = similarity;
                    }
                }
                else
                {
                    pearsonSimilarities[bookId] = 0;
                }
            }

            return pearsonSimilarities;
        }


        public Dictionary<string, double> GetUserVector(List<Rating> userRatings, Dictionary<string, Dictionary<string, double>> bookRatings)
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

        public Dictionary<string, Dictionary<string, double>> CreateBookRatingsDictionary(List<Rating> allRatings)
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

        public Dictionary<string, Dictionary<string, double>> GetBookVectors(List<Rating> allRatings, Dictionary<string, Dictionary<string, double>> bookRatings)
        {
            var bookVectors = new Dictionary<string, Dictionary<string, double>>();

            foreach (var rating in allRatings)
            {
                if (!bookVectors.ContainsKey(rating.BookId) && bookRatings.ContainsKey(rating.BookId))
                {
                    bookVectors[rating.BookId] = new Dictionary<string, double>();
                }

                if (bookVectors.ContainsKey(rating.BookId))
                {
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
            }

            return bookVectors;
        }


    }
}
