namespace Identity.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string Role { get; private set; }

        public User(
            string email,
            string passwordHash,
            string role
            ) 
        { 
            if(string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException("Email inválido");
            if(string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentNullException("password");
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            if (string.IsNullOrWhiteSpace(role))
                Role = "Customer";
            else
                Role = role;
        }

    }
}
