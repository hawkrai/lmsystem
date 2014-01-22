using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Core;
using Application.Infrastructure.MessageManagement;
using Application.Infrastructure.UserManagement;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels
{
    public class MessageViewModel
    {
        private readonly LazyDependency<IMessageManagementService> _messageManagementService = new LazyDependency<IMessageManagementService>();

        public IMessageManagementService MessageManagementService
        {
            get
            {
                return _messageManagementService.Value;
            }
        }

        [HiddenInput(DisplayValue = false)]
        public int FromId { get; set; }

        [Display(Name = "Кому")]
        public int ToId { get; set; }

        [Display(Name = "Сообщение")]
        [DataType(DataType.MultilineText)]
        public string MessageText { get; set; }

        public IList<SelectListItem> GetRecipients()
        {
            var recip = MessageManagementService.GetRecipientsList(FromId);

            return recip.Select(r => new SelectListItem
            {
                Text = r.UserName,
                Value = r.Id.ToString(CultureInfo.InvariantCulture)
            }).ToList();
        }
    }
}