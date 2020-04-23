using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Materials
{
	[DataContract]
    public class DocumentsViewData
    {
        private List<Models.Materials> dc;

        public DocumentsViewData(Models.Materials materials)
        {
            Id = materials.Id;
            Name = materials.Name;
            Text = materials.Text;
        }

        public DocumentsViewData(List<Models.Materials> dc)
        {
            // TODO: Complete member initialization
            this.dc = dc;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Text { get; set; }
    }
}