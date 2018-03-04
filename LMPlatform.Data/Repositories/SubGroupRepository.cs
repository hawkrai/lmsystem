using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Application.Core.Data;
using LMPlatform.Data.Infrastructure;
using LMPlatform.Data.Repositories.RepositoryContracts;
using LMPlatform.Models;

namespace LMPlatform.Data.Repositories
{
    public class SubGroupRepository : RepositoryBase<LmPlatformModelsContext, SubGroup>, ISubGroupRepository
    {
        public SubGroupRepository(LmPlatformModelsContext dataContext)
            : base(dataContext)
        {
        }

		public void SaveStudents(int subjectId, int subjectGroupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts)
        {
            try
            {
                var modelFirstSubGroup =
                        GetBy(
                            new Query<SubGroup>(e => e.SubjectGroupId == subjectGroupId && e.Name == "first").Include(
                                e => e.SubjectStudents.Select(x => x.Student)));
                var modelSecondSubGroup =
                    GetBy(
                        new Query<SubGroup>(e => e.SubjectGroupId == subjectGroupId && e.Name == "second").Include(
                            e => e.SubjectStudents.Select(x => x.Student)));

				var modelThirdSubGroup =
					GetBy(
						new Query<SubGroup>(e => e.SubjectGroupId == subjectGroupId && e.Name == "third").Include(
							e => e.SubjectStudents.Select(x => x.Student)));

                var firstSubGorupStudent = modelFirstSubGroup.SubjectStudents.ToList();
                var secondSubGorupStudent = modelSecondSubGroup.SubjectStudents.ToList();
				var thirdSubGorupStudent = modelThirdSubGroup.SubjectStudents.ToList();

                using (var context = new LmPlatformModelsContext())
                {
                    foreach (var subjectStudent in firstSubGorupStudent)
                    {
                        if (!firstInts.Any(e => e == subjectStudent.StudentId))
                        {
                            context.Set<SubjectStudent>().Remove(context.Set<SubjectStudent>().FirstOrDefault(e => e.Id == subjectStudent.Id));
                        }
                    }

                    foreach (var subjectStudent in secondSubGorupStudent)
                    {
                        if (!secoInts.Any(e => e == subjectStudent.StudentId))
                        {
                            context.Set<SubjectStudent>().Remove(context.Set<SubjectStudent>().FirstOrDefault(e => e.Id == subjectStudent.Id));
                        }
                    }

					foreach (var subjectStudent in thirdSubGorupStudent)
					{
						if (!thirdInts.Any(e => e == subjectStudent.StudentId))
						{
							context.Set<SubjectStudent>().Remove(context.Set<SubjectStudent>().FirstOrDefault(e => e.Id == subjectStudent.Id));
						}
					}

                    context.SaveChanges();

                    foreach (var student in firstInts)
                    {
                        if (!firstSubGorupStudent.Any(e => e.StudentId == student))
                        {
                            context.Set<SubjectStudent>().Add(new SubjectStudent
                            {
                                StudentId = student,
                                SubGroupId = modelFirstSubGroup.Id,
                                SubjectGroupId = subjectGroupId
                            });
                        }
                    }

                    foreach (var student in secoInts)
                    {
                        if (!secondSubGorupStudent.Any(e => e.StudentId == student))
                        {
                            context.Set<SubjectStudent>().Add(new SubjectStudent
                            {
                                StudentId = student,
								SubGroupId = modelSecondSubGroup.Id,
                                SubjectGroupId = subjectGroupId
                            });
                        }
                    }

					foreach (var student in thirdInts)
					{
						if (!thirdSubGorupStudent.Any(e => e.StudentId == student))
						{
							context.Set<SubjectStudent>().Add(new SubjectStudent
							{
								StudentId = student,
								SubGroupId = modelThirdSubGroup.Id,
								SubjectGroupId = subjectGroupId
							});
						}
					}

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

		public void CreateSubGroup(int subjectId, int subjectGroupId, IList<int> firstInts, IList<int> secoInts, IList<int> thirdInts)
        {
            var modelFirstSubGroup = new SubGroup
            {
                Name = "first",
                SubjectGroupId = subjectGroupId
            };

            var modelSecondSubGroup = new SubGroup
            {
                Name = "second",
                SubjectGroupId = subjectGroupId
            };

			var modelThirdSubGroup = new SubGroup
			{
				Name = "third",
				SubjectGroupId = subjectGroupId
			};

            using (var context = new LmPlatformModelsContext())
            {
                Save(modelFirstSubGroup);
                Save(modelSecondSubGroup);
				Save(modelThirdSubGroup);

                DataContext.SaveChanges();

                foreach (var firstInt in firstInts)
                {
                    context.Set<SubjectStudent>().Add(new SubjectStudent
                    {
                        StudentId = firstInt,
                        SubGroupId = modelFirstSubGroup.Id,
                        SubjectGroupId = subjectGroupId
                    });
                }

                foreach (var secoInt in secoInts)
                {
                    context.Set<SubjectStudent>().Add(new SubjectStudent
                    {
                        StudentId = secoInt,
						SubGroupId = modelSecondSubGroup.Id,
                        SubjectGroupId = subjectGroupId
                    });
                }

				foreach (var third in thirdInts)
				{
					context.Set<SubjectStudent>().Add(new SubjectStudent
					{
						StudentId = third,
						SubGroupId = modelThirdSubGroup.Id,
						SubjectGroupId = subjectGroupId
					});
				}

                context.SaveChanges();
            }
        }
    }
}