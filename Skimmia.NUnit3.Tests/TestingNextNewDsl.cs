using System;
using FluentAssertions;
using NUnit.Framework;

namespace Skimmia.NUnit3.Tests
{
    public class TestingNextNewDsl
    {
        [Test, SkimmiaTestBroken]
        public void DescribeSample(SkimmiaTest given, SkimmiaTest when, SkimmiaTest it)
        {
            given("a failing test", () =>
            {
                it("can be viewed", () =>
                {
                    throw new Exception("Error message");
                });

                it("can have multiple failures", () =>
                {
                    throw new Exception("Second error message");
                });
            });

            given("a 2nd failing test", () =>
            {
                it("can be viewed", () =>
                {
                    throw new Exception("Error message");
                });
            });

            when("a later test passes", () =>
            {
                it("hasn't been affected by an earlier failure", () =>
                {
                    true.Should().BeTrue();
                });
            });
        }
    }
}