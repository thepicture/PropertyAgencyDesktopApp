using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyAgencyDesktopApp.Converters
{
    public class CompanyTotalPriceValueConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Deal deal = value as Deal;
            if (deal.Demand == null || deal.Offer == null)
            {
                return 0;
            }
            return (decimal)new ClientBuyerServicesCostValueConverter()
                .Convert(value,
                         null,
                         null,
                         null)
                 + (decimal)new ClientSellerServicesCostConverter()
                 .Convert(new object[] { value },
                          null,
                          null,
                          null);
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
