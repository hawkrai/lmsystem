using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using LMPlatform.Models;

namespace LMPlatform.UI.Services.Modules.Concept
{
    [DataContract]
    public class ConceptViewData
    {
        public ConceptViewData(LMPlatform.Models.Concept concept)
        {
            Id = concept.Id;
            Name = concept.Name;
            ShortName = concept.Name.Length <= 20 ? concept.Name : String.Format("{0}...", concept.Name.Substring(0, 20));
            Container = concept.Container;
            ParentId = concept.ParentId.GetValueOrDefault();
            IsGroup = concept.IsGroup;
            Published = (concept.IsGroup && concept.Children.Any() && concept.Children.All(c => c.Published)) || (!concept.IsGroup && concept.Published);
            ReadOnly = concept.ReadOnly;
            HasData = !String.IsNullOrEmpty(Container);
            Prev = concept.PrevConcept;
            Next = concept.NextConcept;
        }

        public ConceptViewData(LMPlatform.Models.Concept concept, Boolean buildTree)
            : this(concept)
        {
            if (buildTree)
            {
                Children = new List<ConceptViewData>();
                InitTree(concept, concept.Children);
            }
        }

        private void InitTree(LMPlatform.Models.Concept concept, ICollection<LMPlatform.Models.Concept> ch)
        {
            if (ch != null && ch.Any())
                Children = ch.Select(c => new ConceptViewData(c, true)).ToList();
            else
                return;
        }

        [DataMember]
        public Int32 Id
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string ShortName
        {
            get;
            set;
        }


        [DataMember]
        public string Container
        {
            get;
            set;
        }

        [DataMember]
        public int ParentId { get; set; }

        [DataMember]
        public Boolean IsGroup { get; set; }

        [DataMember]
        public Boolean Published { get; set; }

        [DataMember]
        public Boolean ReadOnly { get; set; }

        [DataMember]
        public Boolean HasData { get; set; }

        [DataMember]
        public Int32? Next { get; set; }
        [DataMember]
        public Int32? Prev { get; set; }


        [DataMember(Name = "children")]
        public ICollection<ConceptViewData> Children 
        { 
            get
            {
                return _childrens.SortDoubleLinkedList();
            }
            set
            {
                _childrens = value;
            }
        }

        private ICollection<ConceptViewData> _childrens;
    }

    [DataContract]
    public class AttachViewData
    {
        private String GetFilePath()
        {
            var uploadFolder = "UploadedFiles";
            return String.Format("/{0}/{1}/{2}", uploadFolder, FilePath, FileName);
        }


        public AttachViewData(Int32 id, String name, Attachment att)
        {
            Id = id;
            Name = name;
            HasData = id>0;
            if (att != null)
            {
                FilePath = att.PathName;
                FileName = att.FileName;
                FullPath = GetFilePath();
                HasAttach = true;
            }
        }

        [DataMember]
        public Int32 Id { get; set; }

        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String FilePath { get; set; }

        [DataMember]
        public String FileName { get; set; }

        [DataMember]
        public String FullPath { get; set; }

        [DataMember]
        public Boolean HasData { get; set; }

        [DataMember]
        public Boolean HasAttach { get; set; }
    }

    public static class ConceptViewDataExtension
    {
        public static List<ConceptViewData> SortDoubleLinkedList(this IEnumerable<ConceptViewData> source)
        {
            var checkCount = 0;
            var res = new List<ConceptViewData>();
            if (source == null)
                return res;
            var first = source.FirstOrDefault(s => s.Prev == null);
            if (first == null)
            {
                res.AddRange(source);
                return res;
            }
            res.Add(first);
            var next = source.FirstOrDefault(s => s.Id == first.Next.GetValueOrDefault());
            if (next == null)
                return res;
            else
            {
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
            }

            return res;
        }
    }
}