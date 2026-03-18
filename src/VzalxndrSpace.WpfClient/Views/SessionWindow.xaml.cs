using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VzalxndrSpace.WpfClient.ViewModels;

namespace VzalxndrSpace.WpfClient.Views
{
    /// <summary>
    /// Interaction logic for SessionWindow.xaml
    /// </summary>
    public partial class SessionWindow : Window
    {
        public SessionWindow()
        {
            InitializeComponent();

            this.Closing += SessionWindow_Closing;
        }

        private void SessionWindow_Closing(object? sender, CancelEventArgs e)
        {
            if (DataContext is SessionViewModel vm)
            {
                vm.ForceStopSession();
            }
        }
    }
}
