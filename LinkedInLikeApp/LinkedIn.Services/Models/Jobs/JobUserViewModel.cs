namespace LinkedIn.Services.Models.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class JobUserViewModel
    {
        public string UserId { get; set; }

        public string Username { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}