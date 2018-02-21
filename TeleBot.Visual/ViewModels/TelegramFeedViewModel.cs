using Caliburn.Micro;
using System.Collections.ObjectModel;
using TeleBot.Visual.ViewModel;

namespace TeleBot.Visual.ViewModels
{
    public sealed class TelegramFeedViewModel : PropertyChangedBase
    {
        public ObservableCollection<TelegramMessage> Messages { get; private set; }

        public bool IsVisible { get; set; }

        public bool HasNewMessages { get; set; }

        public TelegramFeedViewModel()
        {
            IsVisible = true;
            Messages = new ObservableCollection<TelegramMessage>();
        }

        public void Toggle()
        {
            IsVisible = !IsVisible;
        }
    }
}
