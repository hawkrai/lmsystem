﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.CTO
{
    public class NewsData
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string DateCreate { get; set; }

        public bool Disabled { get; set; }
    }
}
