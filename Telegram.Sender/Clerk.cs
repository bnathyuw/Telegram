﻿using System;

namespace Telegram.Sender
{
    public class ChitDeliveredEventArgs
    {
        public ChitDeliveredEventArgs(Chit chit)
        {
            Chit = chit;
        }

        public Chit Chit { get; private set; }
    }

    public delegate void ChitDeliveredEventHandler(object sender, ChitDeliveredEventArgs e);

    public interface IDeliverChits : IDisposable
    {
        event ChitDeliveredEventHandler ChitDelivered;
    }

    public interface ISendTelegrams : IDisposable
    {
        void Send(Chit chit);
    }

    public class Clerk : IDisposable
    {
        private readonly IDeliverChits _chitReceiver;
        private readonly ISendTelegrams _telegramSender;

        public Clerk(IDeliverChits chitDeliverer, ISendTelegrams telegramSender)
        {
            _chitReceiver = chitDeliverer;
            _telegramSender = telegramSender;
        }

        public void Run()
        {
            _chitReceiver.ChitDelivered += (sender, args) => _telegramSender.Send(args.Chit);
        }

        public void Dispose()
        {
            _telegramSender.Dispose();
        }
    }
}