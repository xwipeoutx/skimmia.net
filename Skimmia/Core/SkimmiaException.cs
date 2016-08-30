using System;
using System.Collections.Generic;
using System.Linq;
using Skimmia.Core;

namespace Skimmia.NUnit3
{
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