namespace LinkedIn.Services.Models.Jobs
{
    using System;
    using LinkedIn.Models;
    using System.Linq.Expressions;
    using System.Collections.Generic;

    public class UserJobViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}