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
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            var bittrex = new BittrexExchange();
            var coinbase = new BinanceExchange();
            var btTask = await bittrex.GetBalancesAsync();
            var cbTask = await coinbase.GetBalancesAsync();
            //Task.WaitAll(btTask, cbTask);
            var result = new List<Balance>(btTask);
            result.AddRange(cbTask);

            var vm = new MainVM();
            vm.Balances = new ObservableCollection<Balance>(result);

            DataContext = vm;
        }
    }
}
