using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;

namespace PropertyAgencyDesktopApp.Converters
{
    public class ClientBuyerDeductionValueConverter : IValueConverter
    {
        private const double PercentToFactor = 0.01;
        private const double DefaultDealShare = 0.45;

        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Deal deal = value as Deal;
            if (deal.Offer == null || deal.Demand == null)
            {
                return 0;
            }
            decimal totalCompanySum =
                (decimal)new ClientBuyerServicesCostValueConverter().Convert(
                    value,
                    null,
                    null,
                    null)
                 + (decimal)new ClientSellerServicesCostConverter().Convert(
                     new object[] { value },
                     null,
                     null,
                     null);

            if (deal.Demand.Agent.DealShare != null)
            {
                return deal.Demand.Agent.DealShare
                       * System.Convert.ToDecimal(PercentToFactor)
                       * totalCompanySum;
            }
            else
            {
                return System.Convert.ToDecimal(DefaultDealShare) * totalCompanySum;
            }
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
