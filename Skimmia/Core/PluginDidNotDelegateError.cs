using System;

namespace Skimmia.Core
{
    class PluginDidNotDelegateError : Exception
    {
        public override string Message => "A registered plugin did not delegate";
    }
}
