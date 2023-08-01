﻿using CQRSMicro.Domain.DbContexts.Services;
using CQRSMicro.Sale.DBContext;
using CQRSMicro.Sale.DBContext.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CQRSMicro.Sale.DBContext.Services
{
    public class ProductCUDRepository : BaseCUDRepository<Entities.Product, SaleDbContext, Guid>, IProductCUDRepository
    {
        public ProductCUDRepository(DbContextOptions<SaleDbContext> options) : base(options)
        {
        }

        protected override SaleDbContext GetContext() => new(DbOptions);

        protected override IQueryable<Entities.Product> GetDbSetWithIncludes(DbContext ctx) => ctx.Set<Entities.Product>();

    }
}