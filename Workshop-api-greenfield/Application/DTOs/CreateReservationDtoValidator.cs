using FluentValidation;
using System;
using Workshop_api_greenfield.Application.DTOs;

namespace Workshop_api_greenfield.Application.DTOs
{
    /// <summary>
    /// Validator for the CreateReservationDto class.
    /// </summary>
    public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateReservationDtoValidator"/> class.
        /// </summary>
        public CreateReservationDtoValidator()
        {
            RuleFor(r => r.RoomId)
                .NotEmpty().WithMessage("Room ID is required.");

            RuleFor(r => r.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(r => r.StartTime)
                .NotEmpty().WithMessage("Start time is required.")
                .Must(BeInFuture).WithMessage("Start time must be in the future.");

            RuleFor(r => r.EndTime)
                .NotEmpty().WithMessage("End time is required.")
                .GreaterThan(r => r.StartTime).WithMessage("End time must be after start time.");

            RuleFor(r => r.Purpose)
                .MaximumLength(500).WithMessage("Purpose cannot exceed 500 characters.");
        }

        private bool BeInFuture(DateTime dateTime)
        {
            return dateTime > DateTime.UtcNow;
        }
    }
} 