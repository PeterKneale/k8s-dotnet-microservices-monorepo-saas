using System;

namespace BuildingBlocks.Domain.DDD
{
    public static class SystemClock
    {
        private static DateTime? _customDate;

        public static DateTime Now => _customDate ?? DateTime.UtcNow;

        public static void Set(DateTime customDate)
        {
            if (customDate == null)
            {
                throw new ArgumentNullException(nameof(customDate));
            }
            _customDate = customDate;
        }

        public static void Reset() => _customDate = null;
    }
}