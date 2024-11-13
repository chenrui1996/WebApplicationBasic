using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationDemo.Routing.Verification
{
    public class NotEmptyOrWhiteSpaceAttribute : ValidationAttribute
    {
        private string _errorMessage;

        public NotEmptyOrWhiteSpaceAttribute(string errorMessage = "")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str && string.IsNullOrWhiteSpace(str))
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }
    }

    public class ParamsVerificationQueryRequest
    {
        [Range(10, 19, ErrorMessage = "Id must be a positive number.")]
        public int Id { get; set; }
    }

    public class ParamsVerificationModelRequest
    {
        [Range(10, 19, ErrorMessage = "Id must be a positive number.")]
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Name length can't exceed 100 characters.")]
        public string? Name { get; set; }

        [NotEmptyOrWhiteSpace(ErrorMessage = "Label can't be null.")]
        public string? Label { get; set; }

        public override string? ToString()
        {
            return $"Id:{Id};Name:{Name};Label:{Label};";
        }
    }

    public class ParamsVerificationModelRequest1 : IValidatableObject
    {
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal Price1 { get; set; }
        public decimal Discount1 { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Discount > Price)
            {
                yield return new ValidationResult("Discount cannot be greater than Price.", new [] { "Discount" });
            }
            if (Discount1 > Price1)
            {
                yield return new ValidationResult("Discount1 cannot be greater than Price1.", new[] { "Discount1" });
            }
        }
    }

    public class ParamsVerificationFluentRequest
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

    public class ParamsVerificationFluentRequestValidator : AbstractValidator<ParamsVerificationFluentRequest>
    {
        public ParamsVerificationFluentRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.")
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name length must be between 2 and 50 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.StockQuantity)
                .GreaterThan(10)
                .When(x => x.Price > 100)
                .WithMessage("If the price is greater than 100, stock quantity must be above 10.");
        }
    }

    public class ParamsVerificationFluentRequestList
    {
        public List<ParamsVerificationFluentRequest>? ParamsVerificationFluents { get; set; }
    }

    public class ParamsVerificationsValidator : AbstractValidator<ParamsVerificationFluentRequestList>
    {
        public ParamsVerificationsValidator()
        {
            RuleForEach(x => x.ParamsVerificationFluents).SetValidator(new ParamsVerificationFluentRequestValidator());
        }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class ParamsVerificationController : ControllerBase
    {
        [HttpGet("GetValueByRouteId/{id}")]
        public IActionResult GetValueByRouteId([FromRoute, Range(0, 9)] int id)
        {
            return Ok(new { Id = id });
        }

        [HttpGet("GetValueByRouteRequestId/{Id}")]
        public IActionResult GetValueByRouteRequestId([FromRoute] ParamsVerificationQueryRequest request)
        {
            return Ok(new { request.Id });
        }

        [HttpGet("GetValueByQueryId")]
        public IActionResult GetValueById([FromQuery, Range(21, 29)] int id)
        {
            return Ok(new { Id = id });
        }

        [HttpGet("GetValueByQueryRequestId")]
        public IActionResult GetValueByQueryRequestId([FromQuery] ParamsVerificationQueryRequest request)
        {
            return Ok(new { request.Id });
        }

        [HttpPost("GetValueByFormRequest")]
        public IActionResult GetValueByFormRequest([FromForm] ParamsVerificationModelRequest request)
        {
            return Ok(request);
        }

        [HttpPost("GetValueByBodyRequest")]
        public IActionResult GetValueByBodyRequest([FromBody] ParamsVerificationModelRequest request)
        {
            return Ok(request);
        }

        [HttpPost("GetValueByBodyRequest1")]
        public IActionResult GetValueByBodyRequest1([FromBody] ParamsVerificationModelRequest1 request)
        {
            return Ok(request);
        }

        [HttpPost("GetValueByFluentRequest")]
        public IActionResult GetValueByFluentRequest([FromBody] ParamsVerificationFluentRequest request)
        {
            return Ok(request);
        }

        [HttpPost("GetValueByFluentListRequest")]
        public IActionResult GetValueByFluentListRequest([FromBody] ParamsVerificationFluentRequestList request)
        {
            return Ok(request);
        }
    }
}
