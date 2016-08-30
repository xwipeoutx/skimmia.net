using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skimmia.NUnit3;
using Skimmia.Runner;

namespace Skimmia.Tests
{
    public static class Program
    {
        public static void Main()
        {
            SkimmaProcedural.Describe("Sample Failures", (given, when, it) =>
            {
                given("a failing test", () =>
                {
                    when("it fails", () =>
                    {
                        it("can be viewed", () =>
                        {
                            throw new Exception("This is an example error message");
                        });

                        it("can have multiple failures", () =>
                        {
                            throw new Exception("This is another error message");
                        });
                    });
                });

                given("a 2nd failing test", () =>
                {
                    it("can be viewed", () =>
                    {
                        throw new Exception("This is yet another error message");
                    });
                });

                when("a later test passes", () =>
                {
                    it("hasn't been affected by an earlier failure", () => {
                    });
                });
            }).Finish();
        }
    }
}
