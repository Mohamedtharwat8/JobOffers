using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobOffers.Models
{
    public class ApplyForJob
    {
        public int Id { get; set; }

        public string Message { get; set; }
        public DateTime ApplyDate { get; set; }
        public int jobId { get; set; }
        public string UserId { get; set; }
        public virtual Job job { get; set; }
        public virtual ApplicationUser user { get; set; }
    }
}