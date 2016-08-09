using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Skimmia.xUnit.net2
{
    public class TheoryDiscoverer : IXunitTestCaseDiscoverer
    {
        public IEnumerable<IXunitTestCase> Discover(ITestFrameworkDiscoveryOptions discoveryOptions, ITestMethod testMethod,
            IAttributeInfo factAttribute)
        {
            yield return new SkimmiaTestCase(testMethod);
        }

        
    }

    internal class SkimmiaTestCase : IXunitTestCase
    {
        public SkimmiaTestCase(ITestMethod testMethod)
        {
            TestMethod = testMethod;
            Method = testMethod.Method;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
        }

        public void Serialize(IXunitSerializationInfo info)
        {
        }

        public string DisplayName { get; } = "Foo";
        public string SkipReason { get; } = null;
        public ISourceInformation SourceInformation { get; set; }
        public ITestMethod TestMethod { get; }
        public object[] TestMethodArguments { get; } = new object[0];
        public Dictionary<string, List<string>> Traits { get; } = new Dictionary<string, List<string>>();
        public string UniqueID { get; } = "SomeUniqueId";

        public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink, IMessageBus messageBus, object[] constructorArguments,
            ExceptionAggregator aggregator, CancellationTokenSource cancellationTokenSource)
        {
            var runSummary = new RunSummary()
            {
                Total = 2,
                Failed = 0,
                Skipped = 0
            };

            return Task.FromResult(runSummary);
        }

        public IMethodInfo Method { get; }
    }


    [XunitTestCaseDiscoverer("Skimmia.xUnit.net2.TheoryDiscoverer", "Skimmia.xUnit.net2")]
    public class SkimmaFactAttribute : FactAttribute
    {
        
    }
}
