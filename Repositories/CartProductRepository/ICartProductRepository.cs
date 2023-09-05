using Microsoft.AspNetCore.Mvc;
using PersonalShopper.DAL.DTOs;
using PersonalShopper.DAL.Models;
using PersonalShopper.DAL.Repositories.GenericRepository;

namespace PersonalShopper.Repositories.CartProductRepository
{
    public interface ICartProductRepository : IGenericRepository<CartProduct>
    {
        Task<ICollection<CartProduct>> GetProductsByCartId (int cartId);
    }
}
