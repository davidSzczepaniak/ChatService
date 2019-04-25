using ChatService.ClientApp.Service;
using ChatService.ClientApp.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatService.ClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainModel = new MainViewModel();

            //Setting proxy connection to service
            Proxy.SetClient(mainModel, ConnectionFailed);
            DataContext = mainModel;
        }

        /// <summary>
        /// Connection failed flag
        /// </summary>
        private bool connectionFailed = false;

        /// <summary>
        /// Method executed when connection failed
        /// </summary>
        public void ConnectionFailed()
        {
            connectionFailed = true;
            lblConnectionFailed.Content = "Connection has been lost. Close an application";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            // Notify on logout only when connection is open
            if (!connectionFailed)
            {
                Service.Proxy.Instance.Logout(((MainViewModel)DataContext).RoomOverviewModel.CurrentUser);
                Service.Proxy.Instance.Close();
            }
        }
    }
}
