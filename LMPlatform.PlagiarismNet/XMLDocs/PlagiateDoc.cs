namespace LMPlatform.PlagiarismNet.XMLDocs
{
	public class PlagiateDoc
	{
		public PlagiateDoc(string doc, int coeff)
		{
			Doc = doc;
			Coeff = coeff;
		}

		public string Doc { get; set; }
		
		public int Coeff { get; set;}

		
	}
}