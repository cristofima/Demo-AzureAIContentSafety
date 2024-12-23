using AzureAIContentSafety.API.DTO.Requests;
using FluentValidation;

namespace AzureAIContentSafety.API.Validators;

public class PostValidator : AbstractValidator<PostRequest>
{
    public PostValidator()
    {
        RuleFor(x => x)
            .Must(HaveAtLeastOneField)
            .WithMessage("At least one field (Text or Image) is required.");

        When(
            x => x.Text != null,
            () =>
            {
                RuleFor(x => x.Text)
                    .NotEmpty()
                    .WithMessage("Text is required.")
                    .MinimumLength(10)
                    .WithMessage("Text must be at least 10 characters long.")
                    .MaximumLength(1000)
                    .WithMessage("Text can not exceed 1000 characters.");
            }
        );
    }

    private bool HaveAtLeastOneField(PostRequest request)
    {
        return request.Text != null || request.Image != null;
    }
}
