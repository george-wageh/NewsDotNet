using Microsoft.AspNetCore.Components;

namespace NewsBlazorAppAdmin.Shared
{
    public partial class ComponentModel<TItem> where TItem : class
    {

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback<TItem?> OnSaveChanges { get; set; }
    }
}
