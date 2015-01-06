namespace Application.Infrastructure.DTO
{
    public class UserData
    {
        public int UserId { get; set; }

        public bool IsStudent { get; set; }
        
        public bool IsLecturer { get; set; }
        
        public bool IsSecretary { get; set; }

        public bool HasAssignedDiplomProject { get; set; }
    }
}
