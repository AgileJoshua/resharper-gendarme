using System;
using JetBrains.Application.DataContext;
using JetBrains.DataFlow;

namespace RGendarme.Lib
{
    public interface IRGendarmeRule
    {
        bool IsEnabled(Func<Lifetime, DataContexts, IDataContext> ctx);
    }
}