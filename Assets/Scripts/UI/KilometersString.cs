using Assets.SimpleLocalization;
using System.Globalization;

namespace InGame.UI
{
    public class KilometersString
    {
        public int meters;
        
        public KilometersString(int meters)
        {
            this.meters = meters;
        }
        public KilometersString(float meters)
        {
            this.meters = (int)meters;
        }


        public static implicit operator string(KilometersString s)
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            return s.meters.ToString("#,0", nfi) + LocalizationManager.Localize("MeterSymbol");
        }
    }
}