namespace blogApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string ? UserName { get; set; }
        public string? UserPassword { get; set; }
        public string? UserEmail { get; set; }
        public string? UserState { get; set; }
        public string? UserCountry { get; set; }
        public string? AccessToken { get; set; }
        public int? ExpiresIn { get; set; }
        public string? UserSalt { get; set; }
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class UserLoginModel
    {
        public string ? Username { get; set; }
        public string ? Password { get; set; }
        public string? AccessToken { get; set; }
        public int? ExpiresIn { get; set; }
        public string? UserSalt { get; set; }
    }
}
