using System;

namespace LinkedIn.Services.Models.Educations
{
    public class DegreeViewModel
    {
        public string Name { get; set; }
    }

    public class DegreeViewModelWithId
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}