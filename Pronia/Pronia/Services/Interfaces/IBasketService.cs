using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Services.Interfaces
{
    public interface IBasketService
    {
        List<BasketVM> GetBasketDatas();
        void AddProductToBasket(BasketVM existProduct, Product product, List<BasketVM> basket);

        void DeleteProductFromBasket(int id);
    }
}
