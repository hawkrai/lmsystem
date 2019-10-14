using LMPlatform.PlagiarismNet.Services;
using LMPlatform.PlagiarismNet.Services.Interfaces;

namespace LMPlatform.PlagiarismNet.Utils
{
    public class ClusteringFactory
    {
        private static IGreedy _greedy;
        private static ISimilarity _similarity;
        private static IMyStem _myStem;

        public ClusteringFactory() { }

        public static IGreedy GetGreedy()
        {
            return _greedy ?? (_greedy = new Greedy());
        }

        public static ISimilarity GetSimilarity(int mode)
        {
	        if (mode == 0)
	        {
		        _similarity = new Similarity();
	        }
	        else
	        {
		        _similarity = new SimilarityByShingle();
	        }

	        return _similarity;
        }

        public static IMyStem GetMyStem()
        {
            return _myStem ?? (_myStem = new MyStem());
        }
    }
}