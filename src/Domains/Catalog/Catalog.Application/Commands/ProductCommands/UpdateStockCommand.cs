using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Commands.ProductCommands
{
    public class UpdateStockCommand
    {
        public Guid ProductId { get; set; }
        public int amount { get; set; }
    }
}
