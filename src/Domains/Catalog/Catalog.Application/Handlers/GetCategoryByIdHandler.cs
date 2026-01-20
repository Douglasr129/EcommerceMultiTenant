using Catalog.Application.Queries;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Handlers
{
    public class GetCategoryByIdHandler(ICategoryRepository repository)
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<Category> Handle(GetCategoryByIdQuery query)
        {
            return await _repository.GetCategoryById(query.CategoryId);
        }
    }
}
