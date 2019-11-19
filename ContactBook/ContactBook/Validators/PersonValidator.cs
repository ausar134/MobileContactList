using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using ContactBook.Models;
using FluentValidation;

namespace ContactBook.Validators
{
    //[FluentValidation.Attributes.Validator(typeof(PersonValidator))]
    class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            //RuleFor(p => p.FirstName)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} is Empty")
            //    .Length(2,50).WithMessage("{PropertyName} must be between 2 and 50 characters")
            //    .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            //RuleFor(p => p.LastName).Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} is Empty")
            //    .Length(2, 50).WithMessage("{PropertyName} must be between 2 and 50 characters")
            //    .Must(BeAValidName).WithMessage("{PropertyName} contains invalid characters");

            //RuleFor(p => p.MobileNumber)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} is Empty")
            //    .Length(10).WithMessage("{PropertyName} must be exactly 10 characters")
            //    .Must(BeAValidNumber).WithMessage("{PropertyName} contains invalid characters");

            //RuleFor(p => p.EmailAddress)
            //      .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} is Empty")
            //    .Length(5, 40).WithMessage("{PropertyName} must be between 5 and 40 characters");
            //// .Must(BeAValidEmail).WithMessage("{PropertyName} contains invalid characters");


            //RuleFor(p => p.InternalPhone)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} is Empty")
            //    .Equal(3).WithMessage("{PropertyName} must be exactly 3 characters");
              //  .Must(BeAValidNumber).WithMessage("{ProperyName} contains invalid characters");
        }
        //protected bool BeAValidName(string name)
        //{
        //    return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c) || c == '-');
        //}

        //protected bool BeAValidNumber(string number)
        //{
        //    number = number.Replace(" ", "");
        //    number = number.Replace("-", "");
        //    return number.All(char.IsNumber);
        //}

        //protected bool BeAValidEmail(string email)
        //{
        //    try
        //    { 
        //        var address = new MailAddress(email);
        //        return true;
        //    }
        //    catch (FormatException)
        //    {
        //    }
        //    return false;        
        //}

    }
}
