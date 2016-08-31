[![Build status](https://ci.appveyor.com/api/projects/status/uqt2ny5qpt14pv6u/branch/master?svg=true)](https://ci.appveyor.com/project/xwipeoutx/skimmia-net/branch/master)

# skimmia.net

Hierarchical unit test runner for C#

## Installation

```sh
Install-Package Skimmia.NUnit3 -Pre
```

## Sample Usage (nUnit)
```powershell
using NUnit.Framework;
using Skimmia.Core;

namespace Skimmia.NUnit3.Tests
{
    public class SampleTests
    {
        [Test, SkimmiaTest]
        public void SampleTest(SkimmaCallback when, SkimmaCallback then, SkimmaCallback it)
        {
            when("setting up", () =>
            {
                // Setup code
                
                it("asserts", () =>
                {
                    // Assertion code
                });
                
                it("asserts on a second thing", () =>
                {
                    // Assertion code
                });
                
                when("setting more stuff up", () => {
                    // More setup
                    
                    it("asserts some more", () => {
                        // More asserts
                    });
                });
            });
        }
    }
}
```
