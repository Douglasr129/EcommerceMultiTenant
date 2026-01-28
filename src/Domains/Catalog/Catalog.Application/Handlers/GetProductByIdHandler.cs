using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers
{
    public class GetProductByIdHandler(IProductRepository repository)
    {
        private readonly IProductRepository _repository = repository;

        public async Task<Product> Handle(Guid id)
        {
            return await _repository.GetProductById (id);
        }
    }

}
