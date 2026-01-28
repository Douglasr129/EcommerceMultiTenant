using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Xml.Linq;

namespace Catalog.Domain.Entities
{
    [Table("category")]
    public class Category
    {
        [Key]
        [Column("id", TypeName = "uuid")]
        public Guid Id { get; private set; }
        [Required(ErrorMessage = "Nome da categoria é obrigatório")]
        [Column("name", TypeName = "varchar(255)")]
        public string Name { get; private set; }
        public Category()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
        }

        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome da categoria inválido");

            Id = Guid.NewGuid();
            Name = name;
        }
        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Nome da categoria inválido");
            Name = name;
        }
    }

}
