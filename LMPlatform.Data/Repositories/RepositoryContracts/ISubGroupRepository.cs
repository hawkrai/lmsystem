using System.Collections.Generic;
using Application.Core.Data;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories.RepositoryContracts
{
	public interface ISubGroupRepository : IRepositoryBase<SubGroup>
	{
		void SaveStudents(int subjectId, int subjectGroupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts);

		void CreateSubGroup(int subjectId, int subjectGroupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts);
	}
}