using System;

namespace Skimmia.NUnit3.Tests
{
    public static class SkimmaProcedural
    {
        public static Action<string, Action> CreateRunChild(TestRunner runner)
        {
            return (childName, childTest) => runner.RunTest(childName, childTest);
        }

        public static Test Describe(string name, Action<Action<string, Action>> testFunction)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            var runChild = CreateRunChild(testRunner);

            return testRunner.RunTest(name, () => testFunction(runChild));
        }

        public static Test Describe(string name, Action<Action<string, Action>, Action<string, Action>> testFunction)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            var runChild = CreateRunChild(testRunner);

            return testRunner.RunTest(name, () => testFunction(runChild, runChild));
        }

        public static Test Describe(string name, Action<Action<string, Action>, Action<string, Action>, Action<string, Action>> testFunction)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            var runChild = CreateRunChild(testRunner);

            return testRunner.RunTest(name, () => testFunction(runChild, runChild, runChild));
        }

        public static Test Describe(string name, Action<Action<string, Action>, Action<string, Action>, Action<string, Action>, Action<string, Action>> testFunction)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            var runChild = CreateRunChild(testRunner);

            return testRunner.RunTest(name, () => testFunction(runChild, runChild, runChild, runChild));
        }
    }
}