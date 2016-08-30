using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Skimmia.Core
{
    public class Test
    {
        public Test Parent { get; }
        private readonly Dictionary<string, Test> _children = new Dictionary<string, Test>();
        private bool _isComplete;
        private Action _inspect;

        internal Test(string name, Test parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; }
        private string Key => Name.ToLower().Replace(">", "");
        private string FullKey => Parent == null ? Key : (Parent.FullKey + ">" + Key);
        public bool IsComplete => WasSkipped || _isComplete || (_isComplete = RunCount > 0 && _children.Values.All(c => c.IsComplete));

        internal void Run(Action fn)
        {
            if (WasSkipped)
                return;

            try
            {
                fn();
            }
            catch (Exception e)
            {
                Error = e;
                _inspect = fn;
            }
            RunCount++;
        }

        public void Skip()
        {
            WasSkipped = true;
        }

        public bool WasSkipped { get; private set; }

        internal int RunCount { get; private set; }

        internal Test Child(string name)
        {
            if (_children.ContainsKey(name))
                return _children[name];

            return _children[name] = new Test(name, this);
        }

        public IEnumerable<Test> Children => _children.Values.ToList();
        public IEnumerable<Test> Leaves => _children.Any() ? _children.SelectMany(c => c.Value.Leaves) : new [] { this };


        public bool HasPassed => IsComplete && _children.Values.All(c => c.HasPassed) && Error == null;

        public Exception Error { get; private set; }

        public void Inspect()
        {
            Debugger.Launch();
            _inspect(); // Step into this
        }

    }
}