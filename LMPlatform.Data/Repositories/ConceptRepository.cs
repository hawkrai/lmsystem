using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace LMPlatform.Data.Repositories
{
    public class ConceptRepository : RepositoryBase<LmPlatformModelsContext, Concept>, IConceptRepository
    {
        public ConceptRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

        public Concept GetTreeConceptByElementId(int id)
        {
	        using var context = new LmPlatformModelsContext();
	        var concept = context.Set<Concept>().Include(c=>c.Author).Include(c=>c.Subject).Include(c => c.Children).FirstOrDefault(e => e.Id == id);
	        InitTreeConceptByParentIdInner(concept);
	        return concept;
        }

        private void InitTreeConceptByParentIdInner(Concept parent)
        {
	        if (!parent.Children.Any()) return;
	        foreach (var child in parent.Children)
	        {
		        InitTreeConceptByParentIdInner(child);
	        }
        }

        public Concept GetById(int id)
        {
	        using var context = new LmPlatformModelsContext();
	        var concept = context.Set<Concept>()
		        .Include(c => c.Children)
		        .Include(c=>c.Author)
		        .Include(c=>c.Subject)
		        .FirstOrDefault(e => e.Id == id);
	        return concept;
        }

        public IEnumerable<Concept> GetRootElementsByAuthorId(int authorId)
        {
	        using var context = new LmPlatformModelsContext();
	        var concepts = context.Set<Concept>()
		        .Include(c => c.Children)
		        .Include(a => a.Author)
		        .Include(c => c.Subject)
		        .Include(c => c.Subject.SubjectModules)
		        .Where(e => e.Author.Id == authorId && !e.ParentId.HasValue && e.IsGroup);
	        return concepts.ToList();
        }


        public IEnumerable<Concept> GetByParentId(int parentId)
        {
	        using var context = new LmPlatformModelsContext();
	        var concepts = context.Set<Concept>().Include(c=>c.Children).Include(s=>s.Subject).Where(e => e.ParentId == parentId);
	        return concepts.ToList();
        }

        public IEnumerable<Concept> GetBySubjectId(int subjectId)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var concepts = context.Set<Concept>().Include(c => c.Children).Include(s => s.Subject).Where(e => e.Subject.Id == subjectId);
                return concepts.ToList();
            }
        }

        public IEnumerable<Concept> GetByAuthorId(int authorId)
        {
            using (var context = new LmPlatformModelsContext())
            {
                var concepts = context.Set<Concept>().Include(c => c.Children).Include(s => s.Author).Include(s => s.Subject).Where(e => e.Author.Id == authorId);
                return concepts.ToList();
            }
        }

        public void Remove(int id, bool removeChildren)
        {
	        var concept = removeChildren ? GetTreeConceptByElementId(id) : GetBy(new Query<Concept>(c => c.Id == id));
	        if (removeChildren)
	        {
		        RemoveChildren(concept);
	        }
	        else
	        {
		        Delete(concept);
	        }
        }

        private void RemoveChildren(Concept parent)
        {
            Delete(parent);
            if (parent.Children == null || !parent.Children.Any()) return;
            foreach (var c in parent.Children)
            {
	            RemoveChildren(c);
            }
        }

        public IEnumerable<Concept> GetRootElementsBySubjectId(int subjectId)
        {
	        using var context = new LmPlatformModelsContext();
	        var concepts = context.Set<Concept>().Include(c => c.Children).Include(a => a.Subject)
		        .Where(e => e.Subject.Id == subjectId && !e.ParentId.HasValue && e.IsGroup);
	        return concepts.ToList();
        }
    }
}
