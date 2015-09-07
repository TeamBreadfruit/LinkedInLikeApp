namespace LinkedIn.Services.Models.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class AddUserJobBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}