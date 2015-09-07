namespace LinkedIn.Services.Models.Jobs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class JobBindingModel
    {
        [Required]
        public string Name { get; set; }
    }
}