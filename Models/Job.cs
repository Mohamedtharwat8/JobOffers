﻿using JobOffers.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc; 

namespace JobOffers.Models
{
    public class Job
    {
        public int id { get; set; }
        [Display(Name ="Job Name")]
        public string jobTitle { get; set; }
        [Display(Name = "Job Description")]
        [AllowHtml]
        public string jobContent { get; set; }
        [Display(Name = "Job Image")]
        public string jobImage { get; set; }
        [Display(Name ="Job Type")]
        public int categoryId { get; set; }
        public string UserId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}