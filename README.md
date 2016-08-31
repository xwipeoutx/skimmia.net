[![Build status](https://ci.appveyor.com/api/projects/status/uqt2ny5qpt14pv6u/branch/master?svg=true)](https://ci.appveyor.com/project/xwipeoutx/skimmia-net/branch/master)

# skimmia.net

Hierarchical unit test runner for C#

## Sample Usage (nUnit)
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Skimmia.Core;

namespace Skimmia.NUnit3.Tests
{
    public class SampleTests
    {
        [Test, SkimmiaTest]
        public void NestedTestSetup(SkimmaCallback when, SkimmaCallback then, SkimmaCallback it)
        {
            when("a variable is declared", () =>
            {
                var variable = "initially declared value";

                then("it can be asserted on in a test", () =>
                {
                    variable.Should().Be("initially declared value");
                });

                when("the variable is changed", () =>
                {
                    variable = "changed value";

                    then("the new value can be asserted on", () =>
                    {
                        variable.Should().Be("changed value");
                    });
                });

                it("has not run previous test setups", () =>
                {
                    variable.Should().Be("initially declared value");
                });

                variable = null;

                it("runs additional code", () =>
                {
                    variable.Should().BeNull();
                });

                when("there", () =>
                {
                    var codeReadability = "no ";

                    when("is", () =>
                    {
                        codeReadability += "nonsense ";

                        when("a lot of", () =>
                        {
                            codeReadability += "and ";

                            when("nested", () =>
                            {
                                codeReadability += "easily ";

                                when("setup", () =>
                                {
                                    codeReadability += "read.";

                                    then("assert", () =>
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
    }
}
```
