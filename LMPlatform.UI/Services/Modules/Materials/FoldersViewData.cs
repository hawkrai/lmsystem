using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LMPlatform.UI.Services.Modules.Materials
{
    using Models;
 
    [DataContract]
    public class FoldersViewData
    {
        private List<Folders> fl;

        public FoldersViewData(Folders folders)
        {
            Id = folders.Id;
            Name = folders.Name;
            Pid = folders.Pid;
        }

        public FoldersViewData(List<Folders> fl)
        {
            // TODO: Complete member initialization
            this.fl = fl;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Pid { get; set; }
    }
}