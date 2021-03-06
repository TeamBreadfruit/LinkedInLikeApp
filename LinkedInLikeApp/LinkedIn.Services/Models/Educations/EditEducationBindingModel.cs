﻿namespace LinkedIn.Services.Models.Educations
{
    using System;

    public class EditEducationBindingModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string DegreeName { get; set; }

        public double? Grade { get; set; }
    }
}