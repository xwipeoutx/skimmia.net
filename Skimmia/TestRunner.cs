using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Skimmia
{
    public class TestRunner
    {
        private readonly TestEvents _events;
        private readonly List<Test> _rootTests = new List<Test>();
        private bool _aborted;
        private Test _outerTest;
        private bool _branchHasBeenRun;

        public TestRunner(TestEvents events)
        {
            _events = events;
        }

        private void Abort()
        {
            _aborted = true;
        }

        public Test RunTest(string name, Action fn)
        {
            if (_aborted)
                return null;

            var test = CreateTest(name);

            if (_outerTest != null)
                ContinueRun(test, fn);
            else
                StartRun(test, fn);

            return test;
        }

        private Test CreateTest(string name)
        {
            return _outerTest != null
                ? _outerTest.Child(name)
                : new Test(name, null);
        }

        private void StartRun(Test test, Action testFunction)
        {
            _rootTests.Add(test);
            _events.RootStarted.Next(test);

            while (!test.IsComplete)
            {
                _branchHasBeenRun = false;
                ContinueRun(test, testFunction);
                /*
            //!!!!!!!!!!!!!!!!!! TFSBAD NOSHIP put test context back here, or it gets reused with parents.
            this.runStack(
                () => this._continueRun(test, testFunction),
                (plugin, runNext) => plugin.setup(test, runNext)
            );
            */
            }

            _events.RootComplete.Next(test);
        }

        private void ContinueRun(Test test, Action testFunction)
        {
            if (test.IsComplete || _branchHasBeenRun)
            {
                return;
            }

            RunTestFunction(test, testFunction);

            _branchHasBeenRun = true;
        }

        private void RunTestFunction(Test test, Action testFunction)
        {
            if (test.IsComplete || _branchHasBeenRun)
                return;

            if (test.RunCount == 0)
                _events.NodeFound.Next(test);

            var outerTest = _outerTest;
            _outerTest = test;

            _events.NodeEntered.Next(test);
            Thread.Sleep(100);
            test.Run(testFunction);
            _events.NodeExited.Next(test);

            if (test.IsComplete && !test.Children.Any() && !test.WasSkipped)
                RecordCompletion(test);

            _outerTest = outerTest;
        }

        private void RecordCompletion(Test test)
        {
            _events.LeafComplete.Next(test);

            _leaves.Add(test);
            if (test.HasPassed)
                _passed.Add(test);
            else
                _failed.Add(test);
        }

        private IEnumerable<Test> Tests => _rootTests;

        private readonly List<Test> _leaves = new List<Test>();
        private readonly List<Test> _passed = new List<Test>();
        private readonly List<Test> _failed = new List<Test>();
    }
}