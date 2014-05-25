using System;
using System.Collections.Generic;
using System.Linq;
using Application.Core;
using Application.Infrastructure.DTO;
using LMPlatform.Data.Infrastructure;

namespace Application.Infrastructure.DPManagement
{
    public class CorrelationService : ICorrelationService
    {
        public CorrelationService()
        {
            _correlationMethodsMapping = new Dictionary<string, Func<int?, List<Correlation>>>
            {
                { "Group", GetGroupsCorrelation },
                { "DiplomProject", GetDiplomProjectCorrelation }
            };
        }

        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        private readonly Dictionary<string, Func<int?, List<Correlation>>> _correlationMethodsMapping;

        public List<Correlation> GetCorrelation(string entity, int? id)
        {
            if (!_correlationMethodsMapping.ContainsKey(entity))
            {
                throw new Exception("CorrelationService doesn't serve this entity type!");
            }

            return _correlationMethodsMapping[entity](id);
        }

        private List<Correlation> GetGroupsCorrelation(int? id)
        {
            return Context.Groups.Select(x => new Correlation
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }

        private List<Correlation> GetDiplomProjectCorrelation(int? lecturerId)
        {
            var projects = lecturerId.HasValue ? Context.DiplomProjects.Where(x => x.LecturerId == lecturerId) : Context.DiplomProjects;
            return projects.Select(x => new Correlation
                {
                    Id = x.DiplomProjectId,
                    Name = x.Theme
                }).ToList();
        }
    }
}