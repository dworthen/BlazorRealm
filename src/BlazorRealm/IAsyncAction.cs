using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealm
{
    public interface IAsyncAction : IAction
    {
        Task Invoke();
    }
}
