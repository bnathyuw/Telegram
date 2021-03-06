﻿using System;

namespace Telegram.Recipient
{
    public class TelegramReceivedEventArgs : EventArgs
    {
        public TelegramReceivedEventArgs(Telegram telegram)
        {
            Telegram = telegram;
        }

        public Telegram Telegram { get; private set; }
    }

    public delegate void TelegramReceivedEventHandler(object sender, TelegramReceivedEventArgs e);

    public interface IReceiveTelegrams : IDisposable
    {
        event TelegramReceivedEventHandler TelegramReceived;
        void Start();
    }

    public interface IDeliverTelegrams
    {
        void Deliver(Telegram telegram);
    }

    public class Clerk
    {
        private readonly IReceiveTelegrams _telegramReceiver;
        private readonly IDeliverTelegrams _telegramDeliverer;

        public Clerk(IReceiveTelegrams telegramReceiver, IDeliverTelegrams telegramDeliverer)
        {
            _telegramReceiver = telegramReceiver;
            _telegramDeliverer = telegramDeliverer;
        }

        private void Deliver(Telegram telegram)
        {
            _telegramDeliverer.Deliver(telegram);
        }

        public void Run()
        {
            _telegramReceiver.TelegramReceived += (sender, e) => Deliver(e.Telegram);
            _telegramReceiver.Start();
        }
    }
}