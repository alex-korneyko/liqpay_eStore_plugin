using System;

namespace AlexApps.Plugin.Payment.LiqPay.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string message)
            : base(message)
        {
        }
    }
}