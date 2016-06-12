using Application.Core;
using Application.Infrastructure.CTO;
using Application.Infrastructure.CPManagement;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LMPlatform.UI.ApiControllers.CP
{
    public class CourseProjectSubjectController : ApiController
    {
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly LazyDependency<ICPManagementService> _cpManagementService = new LazyDependency<ICPManagementService>();

        private ICPManagementService CPManagementService
        {
            get { return _cpManagementService.Value; }
        }

        public SubjectData Get(int id)
        {
            SubjectData sub = new SubjectData();
            sub = CPManagementService.GetSubject(id);
            return sub;
        }

     
    }
}
