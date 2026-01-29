using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Application.Commands.CategoryCommands
{
    public class UpdateCategoryCommand
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
    }
}
