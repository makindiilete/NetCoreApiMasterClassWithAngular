using System;

namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        /*
         // dob here will automatically map to the value of the variable on which ds method will be called..
         so if we define d var as : -
         DateTime birthdate;
         birthdate.CalculateAge(); // birthdate (d resulting value) here will map to dob

         #    If there are other parameter we need to accept, we can then pass them behind the first parameter without the 'this'
         */
        public static int CalculateAge(this DateTime dob)
        {
            //logic to calculate user age
            var today = DateTime.Today; // as at the time of writing ds program, ds is 8/11/2020
            var age = today.Year - dob.Year; // as at the time of writing ds program, today.Year = 2020, dob.Year can b anything e.g. 1991 (using my dob)
            // we cannot simply get age by subtracting current year from d year of birth bcos if we are in November 2020 and d user was born in December 1991, d subtraction will give us 29, even though d user is still 28yrs of age until December 2020...
            //so we first run a check to see if we add d age we got above (29) in negative format to d date d user was born i.e. -29 + 2020 = 1991
            // so we check if user dateOfBirth is greater than the 1991 our calculation gives us, it means dt we have not reached d user birthday month so dt we need to remove a year from d result (age--) we then return the age
            if (dob.Date > today.AddYears(-age))
            {
                age--;
            }
            return age;
        }

    }
}
