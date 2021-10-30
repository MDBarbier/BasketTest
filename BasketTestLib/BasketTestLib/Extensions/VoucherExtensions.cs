using BasketTestLib.Exceptions;
using BasketTestLib.Interfaces;
using BasketTestLib.Validators;
using FluentValidation.Results;

namespace BasketTestLib.Models
{
    public static class VoucherExtensions
    {
        public static void ValidateVoucher(this IVoucher voucher, ICodeCheckService codeCheckService)
        {
            if (!codeCheckService.CheckCodeValidity(voucher.VoucherCode))
            {
                throw new VoucherCodeInvalidException($"Provided voucher code {voucher.VoucherCode} was not recognised");
            }

            var validator = new VoucherValidator();
            ValidationResult resultOfValidation = validator.Validate(voucher);

            if (!resultOfValidation.IsValid)
            {
                throw new InvalidVoucherException("Validation failed with message: " + resultOfValidation.ToString());
            }
        }
    }
}
