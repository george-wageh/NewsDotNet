using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace NewsBlazorAppAdmin.Shared
{
    public partial class NavMenu
    {
        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
        [Inject]
        public ILocalStorageService _localStorage { set; get; }
        [Inject]
        public NavigationManager navigationManager { set; get; }
        private async Task SignOut()
        {
            await _localStorage.RemoveItemAsync("token");
            navigationManager.NavigateTo("login");
        }
    }
}
