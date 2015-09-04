namespace LinkedIn.Services.Models.Jobs
{
    using System;

    public class JobViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}