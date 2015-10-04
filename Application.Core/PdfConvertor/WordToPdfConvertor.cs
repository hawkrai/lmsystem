using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Doc;
using Spire.Doc.Documents;
using System.IO;
using System.Configuration;


namespace Application.Core.PdfConvertor
{
    public class WordToPdfConvertor
    {
        private readonly string _storageRootTemp = ConfigurationManager.AppSettings["FileUploadPathTemp"];

        public String Convert(String sourceFile)
        {
            Document document = new Document();
            document.LoadFromFile(sourceFile);

            var fileName = String.Format("{0}.pdf",Path.GetFileNameWithoutExtension(sourceFile));
            var fullPath = String.Format("{0}{1}", _storageRootTemp, fileName);

            document.SaveToFile(fullPath, FileFormat.PDF);

            return fileName;
        }
    }
}
