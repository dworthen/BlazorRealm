using System.Threading.Tasks;

namespace Blazor.Realm.Async
{
    public interface IAsyncRealmAction : IRealmAction
    {
        Task Invoke();
    }
}
