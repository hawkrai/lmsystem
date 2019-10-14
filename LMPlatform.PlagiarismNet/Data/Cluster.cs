using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using LMPlatform.PlagiarismNet.XMLDocs;

namespace LMPlatform.PlagiarismNet.Data
{
    public class Cluster
    {
        public  List<Doc> Docs { get; set; }
        public string IdTutor { get; set; }

        public Cluster()
        {
            Docs = new List<Doc>();
        }

        public string GetUniqueKey()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var doc in Docs)
            {
                sb.Append(doc.Path);
            }
            
            try
            {
                var md5 = MD5.Create();
                var data = Encoding.UTF8.GetBytes(sb.ToString());
                var byteData = md5.ComputeHash(data);
                var hash = new StringBuilder();
                foreach (var t in byteData)
                {
                    hash.Append(Convert.ToString((t & 0xFF) + 256, 16).Substring(1));
                }
                return hash.ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }
        
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var doc in Docs)
            {
                sb.Append(doc.DocIndex).Append(" ");
            }
            return sb.ToString().Trim();
        }
    }
}