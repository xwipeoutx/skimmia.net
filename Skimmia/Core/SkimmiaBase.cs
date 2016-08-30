using System;

namespace Skimmia.Core
{
    public static class SkimmiaBase
    {
        public static readonly TestEvents Events = new TestEvents();
        public static readonly TestRunner TestRunner = new TestRunner(Events);

        public static void Test(string name, Action testFunction)
        {
            TestRunner.RunTest(name, testFunction);
        }

        public static void Describe(string name, Action testFunction)
        {
            Test(name, testFunction);
        }

        public static void Given(string name, Action testFunction)
        {
            Test(name, testFunction);
        }

        public static void When(string name, Action testFunction)
        {
            Test(name, testFunction);
        }

        public static void Then(string name, Action testFunction)
        {
            Test(name, testFunction);
        }

        public static void It(string name, Action testFunction)
        {
            Test(name, testFunction);
        }
    }
}