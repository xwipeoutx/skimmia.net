using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;

namespace Skimmia.NUnit3
{
    public class Foo : ITestAction
    {
        public void BeforeTest(ITest test)
        {

        }

        public void AfterTest(ITest test)
        {
            throw new NotImplementedException();
        }

        public ActionTargets Targets { get; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class BasilTestAttribute : Attribute, ITestBuilder
    {
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            var parentSuite = new TestSuite(method.Name);

            new NUnitTestCaseBuilder().BuildTestMethod(method, parentSuite, new TestCaseData());
            yield return new TestMethod(method);
            yield return new TestMethod(method);
        }
    }

    public delegate void SkimmiaTest(string name, Action testFunction);


    [AttributeUsage(AttributeTargets.Method)]
    public class SkimmiaTestBrokenAttribute : Attribute, ISimpleTestBuilder
    {
        TestMethod ISimpleTestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            var paramsFilledIn = SkimmiaTestAttribute.CreateTestPlatform(method);
            return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters(paramsFilledIn));
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class SkimmiaTestAttribute : Attribute, ITestBuilder
    {
        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            var paramsFilledIn = CreateTestPlatform(method);

            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters(paramsFilledIn));
        }

        public static SkimmiaTest[] CreateTestPlatform(IMethodInfo method)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            SkimmiaTest skimmiaChild = (childName, childTest) => testRunner.RunTest(childName, childTest);

            testEvents.RootComplete.Subscribe(test =>
            {
                if (!test.HasPassed)
                    throw new SkimmiaException(test);
            });

            var paramsFilledIn = method.GetParameters().Select(p => skimmiaChild).ToArray();
            return paramsFilledIn;
        }
    }

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
}
