using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using static Skimmia.SkimmiaBase;

namespace Skimmia.NUnit3.Tests
{
    public class SkimmiaException : AggregateException
    {
        private readonly Test _root;
        public IEnumerable<Test> FailingTests { get; }

        public SkimmiaException(Test root)
            : this(root.Leaves.Where(t => !t.HasPassed).ToList())
        {
            _root = root;
        }

        private SkimmiaException(IReadOnlyCollection<Test> failingTests) 
            : base(failingTests.Select(e => e.Error))
        {
            FailingTests = failingTests;
        }

        public override string Message
        {
            get
            {
                return $"\n{_root.Name}\n" 
                    + FailingTests.Aggregate(string.Empty, (previous, test) => $"{previous}  {FullName(test)}\n{test.Error.Message}\n\n");
            }
        }

        public string FullName(Test test)
        {
            return test.Parent == _root 
                ? test.Name 
                : FullName(test.Parent) + "; " + test.Name;
        }
    }

    public class SampleTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Events.RootComplete.Subscribe(root =>
            {
                if (!root.HasPassed)
                {
                    throw new SkimmiaException(root);
                }
            });
        }

        [Test]
        public void DescribeNestedTestSetup()
        {
            When("a variable is declared", () =>
            {
                var variable = "initially declared value";

                Then("it can be asserted on in a test", () =>
                {
                    variable.Should().Be("initially declared value");
                });

                When("the variable is changed", () =>
                {
                    variable = "changed value";

                    Then("the new value can be asserted on", () =>
                    {
                        variable.Should().Be("changed value");
                    });
                });

                It("has not run previous test setups", () =>
                {
                    variable.Should().Be("initially declared value");
                });

                variable = null;

                It("runs additional code", () =>
                {
                    variable.Should().BeNull();
                });

                When("there", () =>
                {
                    var codeReadability = "no ";

                    When("is", () =>
                    {
                        codeReadability += "nonsense ";

                        When("a lot of", () =>
                        {
                            codeReadability += "and ";

                            When("nested", () =>
                            {
                                codeReadability += "easily ";

                                When("setup", () =>
                                {
                                    codeReadability += "read.";

                                    Then("assert", () =>
                                    {
                                        codeReadability.Should().Be("no nonsense and easily read.");
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }

        [Test]
        public void DescribeSampleFailure()
        {
            When("a test fails", () =>
            {
                It("can be viewed", () =>
                {
                    "Failure".Should().Be("Success");
                });

                It("can have multiple failures", () =>
                {
                    "Failure2".Should().Be("Success");
                });
            });

            When("a second test fails", () =>
            {
                It("can also be viewed", () =>
                {
                    "Failure".Should().Be("Success");
                });
            });

            When("in a later test", () =>
            {
                It("hasn't been affected by an earlier failure", () =>
                {
                    "Success".Should().Be("Success");
                });
            });
        }

        [Test]
        public void DescribeTestName()
        {
            It("comes from first argument", () =>
            {
                true.Should().BeTrue();
            });
        }
    }
}
