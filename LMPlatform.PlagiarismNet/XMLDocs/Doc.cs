using System;

namespace LMPlatform.PlagiarismNet.XMLDocs
{
    public class Doc
    {
        public Doc() { }

        public string Path { get; set; }

        public string FileName { get; set; }

        public string DocIndex { get; set; }

        public override bool Equals(Object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (GetType() != obj.GetType())) return false;
            Doc doc = (Doc)obj;
            return DocIndex == null || (DocIndex?.Equals(doc.DocIndex) ?? false);
        }

        public override int GetHashCode()
        {
            int result = DocIndex != null ? DocIndex.GetHashCode() : 0;
            return result;
        }
    }
}