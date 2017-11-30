using System.Collections.Generic;

namespace LMPlatform.UI.Services.Modules
{
    using System.Runtime.Serialization;

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
	public class ResultPlag
	{
		[DataMember]
		public string doc { get; set; }

		[DataMember]
		public string coeff { get; set; }
	}
}