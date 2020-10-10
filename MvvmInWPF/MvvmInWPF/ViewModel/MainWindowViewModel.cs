using System.Collections.ObjectModel;
using MvvmInWPF.Command;
using MvvmInWPF.Messaging;
using MvvmInWPF.Model;

namespace MvvmInWPF.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private IUser _user;
        public RelayCommand RequestLoginCommand { get; set; }

        public ObservableCollection<IUser> Users { get; set; } = new ObservableCollection<IUser>();

        public MainWindowViewModel()
        {
            Messenger.Default.Subscribe<IUser>(this,MessengerEvent.RequestUserLogin, UserLoginReceived);
            RequestLoginCommand = new RelayCommand(RequestLoginCommandExecuted);
        }


        private void RequestLoginCommandExecuted()
        {
            Messenger.Default.Notify<object>(null,MessengerEvent.RequestLoginViewOpen);
        }

        private void UserLoginReceived(IUser incomingUser)
        {             
            this.Users.Add(incomingUser);
        }

        public IUser User
        {
            get => _user;
            set { _user = value; OnPropertyChanged();}
        }
    }
}