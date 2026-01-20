using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Commands
{
    public class UpdateProductCommand
    {
        public Guid ProductId {  get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
