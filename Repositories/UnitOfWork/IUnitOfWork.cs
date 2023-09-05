using PersonalShopper.Repositories.ProductRepository;
using PersonalShopper.Repositories.CartRepository;
using PersonalShopper.Repositories.UserRepository;
using PersonalShopper.Repositories.CartProductRepository;
using PersonalShopper.Repositories.OrderRepository;
using PersonalShopper.Repositories.OrderProductRepository;

namespace PersonalShopper.Repositories.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        ICartProductRepository CartProducts { get;  }
        IUserRepository Users { get; }
        IOrderRepository Orders { get; }
        IOrderProductRepository OrderProducts { get;  }

        int Save();
    }
}
