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

        private static Guid TestSingleton()
        {
            BasketService singleton = BasketService.GetInstance(new CodeCheckServiceStub());
            return singleton.Guid;
        }
    }
}
