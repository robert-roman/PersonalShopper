﻿using Microsoft.Data.SqlClient;
using PersonalShopper.DAL;
using PersonalShopper.Repositories.CartProductRepository;
using PersonalShopper.Repositories.CartRepository;
using PersonalShopper.Repositories.OrderProductRepository;
using PersonalShopper.Repositories.OrderRepository;
using PersonalShopper.Repositories.ProductRepository;
using PersonalShopper.Repositories.UserRepository;

namespace PersonalShopper.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Products = new ProductRepository.ProductRepository(_context);
            Carts = new CartRepository.CartRepository(_context);
            CartProducts = new CartProductRepository.CartProductRepository(_context);
            Orders = new OrderRepository.OrderRepository(_context);
            Users = new UserRepository.UserRepository(_context);
        }

        public IProductRepository Products
        {
            get;
            private set;
        }

        public ICartRepository Carts
        {
            get;
            private set;
        }

        public ICartProductRepository CartProducts
        {
            get;
            private set;
        }

        public IOrderRepository Orders
        {
            get;
            private set;
        }

        public IUserRepository Users
        {
            get;
            private set;
        }

        public IOrderProductRepository OrderProducts
        {
            get;
            private set;
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (SqlException e)
            {
                return 0;
            }
        }

    }
}
