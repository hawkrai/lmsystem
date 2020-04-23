using System;
using System.IO;
using System.Linq;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Version = Lucene.Net.Util.Version;

namespace Application.SearchEngine.SearchMethods
{
    public abstract class BaseSearchMethod
    {
        private readonly string _luceneDirectory;
        private FSDirectory _directoryTemp;
        protected const int HitsLimits = 1000;

        protected BaseSearchMethod(string luceneDirectory)
        {
            if (!System.IO.Directory.Exists(luceneDirectory))
                System.IO.Directory.CreateDirectory(luceneDirectory);
            _luceneDirectory = luceneDirectory;
        }

        public bool IsIndexExist()
        {
            return Directory.Directory.GetFiles().FirstOrDefault() != null;
        }

        protected FSDirectory Directory
        {
            get
            {
                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDirectory));
                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);
                var lockFilePath = Path.Combine(_luceneDirectory, "write.lock");
                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);
                return _directoryTemp;
            }
        }        

        public void DeleteIndex(int id)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            var searchQuery = new TermQuery(new Term(SearchingFields.Id.ToString(), id.ToString()));
            writer.DeleteDocuments(searchQuery);
            analyzer.Close();
        }

        public bool DeleteIndex()
        {
            try
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);
                using (var writer = new IndexWriter(Directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.DeleteAll();
                    analyzer.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public void Optimize()
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                analyzer.Close();
                writer.Optimize();
            }
        }

        protected Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }
            return query;
        }
    }

    enum SearchingFields
    {
        Id,
        FirstName,
        MiddleName,
        LastName,
        Group,
        Name
    }
}
