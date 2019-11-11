using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.HashFunction;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.HashFunction.CRC;
using LMPlatform.PlagiarismNet.Data;
using LMPlatform.PlagiarismNet.Services.Interfaces;
using LMPlatform.PlagiarismNet.Utils;
using LMPlatform.PlagiarismNet.XMLDocs;

namespace LMPlatform.PlagiarismNet.Services
{
    public class SimilarityByShingle : ISimilarity
    {
        public List<SimilarityRow> MakeSimilarityRows(List<Doc> docs, int shingleLength)
        {

            List<SimilarityRow> rows = new List<SimilarityRow>();

            //получим список шинглов для каждого документа
            Dictionary<string, List<long>> doc2Terms = GetDocumentShingle(docs, shingleLength);
            Dictionary<string, int> doc2similarity = new Dictionary<string, int>();

            for (int i = 0; i < docs.Count(); i++)
            {
                var doc = docs[i];
				var row = new SimilarityRow
				{
					Doc = doc
				};
				//список шинглов для данного документа
				var shingles1 = doc2Terms[row.Doc.DocIndex];

                //сравнение с другими документами
                for (int j = 0; j < docs.Count(); j++)
                {

                    //ид документа
                    var doc2 = docs[j];

	                if (!row.Similarity.ContainsKey(doc2))
	                {
		                //схожесть одинаковых документов можно не считать. всегда = 100
		                if (row.Doc.Equals(doc2))
		                {
			                //сам с собой можно не считать. всегда 100
			                row.Similarity.Add(doc2, 100);
			                continue;
		                }

		                //уже считали раньше
		                if (doc2similarity.ContainsKey(GetUniqueKey(i, j)))
		                {
			                row.Similarity.Add(doc2, doc2similarity[GetUniqueKey(i, j)]);
			                continue;
		                }

		                //набор шинглов для документа, с которым сравниваем
		                var shingles2 = doc2Terms[doc2.DocIndex];
		                var same = 0;
		                for (int k = 0; k < shingles1.Count(); k++)
		                {
			                for (int l = 0; l < shingles2.Count(); l++)
			                {
				                if (shingles1[k].Equals(shingles2[l]))
					                same++;
			                }
		                }

		                double s = same * 2;
		                double count = shingles1.Count() + shingles2.Count();
		                double res = s / count;
		                int coeff = (int) Math.Round(res * 100);
		                //добавляем в матрицу
		                doc2similarity.Add(GetUniqueKey(i, j), coeff);

		                row.Similarity.Add(doc2, coeff);
	                }
                }
                rows.Add(row);
            }

            return rows;
        }
        protected Dictionary<string, List<long>> GetDocumentShingle(List<Doc> docs, int shingleLength)
        {
            Dictionary<string, List<long>> doc2term = new Dictionary<string, List<long>>();

            //List<string> stopList = ClusteringFactory.GetMyStem().Parse(MyStem.MYSTEM_DIR + Path.DirectorySeparatorChar + "stop-list.txt");
            ICRC crc = CRCFactory.Instance.Create();
	        bool endflag = false;

            foreach (var doc in docs)
            {
                string docIndex = doc.DocIndex;
	            //terms.RemoveAll(x => stopList.Contains(x));
				List<string> terms = ClusteringFactory.GetMyStem().Parse(doc.Path);
	            List<long> shingles = new List<long>();
				if (terms.Count != 0)
	            {
		            int startIndex = 0;
		            while (true)
		            {
			            if ((startIndex + shingleLength) > terms.Count)
			            {
				            shingleLength = terms.Count - startIndex;
				            endflag = true;
			            }

			            string shingle = string.Join(" ", terms.GetRange(startIndex, shingleLength));

			            var hashValue = crc.ComputeHash(Encoding.UTF8.GetBytes(shingle));
			            long res = Convert.ToInt64(BitConverter.ToInt32(hashValue.Hash, 0));
			            long shing = Convert.ToInt64(int.MaxValue) + Math.Abs(Convert.ToInt64(int.MinValue + Math.Abs(res)));

			            shingles.Add(shing);
			            if (endflag) break;

			            startIndex++;
		            }
		            
				}

				if (!doc2term.ContainsKey(docIndex))
				{
					doc2term.Add(docIndex, shingles);
				}

			}

            return doc2term;
        }

        private string GetUniqueKey(int i, int j)
        {
            return i > j ? $"key:{j}_{i}" : $"key:{i}_{j}";
        }


    }
}