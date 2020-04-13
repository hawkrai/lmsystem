using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMPlatform.Data.Repositories
{
	public class SubjectModuleRepository : RepositoryBase<LmPlatformModelsContext, SubjectModule>
	{
		public SubjectModuleRepository(LmPlatformModelsContext dataContext) : base(dataContext)
		{
		}

		public static int[] GetCMSubjectIds() {

			using (var context = new LmPlatformModelsContext()) 
			{
				return context.SubjectModules.Where(x => x.ModuleId == 14).Select(x => x.SubjectId).ToArray();
			}
		}
	}
}
