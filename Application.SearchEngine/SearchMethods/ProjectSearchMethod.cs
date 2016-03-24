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
    public class ProjectSearchMethod : BaseSearchMethod
    {
        private static readonly string LuceneDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "Lucene_indeces", "Project_indeces");

        public ProjectSearchMethod(): base(LuceneDirectory){}

        private void AddToIndex(Project project, IndexWriter writer)
        {
            var searchQuery = new TermQuery(new Term(SearchingFields.Id.ToString(), project.Id.ToString()));
            writer.DeleteDocuments(searchQuery);

            var doc = new Document();

            doc.Add(new Field(SearchingFields.Id.ToString(), project.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(SearchingFields.Name.ToString(), project.Title, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
        }

        public void UpdateIndex(Project project)
        {
            DeleteIndex(project.Id);
            AddToIndex(project);
        }

        public void AddToIndex(IEnumerable<Project> projects)
        {
            var analyzer = new StandardAnalyzer(Version.LUCENE_30);
            using (var writer = new IndexWriter(Directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
            {
                foreach (var group in projects)
                    AddToIndex(group, writer);
                analyzer.Close();
            }
        }

        public void AddToIndex(Project project)
        {
            AddToIndex(new List<Project> { project });
        }

        private Project MapSearchResult(Document doc)
        {
            return new Project
            {
                Id = int.Parse(doc.Get(SearchingFields.Id.ToString())),
                Title = doc.Get(SearchingFields.Name.ToString())
            };
        }

        private IEnumerable<Project> MapSearchResults(IEnumerable<Document> documents)
        {
            return documents.Select(MapSearchResult).ToList();
        }

        public IEnumerable<Project> MapSearchResults(IEnumerable<ScoreDoc> scoreDocs, IndexSearcher searcher)
        {
            return scoreDocs.Select(scoreDoc => MapSearchResult(searcher.Doc(scoreDoc.Doc))).ToList();
        }

        private IEnumerable<Project> search(string searchQuery, string searchField = "")
        {
            if (string.IsNullOrEmpty(searchQuery.Replace("*", "").Replace("?", "")))
                return new List<Project>();

            using (var searcher = new IndexSearcher(Directory, false))
            {
                var analyzer = new StandardAnalyzer(Version.LUCENE_30);

                var parser = new MultiFieldQueryParser(Version.LUCENE_30, new[]
                {
                    SearchingFields.Id.ToString(),
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

        public IEnumerable<Project> Search(string searchText, string fieldName = "")
        {
            if (string.IsNullOrEmpty(searchText)) return new List<Project>();

            var terms = searchText.Trim().Replace("-", " ").Split(' ')
                .Where(term => !string.IsNullOrEmpty(term)).Select(term => term.Trim() + "*");
            searchText = string.Join(" ", terms);

            return search(searchText, fieldName);
        }
    }
}
