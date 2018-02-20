using Caliburn.Micro;
using System;

namespace TeleBot.Visual.Model
{
    public class TelegramMessage : PropertyChangedBase
    {
        public DateTime Created { get; set; }

        public string Text { get; set; }
    }
}
