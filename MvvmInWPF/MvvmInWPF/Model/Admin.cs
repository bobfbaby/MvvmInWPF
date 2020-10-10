namespace MvvmInWPF.Model
{
    public class Admin : IUser
    {
        public string Username { get; set; }

        public Admin(string username)
        {
            this.Username = username;
        }
    }
}