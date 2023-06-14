using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NP.Data;
using MathNet.Numerics.Statistics;
using System.Linq;
using NP.Models;

public class DataPreprocessor
{
    private readonly NPDbContext _context;

    public DataPreprocessor(NPDbContext context)
    {
        _context = context;
    }

    public List<Rating> GetSVDData()
    {
        // Get all the ratings
        var ratings = _context.Ratings.ToList();

        // Get all the unique users and books
        var users = ratings.Select(r => r.UserId).Distinct().ToList();
        var books = ratings.Select(r => r.BookId).Distinct().ToList();

        // Create a dictionary of user indices and book indices
        var userIndexDict = new Dictionary<string, int>();
        var bookIndexDict = new Dictionary<string, int>();

        for (int i = 0; i < users.Count; i++)
        {
            userIndexDict[users[i]] = i;
        }

        for (int i = 0; i < books.Count; i++)
        {
            bookIndexDict[books[i]] = i;
        }

        // Create a user-book rating matrix
        var ratingMatrix = DenseMatrix.Create(users.Count, books.Count, 0.0);

        foreach (var rating in ratings)
        {
            int userIndex = userIndexDict[rating.UserId];
            int bookIndex = bookIndexDict[rating.BookId];
            ratingMatrix[userIndex, bookIndex] = rating.RatingValue;
        }

        // Perform Singular Value Decomposition (SVD)
        var svd = ratingMatrix.Svd();

        // Get the U, S, and Vt matrices from SVD
        var U = svd.U;
        var S = svd.S;
        var Vt = svd.VT.Transpose();

        // Create a list of Rating objects
        var ratingList = new List<Rating>();

        for (int i = 0; i < users.Count; i++)
        {
            var userId = users[i];

            for (int j = 0; j < books.Count; j++)
            {
                var bookId = books[j];
                var ratingValue = ratingMatrix[i, j];

                ratingList.Add(new Rating
                {
                    UserId = userId,
                    BookId = bookId,
                    RatingValue = ratingValue
                });
            }
        }

        // Return the list of Rating objects
        return ratingList;
    }


    public Matrix<double> RemoveOutliers(Matrix<double> matrix, double zScoreThreshold)
    {
        // Calculate the mean and standard deviation of each column
        var means = new List<double>();
        var stdDevs = new List<double>();
        for (int i = 0; i < matrix.ColumnCount; i++)
        {
            var column = matrix.Column(i);
            means.Add(column.Mean());
            stdDevs.Add(column.StandardDeviation());
        }

        // Initialize a new matrix to hold the processed data
        var processedMatrix = DenseMatrix.Create(matrix.RowCount, matrix.ColumnCount, 0.0);

        // Remove outliers from each column
        for (int colIndex = 0; colIndex < matrix.ColumnCount; colIndex++)
        {
            var colValues = matrix.Column(colIndex).ToArray();
            double colMean = means[colIndex];
            double colStdDev = stdDevs[colIndex];

            for (int rowIndex = 0; rowIndex < matrix.RowCount; rowIndex++)
            {
                double value = matrix[rowIndex, colIndex];
                double zScore = (value - colMean) / colStdDev;

                if (Math.Abs(zScore) <= zScoreThreshold)
                {
                    processedMatrix[rowIndex, colIndex] = value;
                }
            }
        }

        return processedMatrix;
    }

    public Matrix<double> FillMissingData(Matrix<double> matrix)
    {
        // Initialize a new matrix to hold the processed data
        var processedMatrix = DenseMatrix.Create(matrix.RowCount, matrix.ColumnCount, 0.0);

        // Fill in missing values using column means
        for (int colIndex = 0; colIndex < matrix.ColumnCount; colIndex++)
        {
            var colValues = matrix.Column(colIndex).ToArray();
            double colMean = colValues.Where(v => v != 0.0).Average();

            for (int rowIndex = 0; rowIndex < matrix.RowCount; rowIndex++)
            {
                double value = matrix[rowIndex, colIndex];

                if (value == 0.0)
                {
                    processedMatrix[rowIndex, colIndex] = colMean;
                }
                else
                {
                    processedMatrix[rowIndex, colIndex] = value;
                }
            }
        }

        return processedMatrix;
    }

    public List<Rating> GetUserRatingsByUserId(string userId)
    {
        return _context.Ratings.Where(r => r.UserId == userId).ToList();
    }

    public List<Rating> GetBookRatings(string bookId)
    {
        return _context.Ratings.Where(r => r.BookId == bookId).ToList();
    }


}


public static class MatrixExtensions
{
    public static Vector<double> ColumnMeans(this Matrix<double> matrix)
    {
        return DenseVector.Create(matrix.ColumnCount, i => matrix.Column(i).Average());
    }
}