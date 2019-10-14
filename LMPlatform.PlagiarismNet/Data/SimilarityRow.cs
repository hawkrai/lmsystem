using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMPlatform.PlagiarismNet.XMLDocs;

namespace LMPlatform.PlagiarismNet.Data
{
    public class SimilarityRow
    {
        public Doc Doc { get; set; }
        public Dictionary<Doc, int> Similarity { get; set; }

	    public SimilarityRow()
	    {
		    Similarity = new Dictionary<Doc, int>();
	    }

        public int GetSumSimilarity()
        {
            if ((Similarity == null) || (Similarity.Count < 1))
                return 0;
            int total = 0;
            foreach (var sim in Similarity.Values)
            {
                total += sim;
            }
            
            return total;
        }

        public void RemoveSimilarityByIndex(List<Doc> docs)
        {
            foreach (var doc in docs)
            {
                Similarity.Remove(doc);
            }
            
        }

    }
}