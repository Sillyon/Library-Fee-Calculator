using System;
using System.Collections.Generic;
using System.Text;
using LibraryConfigUtilities;

namespace LibraryBusiness
{
    /* Description,
     * settingList member holds configuration parameters stored in the App.config file, 
     * please explore the properties and methods in the Country class to get a better understanding.
     * 
     * Please implement this class accordingly to accomplish requirements.
     * Feel free to add any parameters, methods, class members, etc. if necessary
     */
    public class PenaltyFeeCalculator{
        
        DateTime DateStart, DateEnd;
        string CountryCode;
        //Deðiþkenler tanýmlandý.

        private List<Country> settingList = new LibrarySetting().LibrarySettingList;
        
        public PenaltyFeeCalculator(string country_code, DateTime start_date, DateTime end_date) {
            CountryCode = country_code;
            DateStart = start_date;
            DateEnd = end_date;
        }//Program.cs'den gelen deðiþkenler ile constructor metodu tanýmlandý.

        public String Calculate(){

            if (DateEnd < DateStart)
                return "End Date is incorrect."; //gün giriþi sýralý deðilse.

            foreach (Country country in settingList)//App.config'de arama yapýldý.
            {
                int BusinessDayCount = 0;
                if (country.Culture.Contains(CountryCode))//ülke kodu bulundu.
                {
                    decimal penaltyMultiplier = country.DailyPenaltyFee;//para çarpaný alýndý.
                    //Datetime cinsinden baþlangýç gününden bitiþ gününe kadar loop.
                    for(DateTime DateCurrent = DateStart; DateCurrent<DateEnd; DateCurrent = DateCurrent.AddDays(1))
                    {
                        /* aþaðýda if içine iki koþul koyuldu.
                         * 1. koþul: ilgili gün tatil gününe denk geliyor mu
                         * 2. koþul: haftasonu mu
                         * ikisi de saðlanmazsa iþ günü sayýsý artýrýlýr. */
                        if(!country.HolidayList.Contains(DateCurrent) && !country.WeekendList.Contains(DateCurrent.DayOfWeek))
                            BusinessDayCount++;
                    }
                    BusinessDayCount -= country.PenaltyAppliesAfter;//tahammül gün sayýsýndan iþgünü çýkarýlýr.

                    if(BusinessDayCount>0)//eðer hala iþgünü sayýsý kaldýysa ceza iþlenir.
                        return Convert.ToString(penaltyMultiplier * BusinessDayCount) + " " + country.Currency;
                        //ceza çarpanýyla gün sayýsý çarpýlýr, para cinsi sona eklenir.
                    else
                        return "0.00 " + country.Currency;//ceza günü gerekenden azsa cezasýz.
                }
            }
            return "CultureInfo can NOT found.";//eðer ülke kodu bulunamadýysa.
        }
    }
}
