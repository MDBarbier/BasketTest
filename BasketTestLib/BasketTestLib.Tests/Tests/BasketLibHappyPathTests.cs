using BasketTestLib.Models;
using BasketTestLib.Services;
using FluentAssertions;
using Xunit;

namespace BasketTestLib.Tests
{
    public class BasketLibHappyPathTests
    {
        [Fact]
        public void Basket1HappyPath()
        {
            //Arrange
            var jumper = new Jumper(54.65m);
            var headLight = new HeadLight(3.50m);
            var giftVoucher = new GiftVoucher(10.00m, "XXX-XXX");
            var expectedTotal = 68.15m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();
            
            //Act
            basket.AddProduct(jumper);
            basket.AddProduct(headLight);
            basket.AddProduct(giftVoucher);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
        }

        [Fact]
        public void Basket2HappyPath()
        {
            //Arrange
            var jumper = new Jumper(54.65m);
            var gloves = new Gloves(10.50m);            
            var expectedTotal = 60.15m;
            var giftVoucher = new GiftVoucher(5.00m, "XXX-XXX");
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();

            //Act
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            var voucherApplicationResult = basket.ApplyVoucher(giftVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert            
            voucherApplicationResult.Should().BeTrue();
            totalBasketValue.Should().Be(expectedTotal);
            message.Should().BeEmpty();
        }

        [Fact]
        public void Basket3HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00m);
            var gloves = new Gloves(25.00m);            
            var expectedTotal = 51.00m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();
            var offerVoucher = new OfferVoucher(5.00m, 50.00m, "YYY-YYY", typeof(HeadGear));

            //Act
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            var voucherApplicationResult = basket.ApplyVoucher(offerVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);            
            voucherApplicationResult.Should().BeFalse();
            message.Should().Be("There are no products in your basket applicable to Offer Voucher YYY-YYY.");
        }

        [Fact]
        public void Basket4HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00m);
            var gloves = new Gloves(25.00m);
            var headLight = new HeadLight(3.50m);
            var giftVoucher = new GiftVoucher(10.00m, "XXX-XXX");
            var offerVoucher = new OfferVoucher(5.00m, 50.00m, "YYY-YYY", typeof(HeadGear));
            var expectedTotal = 61.00m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();

            //Act
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            basket.AddProduct(headLight);
            basket.AddProduct(giftVoucher);
            var voucherApplicationResult = basket.ApplyVoucher(offerVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            voucherApplicationResult.Should().BeTrue();            
            message.Should().Be(string.Empty);
        }

        [Fact]
        public void Basket5HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00m);
            var gloves = new Gloves(25.00m);
            var giftVoucher = new GiftVoucher(5.00m, "XXX-XXX");
            var offerVoucher = new OfferVoucher(5.00m, 50.00m, "YYY-YYY", typeof(Product));
            var expectedTotal = 41.00m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();

            //Act
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            var giftVoucherApplicationResult = basket.ApplyVoucher(giftVoucher, out string giftVoucherMessage);
            var offerVoucherApplicationResult = basket.ApplyVoucher(offerVoucher, out string offerVoucherMessage);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            offerVoucherApplicationResult.Should().BeTrue();
            giftVoucherApplicationResult.Should().BeTrue();
            giftVoucherMessage.Should().Be(string.Empty);
            offerVoucherMessage.Should().Be(string.Empty);
        }

        [Fact]
        public void Basket6HappyPath()
        {
            //Arrange           
            var gloves = new Gloves(25.00m);
            var giftVoucher = new GiftVoucher(30.00m, "XXX-XXX");
            var offerVoucher = new OfferVoucher(5.00m, 50.00m, "YYY-YYY", typeof(Product));
            var expectedTotal = 55.00m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();

            //Act
            basket.AddProduct(gloves);
            basket.AddProduct(giftVoucher);
            var voucherApplicationResult = basket.ApplyVoucher(offerVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            voucherApplicationResult.Should().BeFalse();
            message.Should().Be("You have not reached the spend threshold for Gift Voucher YYY-YYY. Spend another £25.01 to receive £5.00 discount from your basket total.");
        }

        [Fact]
        public void Basket7HappyPath()
        {
            //Arrange
            var gloves = new Gloves(25.00m);
            var giftVoucher = new GiftVoucher(30.00m, "XXX-XXX");
            var expectedTotal = 0.00m;
            var basketService = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = basketService.GetBasket();

            //Act
            basket.AddProduct(gloves);
            var voucherApplicationResult = basket.ApplyVoucher(giftVoucher, out string message);
            var totalBasketValue = basket.GetBasketFinalValue();

            //Assert
            totalBasketValue.Should().Be(expectedTotal);
            message.Should().BeEmpty();
            voucherApplicationResult.Should().BeTrue();
        }
    }
}
