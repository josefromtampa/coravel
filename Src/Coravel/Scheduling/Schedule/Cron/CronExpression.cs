using System;
using System.Collections.Generic;

namespace Coravel.Scheduling.Schedule.Cron
{
    public class CronExpression
    {
        private string _seconds;
        private string _minutes;
        private string _hours;
        private string _days;
        private string _months;
        private string _weekdays;

        public CronExpression(string expression)
        {
            var values = expression.Split(' ');
            if (values.Length != 6)
            {
                throw new Exception($"Cron expression '{expression}' is malformed.");
            }

            this._seconds = values[0];
            this._minutes = values[1];
            this._hours = values[2];
            this._days = values[3];
            this._months = values[4];
            this._weekdays = values[5];
        }

        public CronExpression AppendWeekDay(DayOfWeek day)
        {
            int intDay = (int)day;

            if (this._weekdays == "*")
            {
                this._weekdays = intDay.ToString();
            }
            else
            {
                this._weekdays += "," + intDay.ToString();
            }

            return this;
        }

        public bool IsDue(DateTime time)
        {
            return this.IsSecondDue(time)
                && this.IsMinuteDue(time)
                && this.IsHoursDue(time)
                && this.IsDayDue(time)
                && this.IsMonthDue(time)
                && this.IsWeekDayDue(time);
        }
        public bool IsSecondDue(DateTime time)
        {
            return new CronExpressionPart(this._seconds, 60).IsDue(time.Second);
        }
        public bool IsWeekDayDue(DateTime time)
        {
            return new CronExpressionPart(this._weekdays, 7).IsDue((int)time.DayOfWeek);
        }

        private bool IsMinuteDue(DateTime time)
        {
            return new CronExpressionPart(this._minutes, 60).IsDue(time.Minute);
        }

        private bool IsHoursDue(DateTime time)
        {
            return new CronExpressionPart(this._hours, 24).IsDue(time.Hour);
        }

        private bool IsDayDue(DateTime time)
        {
            return new CronExpressionPart(this._days, 31).IsDue(time.Day);
        }

        private bool IsMonthDue(DateTime time)
        {
            return new CronExpressionPart(this._months, 12).IsDue(time.Month);
        }
    }
}