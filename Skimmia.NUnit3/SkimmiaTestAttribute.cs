using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using NUnit.Framework.Internal.Builders;
using Skimmia.Core;

namespace Skimmia.NUnit3
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SkimmiaTestAttribute : Attribute, ITestBuilder
    {
        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, NUnit.Framework.Internal.Test suite)
        {
            object[] paramsFilledIn = CreateTestPlatform(method);

            yield return new NUnitTestCaseBuilder().BuildTestMethod(method, suite, new TestCaseParameters(paramsFilledIn));
        }

        private static SkimmiaCallback[] CreateTestPlatform(IMethodInfo method)
        {
            var testEvents = new TestEvents();
            var testRunner = new TestRunner(testEvents);
            SkimmiaCallback skimmiaChild = (childName, childTest) => testRunner.RunTest(childName, childTest);

            testEvents.RootComplete.Subscribe(test =>
            {
                if (!test.HasPassed)
                    throw new SkimmiaException(test);
            });

            var paramsFilledIn = method.GetParameters().Select(p => skimmiaChild).ToArray();
            return paramsFilledIn;
        }
    }
}
