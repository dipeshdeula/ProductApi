namespace ProductApiAsync.Command
{
    public class CreateUserCommand
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
