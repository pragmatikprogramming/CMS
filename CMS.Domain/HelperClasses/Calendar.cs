using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;



namespace CMS.Domain.HelperClasses
{
    public class CMSCalendar
    {          
        public static List<List<int>> loadDays(DateTime calDate)
        {
            //string CalendarHTML = "<div class=\"fc-content\" style=\"position: relative; min-height: 1px;\"><div class=\"fc-view fc-view-month fc-grid\" style=\"position: relative;\" unselectable=\"on\"><table class=\"fc-border-separate\" style=\"width:100%\" cellspacing=\"0\"><thead><tr class=\"fc-first fc-last\"><th class=\"fc-sun fc-widget-header fc-first\" style=\"width: 97px;\">Sun</th><th class=\"fc-mon fc-widget-header\" style=\"width: 97px;\">Mon</th><th class=\"fc-tue fc-widget-header\" style=\"width: 97px;\">Tue</th><th class=\"fc-wed fc-widget-header\" style=\"width: 97px;\">Wed</th><th class=\"fc-thu fc-widget-header\" style=\"width: 97px;\">Thu</th><th class=\"fc-fri fc-widget-header\" style=\"width: 97px;\">Fri</th><th class=\"fc-sat fc-widget-header fc-last\">Sat</th></tr></thead><tbody>";
            
            List<List<int>> myCal = new List<List<int>>();
            int currMonth = calDate.Month;
            int currYear = calDate.Year;
            int daysInMonth = DateTime.DaysInMonth(currYear, currMonth);
            DateTime firstDayOfCurrMonth = DateTime.Parse(currYear.ToString() + "-" + currMonth.ToString() + "-01");
            DateTime lastDayOfCurrMonth = DateTime.Parse(currYear.ToString() + "-" + currMonth.ToString() + "-" + daysInMonth.ToString());
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;
            
            int firstWeekNum = cal.GetWeekOfYear(firstDayOfCurrMonth, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            int lastWeekNum = cal.GetWeekOfYear(lastDayOfCurrMonth, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            
            DateTime tempDate = firstDayOfCurrMonth;

            while (tempDate.DayOfWeek != DayOfWeek.Sunday)
            {
                tempDate = tempDate.AddDays(-1);
            }

            for (int i = 0; i < 6; i++)
            {
                List<int> myWeek = new List<int>();

                for (int j = 0; j < 7; j++)
                {
                    int dayNum = 0;

                    dayNum = tempDate.Day;
                    myWeek.Add(dayNum);
                    tempDate = tempDate.AddDays(1);
                }

                myCal.Add(myWeek);

            }

            return myCal;

        }
    }
}