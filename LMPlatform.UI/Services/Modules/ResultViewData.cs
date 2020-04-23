using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules
{
	[DataContract]
	public class ResultViewData
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string Code { get; set; }

		[DataMember]
		public List<ResultPlag> DataD { get; set; }
	}

	[DataContract]
	public class ResultPSubjectViewData
	{
		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string Code { get; set; }

		[DataMember]
		public List<ResultPlagSubject> DataD { get; set; }
	}

	[DataContract]
	public class ResultPlag
	{
		[DataMember]
		public string doc { get; set; }

		[DataMember]
		public string coeff { get; set; }

		[DataMember]
		public string author { get; set; }

		[DataMember]
		public string subjectName { get; set; }

		[DataMember]
		public string groupName { get; set; }

		[DataMember]
		public string DocFileName { get; set; }

		[DataMember]
		public string DocPathName { get; set; }
		
	}

	[DataContract]
	public class ResultPlagSubjectClu
	{
		[DataMember]
		public ResultPlagSubject [] clusters { get; set; }
	}

	[DataContract]
	public class ResultPlagSubject
	{
		[DataMember]
		public string [] docs { get; set; }

		[DataMember]
		public List<ResultPlag> correctDocs { get; set; }
	}
}
