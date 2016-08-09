using System;
using FluentAssertions;
using NUnit.Framework;

namespace Skimmia.NUnit3.Tests
{
    public class TestingNewDsl
    {
        [Test]
        public void Foo()
        {
            var result = SkimmaProcedural.Describe("Sample Failures", (given, when, it) =>
            {
                given("a failing test", () =>
                {
                    it("can be viewed", () =>
                    {
                        throw new Exception("Error message");
                    });

                    it("can have multiple failures", () =>
                    {
                        "Failure2".Should().Be("Success");
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
            });

            if(!result.HasPassed)
                throw new SkimmiaException(result);
        }
    }
}