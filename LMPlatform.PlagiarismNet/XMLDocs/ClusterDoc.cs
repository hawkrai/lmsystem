using System.Collections.Generic;
using LMPlatform.PlagiarismNet.Data;

namespace LMPlatform.PlagiarismNet.XMLDocs
{
	public class ClusterDoc
	{
		public ClusterDoc()
		{
		}

		public ClusterDoc(Cluster cluster)
		{
			UniqueKey = cluster.GetUniqueKey();
			Docs = new List<string>();
			cluster.Docs.ForEach((doc)=> {
				Docs.Add(doc.Path);
			});
			
		}

		public List<string> Docs { get; set; }
		public string UniqueKey { get; set; }

	}
}