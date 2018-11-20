using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LMPlatform.Models;
using LMPlatform.Models.DP;

namespace LMPlatform.Data.Infrastructure
{
    public interface IDpContext
    {
        int SaveChanges();

        DbSet<Role> Roles { get; set; }

        DbSet<ScoObjects> ScoObjects { get; set; }

        DbSet<TinCanObjects> TinCanObjects { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<Student> Students { get; set; }

        DbSet<Group> Groups { get; set; }

        DbSet<Lecturer> Lecturers { get; set; }

        DbSet<AssignedDiplomProject> AssignedDiplomProjects { get; set; }

        DbSet<DiplomPercentagesGraph> DiplomPercentagesGraphs { get; set; }

        DbSet<DiplomPercentagesGraphToGroup> DiplomPercentagesGraphToGroup { get; set; }

        DbSet<DiplomPercentagesResult> DiplomPercentagesResults { get; set; }

        DbSet<DiplomProjectConsultationDate> DiplomProjectConsultationDates { get; set; }

        DbSet<DiplomProjectConsultationMark> DiplomProjectConsultationMarks { get; set; }

        DbSet<DiplomProjectGroup> DiplomProjectGroups { get; set; }

        DbSet<DiplomProject> DiplomProjects { get; set; }

        DbSet<DiplomProjectTaskSheetTemplate> DiplomProjectTaskSheetTemplates { get; set; }
        Expression<Func<Student, bool>> StudentIsGraduate { get; }

        IQueryable<Student> GetGraduateStudents();

        IQueryable<Group> GetGraduateGroups();

        DbSet<DiplomProjectNews> DiplomProjectNewses { get; set; }
    }
}