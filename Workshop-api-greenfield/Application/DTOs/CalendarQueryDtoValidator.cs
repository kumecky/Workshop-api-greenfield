using FluentValidation;
using System;
using Workshop_api_greenfield.Application.DTOs;

namespace Workshop_api_greenfield.Application.Validation
{
    /// <summary>
    /// Validator for the <see cref="CalendarQueryDto"/> class.
    /// </summary>
    public class CalendarQueryDtoValidator : AbstractValidator<CalendarQueryDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CalendarQueryDtoValidator"/> class.
        /// </summary>
        public CalendarQueryDtoValidator()
        {
            // The start date should be a valid date
            RuleFor(query => query.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .Must(BeAValidDate).WithMessage("Start date is not valid.");

            // The end date should be a valid date
            RuleFor(query => query.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .Must(BeAValidDate).WithMessage("End date is not valid.");

            // View type should be a valid enum value
            RuleFor(query => query.ViewType)
                .IsInEnum().WithMessage("Invalid view type.");

            // Check that the date range is not too large
            RuleFor(query => query)
                .Must(query => BeReasonableDateRange(query.StartDate, query.EndDate))
                .WithMessage("Date range too large. Maximum range is 31 days.");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default && date > new DateTime(2000, 1, 1) && date < new DateTime(2100, 1, 1);
        }

        private bool BeReasonableDateRange(DateTime start, DateTime end)
        {
            // Maximum 31 days range to prevent performance issues
            return (end - start).TotalDays <= 31;
        }
    }
} 