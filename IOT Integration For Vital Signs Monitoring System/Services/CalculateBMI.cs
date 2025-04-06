namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Services
{
    public class CalculateBMI
    {
        public static decimal CaculatePatientBMI(decimal weight, decimal heightCm)
        {
            if (heightCm <= 0 || weight <= 0)
                return 0;

            decimal heightM = heightCm / 100; // Convert cm to meters
            decimal bmi = weight / (heightM * heightM);
            return Math.Round(bmi, 2); // Round to 2 decimal places
        }

    }
}
