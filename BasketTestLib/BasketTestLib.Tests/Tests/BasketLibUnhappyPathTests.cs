using BasketTestLib.Models;
using BasketTestLib.Services;
using FluentAssertions;
using Xunit;

namespace BasketTestLib.Tests
{
    public class BasketLibUnhappyPathTests
    {
        [Fact]
        public void TestThatGiftVouchersNotAppliedToGiftVouchers()
        {
            //Arrange
            var giftVoucher = new GiftVoucher(10.00f, "XXX-XXX");
            var expectedTotal = 10.00f;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket(null);

            //Act
            basket.AddProduct(giftVoucher);
            var applicationResult = basket.ApplyVoucher(giftVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            applicationResult.Should().BeFalse();
            message.Should().Be("There are no products in your basket applicable to Gift Voucher XXX-XXX.");
        }

        [Fact]
        public void TestThatGiftVouchersNotCountedToOfferVouchers()
        {
            //Arrange
            var giftVoucher = new GiftVoucher(10.00f, "XXX-XXX");
            var gloves = new Gloves(40.00f);
            var offerVoucher = new OfferVoucher(10.00f, 50.00f, "YYY-YYY", typeof(Product));
            var expectedTotal = 50.00f;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket(null);

            //Act
            basket.AddProduct(giftVoucher);
            basket.AddProduct(gloves);
            var applicationResult = basket.ApplyVoucher(offerVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            applicationResult.Should().BeFalse();
            message.Should().Be("You have not reached the spend threshold for Gift Voucher YYY-YYY. Spend another £10.01 to receive £10.00 discount from your basket total.");
        }
    }
}
