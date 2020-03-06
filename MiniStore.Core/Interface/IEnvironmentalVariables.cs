using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Core.Interface
{
    public interface IEnvironmentalVariables
    {

        string ClientUrl { get; }
        string ConnectionString { get; }

        string SendGridKey { get; }
    }
}
