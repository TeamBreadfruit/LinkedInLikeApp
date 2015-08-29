using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedIn.Services.Models.Messages
{
    public class EditMessageBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Content { get; set; }
    }
}