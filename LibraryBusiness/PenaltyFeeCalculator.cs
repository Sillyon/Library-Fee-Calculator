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
        //De�i�kenler tan�mland�.

        private List<Country> settingList = new LibrarySetting().LibrarySettingList;
        
        public PenaltyFeeCalculator(string country_code, DateTime start_date, DateTime end_date) {
            CountryCode = country_code;
            DateStart = start_date;
            DateEnd = end_date;
        }//Program.cs'den gelen de�i�kenler ile constructor metodu tan�mland�.

        public String Calculate(){

            if (DateEnd < DateStart)
                return "End Date is incorrect."; //g�n giri�i s�ral� de�ilse.

            foreach (Country country in settingList)//App.config'de arama yap�ld�.
            {
                int BusinessDayCount = 0;
                if (country.Culture.Contains(CountryCode))//�lke kodu bulundu.
                {
                    decimal penaltyMultiplier = country.DailyPenaltyFee;//para �arpan� al�nd�.
                    //Datetime cinsinden ba�lang�� g�n�nden biti� g�n�ne kadar loop.
                    for(DateTime DateCurrent = DateStart; DateCurrent<DateEnd; DateCurrent = DateCurrent.AddDays(1))
                    {
                        /* a�a��da if i�ine iki ko�ul koyuldu.
                         * 1. ko�ul: ilgili g�n tatil g�n�ne denk geliyor mu
                         * 2. ko�ul: haftasonu mu
                         * ikisi de sa�lanmazsa i� g�n� say�s� art�r�l�r. */
                        if(!country.HolidayList.Contains(DateCurrent) && !country.WeekendList.Contains(DateCurrent.DayOfWeek))
                            BusinessDayCount++;
                    }
                    BusinessDayCount -= country.PenaltyAppliesAfter;//tahamm�l g�n say�s�ndan i�g�n� ��kar�l�r.

                    if(BusinessDayCount>0)//e�er hala i�g�n� say�s� kald�ysa ceza i�lenir.
                        return Convert.ToString(penaltyMultiplier * BusinessDayCount) + " " + country.Currency;
                        //ceza �arpan�yla g�n say�s� �arp�l�r, para cinsi sona eklenir.
                    else
                        return "0.00 " + country.Currency;//ceza g�n� gerekenden azsa cezas�z.
                }
            }
            return "CultureInfo can NOT found.";//e�er �lke kodu bulunamad�ysa.
        }
    }
}
