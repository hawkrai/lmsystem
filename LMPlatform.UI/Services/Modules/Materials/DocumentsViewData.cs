namespace LMPlatform.UI.Services.Modules.Materials
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using LMPlatform.Models;
 
    [DataContract]
    public class DocumentsViewData
    {
        private List<Materials> dc;

        public DocumentsViewData(Materials materials)
        {
            Id = materials.Id;
            Name = materials.Name;
            Text = materials.Text;
        }

        public DocumentsViewData(List<Materials> dc)
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