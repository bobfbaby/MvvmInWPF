using System.Windows;
using MvvmInWPF.Controller;

namespace MvvmInWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public WindowController WindowController { get; set; } = new WindowController();
        
         
    }
}