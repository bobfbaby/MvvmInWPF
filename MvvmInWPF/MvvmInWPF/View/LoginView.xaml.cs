using System.Windows;
using MvvmInWPF.Messaging;

namespace MvvmInWPF.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            Messenger.Default.Subscribe(this,MessengerEvent.RequestLoginViewClose, () =>
            {
                Close();
            });
        }
    }
}