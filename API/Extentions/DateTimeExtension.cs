using Microsoft.EntityFrameworkCore.Storage;

namespace API.Extentions
{
    public static class DateTimeExtension
    {
        public static int CalculateAge( this DateOnly dob)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = today.Year - dob.Year;
            if (dob > today.AddYears(-age)) age--;
            return age;

        }
    }
}
