using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMPlatform.PlagiarismNet.Data;
using LMPlatform.PlagiarismNet.Services.Interfaces;
using LMPlatform.PlagiarismNet.XMLDocs;

namespace LMPlatform.PlagiarismNet.Services
{
    public class Greedy : IGreedy
    {
        public static int THRESHOLD = 90;

        public List<Cluster> Clustering(List<SimilarityRow> rows, int threshold)
        {
            if (threshold == 0)
                threshold = THRESHOLD;

            List<Cluster> clusters = new List<Cluster>();
            int size = rows.Count();
            //пока есть нераспределенные документы
            while (size > 0)
            {
                //ищем строку с максимальной суммой
                int maxSumSimilarity = rows.Max(y => y.GetSumSimilarity());
                SimilarityRow maxRow = rows.Find(x => x.GetSumSimilarity() == maxSumSimilarity);

                Cluster cluster = new Cluster();
                foreach (var entry in maxRow.Similarity)
                {
                    //если коэффициент схожести больше заданного порогового значения - добавляем документ в кластер
                    if (entry.Value >= threshold)
                        cluster.Docs.Add(entry.Key);
                }

                //кластер не пустой
                if (cluster.Docs.Count != 0)
                    clusters.Add(cluster);

                //удаляем документы, которые попали в кластер
                DeleteRows(rows, cluster.Docs);

                size = rows.Count();
            }

            return clusters;
        }
        private void DeleteRows(List<SimilarityRow> rows, List<Doc> delDocs)
        {
            //удаляем документы
            //rows.stream()..removeAll(x => delIndexes.Contains(x.DocIndex));
            List<SimilarityRow> remLst = new List<SimilarityRow>();
            foreach (var row in rows)
            {
                if (delDocs.Contains(row.Doc))
                    remLst.Add(row);
            }
            rows.RemoveAll(x => remLst.Contains(x));
            //из оставшихся документов удаляем коэффициенты схожести с уже включенными в кластер документами
            foreach (var row in rows)
                row.RemoveSimilarityByIndex(delDocs);

        }


    }
}