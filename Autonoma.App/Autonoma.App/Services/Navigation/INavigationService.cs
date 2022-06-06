using System.Collections.Generic;
using System.Threading.Tasks;

namespace Autonoma.App.Services.Navigation
{
    public interface INavigationService
    {
        Task InitializeAsync();

        Task NavigateToAsync(string route, IDictionary<string, string> routeParameters = null);
    }
}
