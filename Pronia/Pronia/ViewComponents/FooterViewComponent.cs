using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.Services.Interfaces;
using Pronia.ViewModels;

namespace Pronia.ViewComponents
{
    public class FooterViewComponent: ViewComponent
    {
        private readonly ILayoutService _layoutService;
        public FooterViewComponent(ILayoutService layoutService)
        {
            _layoutService = layoutService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult(View(new LayoutVM { Socials = await _layoutService.GetSocialData(), 
                                                             Settings =  _layoutService.GetSettingsData() }));
        }


    }
}
