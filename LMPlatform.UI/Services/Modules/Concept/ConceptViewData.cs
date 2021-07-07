using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Concept
{
    [DataContract]
    public class ConceptViewData
    {
        public ConceptViewData(Models.Concept concept)
        {
            Id = concept.Id;
            Name = concept.Name;
            ShortName = concept.Name.Length <= 20 ? concept.Name : $"{concept.Name.Substring(0, 20)}...";
            Container = concept.Container;
            ParentId = concept.ParentId.GetValueOrDefault();
            IsGroup = concept.IsGroup;
            Published = concept.Published;
            //Published = (concept.IsGroup && concept.Children.Any() && concept.Children.All(c => c.Published)) || (!concept.IsGroup && concept.Published);
            ReadOnly = concept.ReadOnly;
            HasData = !string.IsNullOrEmpty(Container);
            Prev = concept.PrevConcept;
            Next = concept.NextConcept;
            SubjectName = concept.Subject.Name;
        }

        public ConceptViewData(Models.Concept concept, bool buildTree)
            : this(concept)
        {
	        if (!buildTree) return;
	        Children = new List<ConceptViewData>();
            InitTree(concept.Children);
        }

        public ConceptViewData(Models.Concept concept, bool buildTree, Func<Models.Concept, bool> filterFirstLevelChildren)
            : this(concept)
        {
	        if (!buildTree) return;
	        Children = new List<ConceptViewData>();
            if (filterFirstLevelChildren == null)
            {
	            InitTree(concept.Children);
            }
            else
            {
	            InitTree(concept.Children.Where(filterFirstLevelChildren).ToList());
            }
        }

        private void InitTree(ICollection<Models.Concept> ch)
        {
	        if (ch != null && ch.Any())
	        {
                Children = ch.Select(c => new ConceptViewData(c, true)).ToList();
	        }
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ShortName { get; set; }

        [DataMember]
        public string SubjectName { get; set; }

        [DataMember]
        public string Container { get; set; }

        [DataMember]
        public string FilePath { get; set; }

		[DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public bool IsGroup { get; set; }

        [DataMember]
        public bool Published { get; set; }

        [DataMember]
        public bool ReadOnly { get; set; }

        [DataMember]
        public bool HasData { get; set; }

        [DataMember]
        public int? Next { get; set; }
        [DataMember]
        public int? Prev { get; set; }
        
        [DataMember(Name = "children")]
        public ICollection<ConceptViewData> Children 
        { 
            get => _childrens.SortDoubleLinkedList();
            set => _childrens = value;
        }

        private ICollection<ConceptViewData> _childrens;
    }

    [DataContract]
    public class AttachViewData
    {
        private string GetFilePath()
        {
            var uploadFolder = "UploadedFiles";
            return $"/{uploadFolder}/{FilePath}/{FileName}";
        }


        public AttachViewData(int id, string name, Attachment att)
        {
            Id = id;
            Name = name;
            HasData = id>0;
            if (att == null) return;
            FilePath = att.PathName;
            FileName = att.FileName;
            FullPath = GetFilePath();
            HasAttach = true;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FilePath { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string FullPath { get; set; }

        [DataMember]
        public bool HasData { get; set; }

        [DataMember]
        public bool HasAttach { get; set; }
    }

    public static class ConceptViewDataExtension
    {
        public static List<ConceptViewData> SortDoubleLinkedList(this IEnumerable<ConceptViewData> source)
        {
            var checkCount = 0;
            var res = new List<ConceptViewData>();
            if (source == null)
            {
	            return res;
            }

            var first = source.FirstOrDefault(s => s.Prev == null);
            if (first == null)
            {
                res.AddRange(source);
                return res;
            }
            res.Add(first);
            var next = source.FirstOrDefault(s => s.Id == first.Next.GetValueOrDefault(-1));
            if (next == null)
            {
                res.AddRange(source.Where(i=> res.All(r => r.Id != i.Id)));
                return res;
            }

            res.Add(next);
            first = next;
            while (next != null && checkCount < 1000)
            {
	            next = source.FirstOrDefault(s => s.Id == first.Next.GetValueOrDefault());
	            if (next != null)
		            res.Add(next);
	            first = next;
	            checkCount++;
            }

            return res;
        }
    }
}