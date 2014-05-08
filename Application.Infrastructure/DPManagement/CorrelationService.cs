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
            _correlationMethodsMapping = new Dictionary<string, Func<List<Correlation>>>
            {
                { "Group", GetGroupsCorrelation }
            };
        }

        private readonly LazyDependency<IDpContext> context = new LazyDependency<IDpContext>();

        private IDpContext Context
        {
            get { return context.Value; }
        }

        private readonly Dictionary<string, Func<List<Correlation>>> _correlationMethodsMapping;
        
        public List<Correlation> GetCorrelation(string entity)
        {
            if (!_correlationMethodsMapping.ContainsKey(entity))
            {
                throw new Exception("CorrelationService doesn't serve this entity type!");
            }

            return _correlationMethodsMapping[entity]();
        }

        private List<Correlation> GetGroupsCorrelation()
        {
            return Context.Groups.Select(x => new Correlation
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();
        }
    }
}