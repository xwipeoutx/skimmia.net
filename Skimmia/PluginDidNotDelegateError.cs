﻿using System;

namespace Skimmia
{
    class PluginDidNotDelegateError : Exception
    {
        public override string Message => "A registered plugin did not delegate";
    }
}
