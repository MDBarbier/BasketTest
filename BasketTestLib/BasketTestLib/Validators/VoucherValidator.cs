using BasketTestLib.Interfaces;
using FluentValidation;

namespace BasketTestLib.Validators
{
    public class VoucherValidator : AbstractValidator<IVoucher>
    {
        public VoucherValidator()
        {
            RuleFor(x => x.VoucherCode).NotEmpty();
            RuleFor(x => x.DiscountAmount).NotEmpty();
            RuleFor(x => x.DiscountAmount).NotEqual(0.0m);
            RuleFor(x => x.ThresholdToActivate).NotNull();
        }
    }
}
