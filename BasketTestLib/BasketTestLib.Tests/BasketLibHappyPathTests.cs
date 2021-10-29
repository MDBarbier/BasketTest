using BasketTestLib.Models;
using System;
using Xunit;

namespace BasketTestLib.Tests
{
    public class BasketLibHappyPathTests
    {
        [Fact]
        public void Basket1HappyPath()
        {
            //Arrange
            var jumper = new Jumper(54.65f);
            var headLight = new HeadLight(3.50f);
            var giftVoucher = new GiftVoucher(10);
            var expectedSubTotal = 68.15f;
            var basket = new Basket();
            
            //Action
            basket.AddProduct(jumper);
            basket.AddProduct(headLight);
            basket.AddProduct(giftVoucher);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }

        [Fact]
        public void Basket2HappyPath()
        {
            //Arrange
            var jumper = new Jumper(54.65f);
            var gloves = new Gloves(10.50f);            
            var expectedSubTotal = 65.15f;
            var giftVoucher = new GiftVoucher(5);
            var basket = new Basket();

            //Action
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            var voucherApplicationResult = basket.ApplyVoucher(giftVoucher);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.True(voucherApplicationResult);
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }

        [Fact]
        public void Basket3HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00f);
            var gloves = new Gloves(25.00f);            
            var expectedSubTotal = 51.00f;
            var basket = new Basket();
            var offerVoucher = new OfferVoucher(5.00f, 50.00f, typeof(HeadGear));
            //Action
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            var voucherApplicationResult = basket.ApplyVoucher(offerVoucher);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
            Assert.True(voucherApplicationResult);
        }

        [Fact]
        public void Basket4HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00f);
            var gloves = new Gloves(25.00f);
            var headLight = new HeadLight(3.50f);
            var giftVoucher = new GiftVoucher(10);
            var expectedSubTotal = 64.50f;
            var basket = new Basket();

            //Action
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);
            basket.AddProduct(headLight);
            basket.AddProduct(giftVoucher);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }

        [Fact]
        public void Basket5HappyPath()
        {
            //Arrange
            var jumper = new Jumper(26.00f);
            var gloves = new Gloves(25.00f);            
            var expectedSubTotal = 51.00f;
            var basket = new Basket();

            //Action
            basket.AddProduct(jumper);
            basket.AddProduct(gloves);            
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }

        [Fact]
        public void Basket6HappyPath()
        {
            //Arrange           
            var gloves = new Gloves(25.00f);
            var giftVoucher = new GiftVoucher(30);
            var expectedSubTotal = 55.00f;
            var basket = new Basket();

            //Action
            basket.AddProduct(gloves);
            basket.AddProduct(giftVoucher);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }

        [Fact]
        public void Basket7HappyPath()
        {
            //Arrange
            var gloves = new Gloves(25.00f);
            var expectedSubTotal = 25.00f;
            var basket = new Basket();

            //Action
            basket.AddProduct(gloves);
            var totalBasketValue = basket.GetBasketTotalValue();

            //Assert
            Assert.Equal(totalBasketValue, expectedSubTotal);
        }
    }
}
