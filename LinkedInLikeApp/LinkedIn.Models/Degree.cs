namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Degree
    {
        
        public Degree()
        {
            this.Id = Guid.NewGuid();        
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
      
    }
}
