using Microsoft.EntityFrameworkCore;
using PersonalShopper.DAL;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.OrderProductRepository
{
    public class OrderProductRepository : GenericRepository<OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<(int ProductId, string ProductName, float ProductPrice, float OrderProductPrice, int OrderProductQuantity)>> 
                         ComparePricesForAProduct()
        {
            var dbContext = (ApplicationDbContext)_context;
            var pricesForAProduct = from orderProduct in dbContext.OrdersProducts
                                    join product in dbContext.Products
                                    on orderProduct.ProductId equals product.ProductId
                                    select new
                                    {
                                        orderProduct.ProductId,
                                        orderProduct.ProductName,
                                        ProductPrice = product.ProductPrice,
                                        OrderProductPrice = orderProduct.ProductPrice,
                                        orderProduct.OrderProductQuantity
                                    };

            var resultList = await pricesForAProduct.ToListAsync();

            return resultList.Select(result => (
                result.ProductId,
                result.ProductName,
                result.ProductPrice,
                result.OrderProductPrice,
                result.OrderProductQuantity
            )).ToList();
        }

    }
}
