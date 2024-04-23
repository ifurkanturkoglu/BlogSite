using BlogSiteModels.Models;
using FluentValidation;

namespace BlogSite.Areas.Admin.Validators
{
    public class BlogValidator : AbstractValidator<Blog>
    {
        public BlogValidator()
        {

            RuleFor(a => a.BlogTitle).NotEmpty()
                .WithMessage("Boş Bırakılmaz")
                .MaximumLength(100)
                .WithMessage("100 karakterden fazla girmeyin.");

            RuleFor(a => a.BlogDescription).NotEmpty()
                .WithMessage("Boş bırakılmaz.")
                .MaximumLength(300)
                .WithMessage("300 karakterden fazla girmeyin.");

            RuleFor(a => a.BlogText).NotEmpty()
                .WithMessage("Boş bırakılmaz")
                .MaximumLength(3000)
                .WithMessage("3000 karakterden fazla girmeyin");

            RuleFor(a => a.ImageUrl).MaximumLength(200);

        }
    }
}
