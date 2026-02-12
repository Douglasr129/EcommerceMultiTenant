using Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Tests.Unit
{
    public class UserTests
    {
        [Fact]
        public void Should_ThrowException_When_PriceIsNegtative()
        {
            Assert.Throws<ArgumentNullException>(() => new User("teste de nome", "", "hash", "Customer"));
            Assert.Throws<ArgumentNullException>(() => new User("teste de nome", "Email@e.com", "", "Customer"));
        }
        [Fact]
        public void Should_CreateUser_When_ValidData()
        {
            var user = new User("teste de nome", "test@email.com", "hash", "");
            Assert.NotNull(user);
            Assert.Equal("Customer", user.Role);
        }

    }
}
