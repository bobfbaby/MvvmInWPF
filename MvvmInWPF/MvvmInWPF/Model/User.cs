namespace MvvmInWPF.Model
{
    public class User: IUser
    {
        public string Username { get; set; }

        public User(string username)
        {
            this.Username = username;
        }
    }
}