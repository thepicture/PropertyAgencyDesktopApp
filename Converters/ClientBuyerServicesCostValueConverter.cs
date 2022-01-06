using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyAgencyDesktopApp.Converters
{
    public class ClientBuyerServicesCostValueConverter : IValueConverter
    {
        private const int ZeroCost = 0;
        private const double ClientBuyerDealShare = 0.03;

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Deal deal = value as Deal;
            return deal.Demand == null
                ? ZeroCost
                : (object)(deal.Offer.Price
                           * System.Convert.ToDecimal(ClientBuyerDealShare));
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
