using System.Collections.Generic;
using LMPlatform.PlagiarismNet.Data;
using LMPlatform.PlagiarismNet.XMLDocs;

namespace LMPlatform.PlagiarismNet.Services.Interfaces
{
    public  interface ISimilarity
    {
         List<SimilarityRow> MakeSimilarityRows(List<Doc> paramList, int paramInt);
    }
}
