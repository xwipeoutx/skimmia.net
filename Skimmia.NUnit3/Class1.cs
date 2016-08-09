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
        public IEnumerable<TestMethod> BuildFrom(IMethodInfo method, Test suite)
        {
            var parentSuite = new TestSuite(method.Name);
            
            new NUnitTestCaseBuilder().BuildTestMethod(method, parentSuite, new TestCaseData());
            yield return new TestMethod(method);
            yield return new TestMethod(method);
        }
    }
}
