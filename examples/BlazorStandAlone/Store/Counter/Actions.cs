using BlazorRealm;

namespace BlazorStandAlone.Store.Counter
{
    public class IncrementByOne : IAction { }

    public class IncrementByValue : IAction
    {
        public int Value { get; set; }
        public IncrementByValue(int value)
        {
            Value = value;
        }
    }

    public class DecrementByOne : IAction { }

    public class DecrementByValue : IAction
    {
        public int Value { get; set; }
        public DecrementByValue(int value)
        {
            Value = value;
        }
    }

    public class ResetCount : IAction { }
}
