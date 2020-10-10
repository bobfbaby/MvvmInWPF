using MvvmInWPF.Messaging;
using MvvmInWPF.View;

namespace MvvmInWPF.Controller
{
    public class WindowController
    {
        public WindowController()
        {
             
            
            Messenger.Default.Subscribe(this, MessengerEvent.RequestLoginViewOpen, () =>
            {
                new LoginView().Show();
                
            });
            
             
        }

        ~WindowController()
        {
            Messenger.Default.Unsubscribe(this, MessengerEvent.RequestLoginViewOpen);
        }
    }
}