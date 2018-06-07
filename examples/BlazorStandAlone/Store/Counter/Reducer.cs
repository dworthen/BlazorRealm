using BlazorRealm;
using BlazorStandAlone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStandAlone.Store.Counter
{
    public static class Reducer
    {
        public static int Reduce(int count, IAction action)
        {
            switch(action)
            {
                case IncrementByOne _:
                    return count + 1;
                case IncrementByValue a:
                    return count + a.Value;
                case DecrementByOne _:
                    return count - 1;
                case DecrementByValue a:
                    return count - a.Value;
                case ResetCount _:
                    return 0;
                default:
                    return count;
            }
        }
    }
}
