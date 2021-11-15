using System.Collections.Generic;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public static class UaResources
    {
        public static Dictionary<string, string> Resources { get; set; } = new()
        {
            [LocalizationResourceNames.PublicKey] = "Публічний ключ",
            [LocalizationResourceNames.PublicKey + ".hint"] = "Необхідно отримати в своєму профілі на сайті liqpay.ua",
            [LocalizationResourceNames.PrivateKey] = "Приватний ключ",
            [LocalizationResourceNames.PrivateKey + ".hint"] = "Необхідно отримати в своєму профілі на сайті liqpay.ua",
            [LocalizationResourceNames.ServerCallbackUrl] = "URL повідомлень сервер-сервер",
            [LocalizationResourceNames.ServerCallbackUrl + ".hint"] = "Необхідно вписати ці дані в своєму профілі на сайті liqpay.ua",
            [LocalizationResourceNames.ClientCallbackUrl] = "URL магазину клієнт-сервер",
            [LocalizationResourceNames.ClientCallbackUrl + ".hint"] = "Необхідно вписати ці дані в своєму профілі на сайті liqpay.ua",
            [LocalizationResourceNames.OneClickPaymentIsAllow] = "Дозволити клієнтам оплату в один клік",
            [LocalizationResourceNames.OneClickPaymentIsAllow + ".hint"] = "Дає можливість клієнту зберегти платіжні дані в системі LiqPay для подальших оплат в один клік. (Функція в розробці)",
            [LocalizationResourceNames.Sandbox] = "Режим тестування",
            [LocalizationResourceNames.Sandbox + ".hint"] = "У режимі тестування всі платежі дорівнюють 1 (UAH, RUR, USD ...) і з картки кошти не списуються",
            
            [LocalizationResourceNames.YouWillBeRedirectedToLiqPay] = "Ви будете перенаправлені на сайт платіжної системи LiqPay",
            [LocalizationResourceNames.WhenButtonPressed] = "При натисканні кнопки 'Оплатити', ви будете перенаправлені " +
                                                            "на сторінку платіжної системи LiqPay ПриватБанк",
            [LocalizationResourceNames.PaymentsList] = "Вам на вибір буде надано кілька способів оплати",
            [LocalizationResourceNames.PaymentDescription] = "Оплата через процесінговий сервіс LiqPay ПриватБанк",
        };
    }
}