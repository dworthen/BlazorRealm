using System.Threading.Tasks;

namespace Blazor.Realm.Async
{
    public interface IAsyncAction : IAction
    {
        Task Invoke();
    }
}
