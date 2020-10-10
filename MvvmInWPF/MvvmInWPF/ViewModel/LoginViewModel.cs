using MvvmInWPF.Command;
using MvvmInWPF.Messaging;
using MvvmInWPF.Model;

namespace MvvmInWPF.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private bool _isAdmin;

        public bool IsAdmin
        {
            get => _isAdmin;
            set { _isAdmin = value; OnPropertyChanged(); }
        }

        public RelayCommand LoginCommand { get; set; }

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public IUser User { get; set; }
        
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(LoginCommandExecuted);
            IsAdmin = false;
        }


        private void LoginCommandExecuted()
        {
            if (IsAdmin)
            {
                this.User = new Admin(this.Username);
            }
            else
            {
                this.User = new User(Username);
            }
             
            Messenger.Default.Notify(this.User, MessengerEvent.RequestUserLogin);
            Messenger.Default.Notify<object>(null, MessengerEvent.RequestLoginViewClose);
        }

        
    }
}