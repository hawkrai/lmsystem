using System.Collections.Generic;
using LMPlatform.PlagiarismNet.Data;

namespace LMPlatform.PlagiarismNet.Services.Interfaces
{
    public interface IGreedy
    {
        List<Cluster> Clustering(List<SimilarityRow> paramList, int paramInt);
    }
}
