using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }

        public Money(decimal amount, string currency = "BRL")
        {
            if (amount < 0)
                throw new Exceptions.DomainException("Valor monetário não pode ser negativo");

            if (string.IsNullOrWhiteSpace(currency))
                throw new Exceptions.DomainException("Moeda inválida");

            Amount = Math.Round(amount, 2); // sempre com 2 casas decimais
            Currency = currency.ToUpper();
        }

        public override string ToString()
        {
            return $"{Currency} {Amount:N2}";
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
                throw new Exceptions.DomainException("Moedas diferentes não podem ser somadas");

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
                throw new Exceptions.DomainException("Moedas diferentes não podem ser subtraídas");

            var result = Amount - other.Amount;
            if (result < 0)
                throw new Exceptions.DomainException("Resultado não pode ser negativo");

            return new Money(result, Currency);
        }
    }

}
