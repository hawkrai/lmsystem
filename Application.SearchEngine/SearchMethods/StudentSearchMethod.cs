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
    public class StudentSearchMethod : BaseSearchMethod
    {
        private static readonly string LuceneDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Lucene_indeces", "Student_indeces");                

        public StudentSearchMethod(): base(LuceneDirectory){}        

        private void AddToIndex(Student student, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term(SearchingFields.Id.ToString(), student.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            var doc = new Document();
            
            doc.Add(new Field(SearchingFields.Id.ToString(), student.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
			doc.Add(new Field(SearchingFields.FirstName.ToString(), student.FirstName ?? "", Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field(SearchingFields.MiddleName.ToString(), student.MiddleName ?? "", Field.Store.YES, Field.Index.ANALYZED));
			doc.Add(new Field(SearchingFields.LastName.ToString(), student.LastName ?? "", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchingFields.Group.ToString(), student.Group != null ? student.Group.Name : "", Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(SearchingFields.Name.ToString(), student.User != null ? student.User.UserName : "", Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public void UpdateIndex(Student student)
        {
            DeleteIndex(student.Id);
            AddToIndex(student);
        }

        public void AddToIndex(IEnumerable<Student> students)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
            foreach (var student in students)
            {
	            AddToIndex(student, writer);
            }

            analyzer.Close();
        }

        public void AddToIndex(Student student)
        {
            AddToIndex(new List<Student> {student});
        }

        private Student MapSearchResult(Document doc)
        {
            int groupId;
            int.TryParse(doc.Get(SearchingFields.Group.ToString()), out groupId);
            return new Student
            {
                Id = int.Parse(doc.Get(SearchingFields.Id.ToString())),
                FirstName = doc.Get(SearchingFields.FirstName.ToString()),
                MiddleName = doc.Get(SearchingFields.MiddleName.ToString()),
                LastName = doc.Get(SearchingFields.LastName.ToString()),
                Email = doc.Get(SearchingFields.Name.ToString()), //логин
                GroupId = groupId
            };
        }

        private IEnumerable<Student> MapSearchResults(IEnumerable<Document> documents)
        {
            return documents.Select(MapSearchResult).ToList();
        }

        private IEnumerable<Student> MapSearchResults(IEnumerable<ScoreDoc> scoreDocs, IndexSearcher searcher)
        {
            return scoreDocs.Select(scoreDoc => MapSearchResult(searcher.Doc(scoreDoc.Doc))).ToList();
        }

        private IEnumerable<Student> search(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", ""))) 
                return new List<Student>();            

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

        public IEnumerable<Student> Search(string searchText, string fieldName = "")
        {
            if (string.IsNullOrEmpty(searchText)) return new List<Student>();

            var terms = searchText.Trim().Replace("-", " ").Split(' ')
                .Where(term => !string.IsNullOrEmpty(term)).Select(term => term.Trim() + "*");
            searchText = string.Join(" ", terms);

            return search(searchText, fieldName);
        }        
    }    
}
