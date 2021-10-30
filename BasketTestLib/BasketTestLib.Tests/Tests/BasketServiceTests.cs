using BasketTestLib.Models;
using BasketTestLib.Services;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace BasketTestLib.Tests
{
    public class BasketServiceTests
    {        
        [Fact]
        public void TestSingletonThreadSafeBehaviour()
        {
            Guid thread1Guid = Guid.NewGuid();
            Guid thread2Guid = Guid.NewGuid();
            Guid thread3Guid = Guid.NewGuid();

            Thread process1 = new(() =>
            {
                thread1Guid = TestSingleton();
            });
            Thread process2 = new(() =>
            {
                thread2Guid = TestSingleton();
            });
            Thread process3 = new(() =>
            {
                thread3Guid = TestSingleton();
            });

            process1.Start();
            process2.Start();
            process3.Start();

            process1.Join();
            process2.Join();
            process3.Join();

            thread1Guid.Should().Be(thread2Guid);
            thread1Guid.Should().Be(thread3Guid);
            thread3Guid.Should().Be(thread2Guid);
        }

        [Fact]
        public void TestBasketRetrieval()
        {
            //Arrange
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            
            //Act
            var basket = singleton.GetBasket(null);
            var retrievedBasket = singleton.GetBasket(basket.BasketGuid);

            //Assert
            basket.BasketGuid.Should().NotBeEmpty();
            retrievedBasket.Should().NotBeNull();
            retrievedBasket.BasketGuid.Should().Be(basket.BasketGuid);
        }

        [Fact]
        public void TestMultipleBasket()
        {
            //Arrange
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());

            //Act
            var basket1 = singleton.GetBasket(null);
            var basket2 = singleton.GetBasket(null);            

            //Assert
            basket1.BasketGuid.Should().NotBeEmpty();
            basket2.BasketGuid.Should().NotBeEmpty();            
            basket1.BasketGuid.Should().NotBe(basket2.BasketGuid);
        }

        [Fact]
        public void TestRemoveItem()
        {
            //Arrange
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = singleton.GetBasket(null);
            var product1 = new Gloves(10.00m);
            var product2 = new Jumper(20.00m);

            //Act
            basket.AddProduct(product1);
            basket.AddProduct(product2);

            //Assert
            basket.BasketContents.Count.Should().Be(2);

            //Act
            basket.RemoveProduct(product1, out string message);

            //Assert
            basket.BasketContents.Count.Should().Be(1);
            basket.BasketContents.First().GetType().Should().Be(typeof(Jumper));
            message.Should().BeEmpty();
        }

        [Fact]
        public void TestRecalculateDiscount()
        {
            //Arrange
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            var basket = singleton.GetBasket(null);
            var product1 = new Gloves(10.00m);
            var product2 = new Jumper(20.00m);
            var offerVoucher = new OfferVoucher(10.00m, 29.99m, "YYY-YYY", typeof(Product));

            //Act - add the products and apply the voucher, then calculate the basket price
            basket.AddProduct(product1);
            basket.AddProduct(product2);
            var applyResult = basket.ApplyVoucher(offerVoucher, out string applyVoucherMessage);
            var firstTotal = basket.GetBasketFinalValue();

            //Assert - check that the voucher was applied OK initially and the amount has been updated
            applyResult.Should().BeTrue();
            firstTotal.Should().Be(20.00m);

            //Act - now remove the product which means the voucher is not valid any more
            basket.RemoveProduct(product1, out string message);

            //Assert - check that the voucher has been removed the total has been recalculated correctly, plus a message has been returned to indicate the voucher is no longer valid
            basket.BasketContents.Count.Should().Be(1);
            basket.AppliedVouchers.Count.Should().Be(0);
            basket.BasketContents.First().GetType().Should().Be(typeof(Jumper));
            basket.GetBasketFinalValue().Should().Be(30.00m);
            message.Should().Be("You have not reached the spend threshold for Gift Voucher YYY-YYY. Spend another £10.00 to receive £10.00 discount from your basket total.");
        }

        private static Guid TestSingleton()
        {
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            return singleton.Guid;
        }
    }
}
