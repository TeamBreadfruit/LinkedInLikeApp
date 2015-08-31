using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedIn.Services.Models.Groups
{
    public class GroupBindingModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

    }
}