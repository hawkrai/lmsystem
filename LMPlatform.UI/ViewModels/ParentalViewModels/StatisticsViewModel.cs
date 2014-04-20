using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMPlatform.Models;

namespace LMPlatform.UI.ViewModels.ParentalViewModels
{
    public class StatisticsViewModel : ParentalViewModel
    {
        public StatisticsViewModel(Group group)
            : base(group)
        {
        }

        public IEnumerable<SelectListItem> GetStudentsSelectList()
        {
            var items = Students.Select(s => new SelectListItem()
                        {
                            Text = s.FullName,
                            Value = s.Id.ToString(),
                            Selected = false,
                        }).OrderBy(e => e.Text).ToList();

            var allItem = new SelectListItem()
                {
                    Text = "Все",
                    Value = "-1",
                    Selected = true
                };

            items.Insert(0, allItem);

            return items;
        }
    }
}