using FluentValidation;
using System.Text.RegularExpressions;

namespace StudentManagement.Model
{
    public class StudentValidator : AbstractValidator<Student>
    {
       
        public StudentValidator()
        {

            RuleFor(s => s.Name)
                .NotEmpty()
                .MinimumLength(2).WithMessage("Name Must be of atleast 2 charaters long")
                .MaximumLength(50).WithMessage("Name connot exceed 50 characters.")
                .Must(BeValidName).WithMessage("Name contails invalid characters");
            //.Matches(@"^[a-zA-Z\s]+$").WithMessage("Name contains invalid characters (only letters and spaces are allowed)."); 

            RuleFor(s => s.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.");

            RuleFor(s => s.Department)
                .NotEmpty().WithMessage("Department is required.")
                .Length(3, 50).WithMessage("Department name must be between 3 and 50 characters.");


            RuleFor(s => s.City)
                .NotEmpty().WithMessage("City is required.")
                .MinimumLength(2).WithMessage("City name must be at least 2 characters.");

            RuleFor(s=>s.State)
                .NotEmpty().WithMessage("State is required.")
                .Length(2, 50).WithMessage("State name must be between 2 and 50 characters.");

            RuleFor(person => person.PostalCode)
            // 1. Check for a non-zero value (equivalent to NotEmpty for int)
            .NotEqual(0).WithMessage("Postal code is required.") 

            // 2. Custom check for the number of digits (5 or 6 digits)
            .Must(BeValidNumericPostalCode)
            .WithMessage("Invalid postal code. It must be 5 or 6 digits long.");

        }

        private bool BeValidNumericPostalCode(int code)
        {
            // Convert the int to a string to easily count digits
            string codeString = code.ToString();

            // Check if the length is exactly 5 or 6 digits
            return codeString.Length == 5 || codeString.Length == 6;
        }

        private bool BeValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return true;

            return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
        }
    }
}
