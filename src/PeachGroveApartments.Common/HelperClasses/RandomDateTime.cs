using System;

namespace AcmeApartments.Common.HelperClasses
{
    public class RandomDateTime
    {
        private DateTime _start;
        private Random _gen;
        private int _range;

        public RandomDateTime()
        {
            _start = new DateTime(1995, 1, 1);
            _gen = new Random();
            _range = (DateTime.Today - _start).Days;
        }

        public DateTime Next()
        {
            return _start.AddDays(
                _gen.Next(_range))
                .AddHours(_gen.Next(0, 24))
                .AddMinutes(_gen.Next(0, 60))
                .AddSeconds(_gen.Next(0, 60)
                );
        }
    }
}