using Caliburn.Micro;
using TeleBot.Visual.ViewModel;
using TeleBot.Visual.ViewModels;

namespace TeleBot.Visual
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }
    }
}
