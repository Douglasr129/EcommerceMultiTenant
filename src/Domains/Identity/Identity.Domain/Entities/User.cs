using System.Data;

namespace Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }
        public bool Active { get; private set; }

        public User(
            string name,
            string email,
            string passwordHash,
            string role
            )
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Nome inválido");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("Email inválido");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException("Senha com formato inválido");
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            if (string.IsNullOrWhiteSpace(role))
                Role = "Customer";
            else
                Role = role;

            Active = true;
        }
        public void ChangeSituation()
        {
            Active = !Active;
        }
        public void ChangeRule(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                Role = "Customer";
            else
                Role = role;
        }
        public void ChangeUser(string name, string email, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("Nome inválido");
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("Email inválido");
            if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException("Senha com formato inválido");
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
        }


    }
}
