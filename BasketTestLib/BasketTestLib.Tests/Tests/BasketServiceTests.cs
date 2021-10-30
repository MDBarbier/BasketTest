using BasketTestLib.Services;
using FluentAssertions;
using System;
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

        private static Guid TestSingleton()
        {
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            return singleton.Guid;
        }
    }
}
