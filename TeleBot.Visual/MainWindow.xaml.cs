using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using TeleBot.Visual.Markets;
using TeleBot.Visual.Model;

namespace TeleBot.Visual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var vm = new MainVM();
            vm.Balances = new ObservableCollection<Balance>(new Balance[] 
            {
                new Balance { Coin = "BTC", Available = 1, Locked = 2, Total = 3, Exchange = "Binance"},
                new Balance { Coin = "ETH", Available = 1, Locked = 2, Total = 3, Exchange = "Binance"},
                new Balance { Coin = "LTC", Available = 1, Locked = 2, Total = 3, Exchange = "Binance"},
                new Balance { Coin = "BCC", Available = 1, Locked = 2, Total = 3, Exchange = "Binance"},
            });
            DataContext = vm;
        }
    }
}
