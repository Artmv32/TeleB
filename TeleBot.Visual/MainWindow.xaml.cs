using System.Windows;
using TeleBot.Visual.ViewModel;
using TeleBot.Visual.ViewModels;

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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            //var bittrex = new BittrexExchange();
            //var coinbase = new BinanceExchange();
            //var btTask = await bittrex.GetActiveOrdersAsync();
            //var cbTask = await coinbase.GetActiveOrdersAsync();
            ////Task.WaitAll(btTask, cbTask);
            //var result = new List<TradeOrder>(btTask);
            //result.AddRange(cbTask);

            var vm = new MainViewModel();

            DataContext = vm;
        }
    }
}
