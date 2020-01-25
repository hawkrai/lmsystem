namespace LMPlatform.UI.MappingModels
{
	public class StudentSimpleModel
	{
		public int Id { get; set; }

		public virtual bool IsNew => Id == 0;

		public string Email { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string MiddleName { get; set; }

		public bool? Confirmed { get; set; }
		
		public int GroupId { get; set; }

		public string FullName => $"{LastName} {FirstName} {MiddleName}";
	}
}