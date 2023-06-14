using System;
using NP.Models;
namespace NP.Recommender
{
	public class Recommender
	{
        private readonly PearsonCorrelation _pearson;
        private readonly CosineSimilarity _cosine;

		public Recommender(PearsonCorrelation pearson, CosineSimilarity cosine)
		{
            _pearson = pearson;
            _cosine = cosine;

		}

        public List<Book> PearsonRecommender(string userId)
        {
            var recommendations = _pearson.GetRecommendations(userId, 10).ToList();

            return recommendations;
        }


        public List<Book> CosineRecommender(string userId)
        {
            var recommendations = _cosine.GetRecommendations(userId, 10);

            return recommendations;
        }

    }
}

