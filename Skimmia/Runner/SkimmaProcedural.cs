using System;
using System.Linq;
using Skimmia.Core;
using Skimmia.NUnit3;

namespace Skimmia.Runner
{
    public static class SkimmaProcedural
    {
        public static Test Describe(string name, Action<SkimmaCallback> testFunction)
        {
            return RunTest(name, testFunction);
        }

        public static Test Describe(string name, Action<SkimmaCallback, SkimmaCallback> testFunction)
        {
            return RunTest(name, cb => testFunction(cb, cb));
        }

        public static Test Describe(string name, Action<SkimmaCallback, SkimmaCallback, SkimmaCallback> testFunction)
        {
            return RunTest(name, cb => testFunction(cb, cb, cb));
        }

        public static Test Describe(string name, Action<SkimmaCallback, SkimmaCallback, SkimmaCallback, SkimmaCallback> testFunction)
        {
            return RunTest(name, cb => testFunction(cb, cb, cb, cb));
        }

        public static Test Describe(string name, Action<SkimmaCallback, SkimmaCallback, SkimmaCallback, SkimmaCallback, SkimmaCallback> testFunction)
        {
            return RunTest(name, cb => testFunction(cb, cb, cb, cb, cb));
        }

        private static Test RunTest(string name, Action<SkimmaCallback> runTest)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            var runChild = CreateRunTestCallback(testRunner);

            return testRunner.RunTest(name, () => runTest(runChild));
        }

        private static SkimmaCallback CreateRunTestCallback(TestRunner runner)
        {
            return (childName, childTest) => runner.RunTest(childName, childTest);
        }
    }

    public static class WriteResult
    {
        public static Test Finish(this Test test)
        {
            ToConsole(test);

            if (!test.HasPassed)
                Console.WriteLine(new SkimmiaException(test));

            return test;
        }

        public static void ToConsole(Test test)
        {
            WriteTestToConsole(test, 0);
        }

        public static void WriteTestToConsole(Test test, int indent)
        {
            var success = test.HasPassed ? "-" : "×";
            var indentSpaces = string.Join("", Enumerable.Repeat(" ", indent));

            var initialColor = Console.ForegroundColor;
            Console.ForegroundColor = test.HasPassed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"{indentSpaces}{success} {test.Name}");
            Console.ForegroundColor = initialColor;

            foreach (var child in test.Children)
            {
                WriteTestToConsole(child, indent + 2);
            }
        }
    }
}