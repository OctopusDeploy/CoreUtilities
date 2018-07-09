using System;
using NUnit.Framework;
using Octopus.CoreUtilities.Extensions;

namespace Octopus.CoreUtilities.Tests.Fixtures
{
    using static EitherExtensions;
    using Unit = ValueTuple;
    
    [TestFixture]
    public class EitherFixture
    {
        [Test]
        public void ThrowsErrorWhenAccessingLeftWhenInRightState()
        {
            Either<string, double> result = 10;
            Assert.Throws<InvalidOperationException>(() => Assert.IsEmpty(result.Left));
        }
        
        [Test]
        public void ThrowsErrorWhenAccessingRightWhenInLeftState()
        {
            Either<string, double> result = "Go left, kind sir.";
            Assert.Throws<InvalidOperationException>(() => Assert.GreaterOrEqual(result.Right, 0.00));
        }

        [Test]
        public void CanAccessRightValueWhenInCorrectState()
        {
            var expected = 10.0;
            Either<string, double> result = expected;
            Assert.AreEqual(expected, result.Right);
        }

        [Test]
        public void CanAccessLeftValueWhenInCorrectState()
        {
            const string expected = "Go left, kind sir.";
            Either<string, double> result = "Go left, kind sir.";
            Assert.AreEqual(expected, result.Left);
        }

        [Test]
        public void MapInLeftStateDoesNotThrow()
        {
            const string expected = "Go left, kind sir.";
            Either<string, double> result = expected;
            result.Map(x => x * 5).Map(x => x + 5);
            Assert.AreEqual(result.Left, expected);
        }

        [Test]
        public void CanMapWhenInRightState()
        {
            double Calculate(double x) => x * 10 + 1;
            const double initial = 10d;
            var expected = Calculate(initial);
            var result = initial.AsRight<string, double>().Map(Calculate);
            Assert.AreEqual(expected, result.Right);
        }
        
        [Test]
        public void SelectInLeftStateDoesNotThrow()
        {
            const string expected = "Go left, kind sir.";
            Either<string, double> result = expected;
            result.Select(x => x * 5).Select(x => x + 5);
            Assert.AreEqual(result.Left, expected);
        }

        [Test]
        public void SelectWhenInRightState()
        {
            double Calculate(double x) => x * 10 + 1;
            const double initial = 10d;
            var expected = Calculate(initial);
            var result = initial.AsRight<string, double>().Select(Calculate);
            Assert.AreEqual(expected, result.Right);
        }
        

        [Test]
        public void MatchUsesLeftWhenInLeftState()
        {
            const string expected = "Left, left!";
            void AssertLeft(string value) => Assert.AreEqual(expected, value);
            void ThrowRight(double value) => throw new InvalidOperationException("Should not access right!");
            var result = "Left, left!".AsLeft<string, double>();
            Assert.DoesNotThrow(() => result.Match(AssertLeft, ThrowRight));
        }

        [Test]
        public void MatchUsersRightWhenInRightState()
        {
            const double expected = 10d;
            void ThrowLeft(string value) => throw new InvalidOperationException("Should not access left!");
            void AssertRight(double value) => Assert.AreEqual(expected, value);
            var result = expected.AsRight<string, double>();
            Assert.DoesNotThrow(() => result.Match(ThrowLeft, AssertRight));
        }

        [Test]
        public void BindToRight()
        {
            Either<string, double> first = 10d;
            Either<string, double> second = 20d;

            var result = first.Bind(x => second.Map(y => y + x));
            Assert.AreEqual(30d, result.Right);
        }

        [Test]
        public void BindToLeft()
        {
            const string expected = "I Failed";
            
            Either<string, double> first = 10d;
            var second = expected.AsLeft<string, double>();

            var result = first.Bind(x => second.Map(y => y + x));
            Assert.DoesNotThrow(() =>
            {
                Assert.AreEqual(expected, result.Left);
            });
        }

        [Test]
        public void CanPerformActionUsingRightValue()
        {
            var value = 0d;
            const double expected = 10d;
            expected.AsRight<Unit, double>().Tee(e => e.ForEach(x => value = x)).Map(x => x + 5);
            Assert.AreEqual(expected, value);
        }
    }
}