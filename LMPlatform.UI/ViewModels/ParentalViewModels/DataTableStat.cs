using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LMPlatform.UI.ViewModels.ParentalViewModels
{
    public class DataTableStat
    {
        public DataTableStat(string stat)
        {
            Stat = stat;
        }

        [DisplayName("Сообщения")]
        public string Stat { get; set; }
    }
}