using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly ILayoutService _layoutService;
        public HeaderViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            LayoutVM model = new LayoutVM()
            {
                Settings = _layoutService.GetSettingsData()
            };
            return await Task.FromResult(View(model));
        }
    }
}
