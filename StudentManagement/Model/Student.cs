using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Model
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string? Name{ get; set; }
      
        public string? Email { get; set; }

        public string? Department { get; set; }
    
        public string? State { get; set; }
   
        public string? City { get; set; }
     
        public int PostalCode { get; set; }

        
    }
}
