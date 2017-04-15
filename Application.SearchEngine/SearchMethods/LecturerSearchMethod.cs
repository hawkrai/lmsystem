using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LMPlatform.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Version = Lucene.Net.Util.Version;

namespace Application.SearchEngine.SearchMethods
{
    public class LecturerSearchMethod : BaseSearchMethod
    {
        private static readonly string LuceneDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Lucene_indeces", "Lecturer_indeces");

        public LecturerSearchMethod() : base(LuceneDirectory) {}

        private void AddToIndex(Lecturer lecturer, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term(SearchingFields.Id.ToString(), lecturer.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            var doc = new Document();

            doc.Add(new Field(SearchingFields.Id.ToString(), lecturer.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field(SearchingFields.FirstName.ToString(), lecturer.FirstName ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchingFields.MiddleName.ToString(), lecturer.MiddleName ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field(SearchingFields.LastName.ToString(), lecturer.LastName ?? string.Empty, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchingFields.Name.ToString(), lecturer.User != null ? lecturer.User.UserName : "", Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public void UpdateIndex(Lecturer lecturer)
        {
            DeleteIndex(lecturer.Id);
            AddToIndex(lecturer);
        }

        public void AddToIndex(IEnumerable<Lecturer> lecturer)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var student in lecturer)
                    AddToIndex(student, writer);
                analyzer.Close();
            }
        }

        public void AddToIndex(Lecturer lecturer)
        {
            AddToIndex(new List<Lecturer> { lecturer });
        }

        private Lecturer MapSearchResult(Document doc)
        {
            return new Lecturer
            {
                Id = int.Parse(doc.Get(SearchingFields.Id.ToString())),
                FirstName = doc.Get(SearchingFields.FirstName.ToString()),
                MiddleName = doc.Get(SearchingFields.MiddleName.ToString()),
                LastName = doc.Get(SearchingFields.LastName.ToString()),
                Skill = doc.Get(SearchingFields.Name.ToString()) //логин
            };
        }

        private IEnumerable<Lecturer> MapSearchResults(IEnumerable<Document> documents)
        {
            return documents.Select(MapSearchResult).ToList();
        }

        public IEnumerable<Lecturer> MapSearchResults(IEnumerable<ScoreDoc> scoreDocs, IndexSearcher searcher)
        {
            return scoreDocs.Select(scoreDoc => MapSearchResult(searcher.Doc(scoreDoc.Doc))).ToList();
        }

        private IEnumerable<Lecturer> search(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<Lecturer>();

            using (var searcher = new IndexSearcher(Directory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[]
                {
                    SearchingFields.Id.ToString(),
                    SearchingFields.FirstName.ToString(),
                    SearchingFields.MiddleName.ToString(),
                    SearchingFields.LastName.ToString(),
                    SearchingFields.Group.ToString(),
                    SearchingFields.Name.ToString()
                }, analyzer);

                var query = ParseQuery(searchQuery, parser);
                var docs = searcher.Search(query, null, HitsLimits, Sort.RELEVANCE).ScoreDocs;
                var results = MapSearchResults(docs, searcher);
                analyzer.Close();
                searcher.Dispose();

                return results;
            }
        }

        public IEnumerable<Lecturer> Search(string searchText, string fieldName = "")
        {
            if (string.IsNullOrEmpty(searchText)) return new List<Lecturer>();

            var terms = searchText.Trim().Replace("-", " ").Split(' ')
                .Where(term => !string.IsNullOrEmpty(term)).Select(term => term.Trim() + "*");
            searchText = string.Join(" ", terms);

            return search(searchText, fieldName);
        }    
    }
}
