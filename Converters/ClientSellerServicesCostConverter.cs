using PropertyAgencyDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PropertyAgencyDesktopApp.Converters
{
    public class ClientSellerServicesCostConverter : IMultiValueConverter
    {
        private const int NoPrice = 0;
        private const int ApartmentDealShare = 36000;
        private const double ApartmentSellerDealShare = 0.01;
        private const int PropertyDealShare = 30000;
        private const double PropertySellerDealShare = 0.02;
        private const int HouseDealShare = 30000;
        private const double HouseSellerDealShare = 0.01;

        public object Convert(object[] values,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            if (values[0] == DependencyProperty.UnsetValue)
            {
                return NoPrice;
            }
            Deal deal = (Deal)values[0];

            if (deal.Offer == null)
            {
                return NoPrice;
            }

            if (deal.Offer.Property.Apartment.Count > 0)
            {
                return ApartmentDealShare + (System.Convert.ToDecimal(ApartmentSellerDealShare)
                                * deal.Offer.Price);
            }
            else if (deal.Offer.Property.Land.Count > 0)
            {
                return PropertyDealShare + (System.Convert.ToDecimal(PropertySellerDealShare)
                              * deal.Offer.Price);
            }
            else if (deal.Offer.Property.House.Count > 0)
            {
                return HouseDealShare + (System.Convert.ToDecimal(HouseSellerDealShare)
                              * deal.Offer.Price);
            }
            return NoPrice;
        }

        public object[] ConvertBack(object value,
                                    Type[] targetTypes,
                                    object parameter,
                                    CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
