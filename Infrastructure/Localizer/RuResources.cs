using System.Collections.Generic;

namespace AlexApps.Plugin.Payment.LiqPay.Infrastructure.Localizer
{
    public static class RuResources
    {
        public static Dictionary<string, string> Resources { get; set; } = new()
        {
            [LocalizationResourceNames.PublicKey] = "Публичный ключь",
            [LocalizationResourceNames.PublicKey + ".hint"] = "Необходимо получить в своём профиле на сайте liqpay.ua",
            [LocalizationResourceNames.PrivateKey] = "Приватный ключь",
            [LocalizationResourceNames.PrivateKey + ".hint"] = "Необходимо получить в своём профиле на сайте liqpay.ua",
            [LocalizationResourceNames.ServerCallbackUrl] = "URL уведомлений сервер-сервер",
            [LocalizationResourceNames.ServerCallbackUrl + ".hint"] = "Необходимо вписать эти данные в своём профиле на сайте liqpay.ua",
            [LocalizationResourceNames.ClientCallbackUrl] = "URL магазина клиент-сервер",
            [LocalizationResourceNames.ClientCallbackUrl + ".hint"] = "Необходимо вписать эти данные в своём профиле на сайте liqpay.ua",
            [LocalizationResourceNames.OneClickPaymentIsAllow] = "Разрешить клиентам оплату в один клик",
            [LocalizationResourceNames.OneClickPaymentIsAllow + ".hint"] = "Даёт возможность клиенту сохранить " +
                                                                           "платёжные данные в системе LiqPay для " +
                                                                           "последующих оплат в один клик. (Фукция в разработке)",
            [LocalizationResourceNames.Sandbox] = "Режим тестирования",
            [LocalizationResourceNames.Sandbox + ".hint"] = "В режиме тестирования все платежи равняются " +
                                                            "1 (UAH, RUR, USD ...) и с карты средства не списываются",
            
            [LocalizationResourceNames.YouWillBeRedirectedToLiqPay] = "Вы будете перенаправлены на сайт платёжной системы LiqPay",
            [LocalizationResourceNames.WhenButtonPressed] = "При нажатии кнопки 'Оплатить', Вы будете перенаправлены " +
                                                            "на страницу платёжной системы LiqPay ПриватБанк",
            [LocalizationResourceNames.PaymentsList] = "Вам на выбор будет предоставлено несколько способов оплаты"
        };
    }
}