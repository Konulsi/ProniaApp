using Microsoft.AspNetCore.Mvc;
using Pronia.Services.Interfaces;

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
            return await Task.FromResult(View(_layoutService.GetSettingsData()));
        }
    }
}
