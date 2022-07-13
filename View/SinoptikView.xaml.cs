using SinoptikWPF.ViewModel;
using System.Windows;

namespace SinoptikWPF.View
{
    /// <summary>
    /// Interaction logic for SinoptikView.xaml
    /// </summary>
    public partial class SinoptikView : Window
    {
        public SinoptikView()
        {
            InitializeComponent();
            this.DataContext = new SinoptikViewModel();
        }
    }
}
