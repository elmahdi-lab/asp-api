using PomeloHealthApi.Database;
using PomeloHealthApi.Models;
using System;
using System.Linq;

namespace PomeloHealthApi.Validators
{
    public static class DateValidator
    {
        private static readonly int[] _minutesAllowed = new int[4] { 0, 15, 30, 45 };
        private static readonly DatabaseContext _context = new DatabaseContext();

       

        public static bool IsValid(IDateEntity entity)
        {
            if (!IsDateValid(entity.StartDate) && !IsDateValid(entity.EndDate))
            {
                return false;
            }

            return true;
        }

        private static bool IsDateValid(DateTime date)
        {
            return _minutesAllowed.Contains(date.Minute);
        }
    }
}