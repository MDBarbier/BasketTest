using BasketTestLib.Models;
using BasketTestLib.Services;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace BasketTestLib.Tests
{
    public class BasketServiceTests
    {
        [Fact]
        public void TestRemoveItem()
        {
            //Arrange
            var basketService = new BasketService(new CodeCheckServiceStub());            
            var product1 = new Gloves(10.00m);
            var product2 = new Jumper(20.00m);

            //Act
            basketService.AddProduct(product1);
            basketService.AddProduct(product2);

            //Assert
            basketService.BasketContents.Count.Should().Be(2);

            //Act
            basketService.RemoveProduct(product1, out string message);

            //Assert
            basketService.BasketContents.Count.Should().Be(1);
            basketService.BasketContents.First().GetType().Should().Be(typeof(Jumper));
            message.Should().BeEmpty();
        }

        [Fact]
        public void TestRecalculateDiscount()
        {
            //Arrange
            var codeCheckServiceStub = new CodeCheckServiceStub();
            var basketService = new BasketService(codeCheckServiceStub);            
            var product1 = new Gloves(10.00m);
            var product2 = new Jumper(20.00m);
            var offerVoucher = new OfferVoucher(10.00m, 29.99m, "YYY-YYY", typeof(Product));

            //Act - add the products and apply the voucher, then calculate the basket price
            basketService.AddProduct(product1);
            basketService.AddProduct(product2);
            var applyResult = offerVoucher.ApplyVoucher(codeCheckServiceStub, basketService, out string _);
            var firstTotal = basketService.GetBasketFinalValue();

            //Assert - check that the voucher was applied OK initially and the amount has been updated
            applyResult.Should().BeTrue();
            firstTotal.Should().Be(20.00m);

            //Act - now remove the product which means the voucher is not valid any more
            basketService.RemoveProduct(product1, out string message);

            //Assert - check that the voucher has been removed the total has been recalculated correctly, plus a message has been returned to indicate the voucher is no longer valid
            basketService.BasketContents.Count.Should().Be(1);
            basketService.AppliedVouchers.Count.Should().Be(0);
            basketService.BasketContents.First().GetType().Should().Be(typeof(Jumper));
            basketService.GetBasketFinalValue().Should().Be(30.00m);
            message.Should().Be("You have not reached the spend threshold for Gift Voucher YYY-YYY. Spend another £10.00 to receive £10.00 discount from your basket total.");
        }

        [Fact]
        public void TestOnlySingleOfferVoucherAllowed()
        {
            //Arrange
            var codeCheckServiceStub = new CodeCheckServiceStub();
            var basketService = new BasketService(new CodeCheckServiceStub());            
            var product1 = new Gloves(50.00m);
            var product2 = new Jumper(20.00m);
            var offerVoucher1 = new OfferVoucher(10.00m, 29.99m, "YYY-YYY", typeof(Product));
            var offerVoucher2 = new OfferVoucher(10.00m, 29.99m, "YYY-YYY", typeof(Product));

            //Act - add the products and apply the voucher, then calculate the basket price
            basketService.AddProduct(product1);
            basketService.AddProduct(product2);
            var applyResult1 = offerVoucher1.ApplyVoucher(codeCheckServiceStub, basketService, out string _); 

            //Assert - check that the voucher was applied OK initially and the amount has been updated
            applyResult1.Should().BeTrue();

            //Act - now remove the product which means the voucher is not valid any more            
            var applyResult2 = offerVoucher2.ApplyVoucher(codeCheckServiceStub, basketService, out string applyVoucherMessage2);

            //Assert - check that the voucher has been removed the total has been recalculated correctly, plus a message has been returned to indicate the voucher is no longer valid
            applyResult2.Should().BeFalse();
            applyVoucherMessage2.Should().Be("An offer voucher has already been applied, only one offer voucher may be used per transaction");
        }
    }
}
