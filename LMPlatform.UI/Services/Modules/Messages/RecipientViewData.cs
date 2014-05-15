using System.Globalization;

namespace LMPlatform.UI.Services.Modules.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.ServiceModel.Description;

    using Application.Core;
    using Application.Infrastructure.FilesManagement;
    using Application.Infrastructure.MessageManagement;

    using LMPlatform.Models;

    [DataContract]
    public class RecipientViewData
    {
        private readonly LazyDependency<IMessageManagementService> messageManagementService = new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService
        {
            get
            {
                return messageManagementService.Value;
            }
        }

        public RecipientViewData(int id, string name)
        {
            Id = id;
            Name = name;
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}