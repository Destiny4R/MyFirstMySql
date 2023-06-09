using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace MyFirstDataAccess
{

    public static class SD
    {
        //public static string claim = "499eff21-f687-46a0-8972-4d12c13e3ec0";
        public const string Role_Admin = "Admin";
        public const string Role_Staff = "Staff";
        public const string Role_ExamOfficer = "Exam Officer";
        public const string Role_Cashier = "Cashier";
        public const string Role_Accountant = "Accountant";

        //FEES PAYMENT STATUS
        public static string Fees_Status_Completed = "Completed";
        public static string Fees_Status_Part_Payment = "Part Payment";

        //Teachers status
        public static string Status_Active = "Active";
        public static string Status_InActive = "InActive";

        //Teachers status
        public static string IsEmplyee = "IsEmplyee";
        public static string IsStudent = "IsStudent";

        //Sending API SMS
        public  static string sendsms(string username, string password, string sender, string message, string numbers)
        {
            string url =  $"https://portal.nigeriabulksms.com/api/?username={username}&password={password}&message={message}&sender={sender}&mobiles={numbers}";
            return url;
        }
        //Calculation for Remark
        public static double TotalAnswer(double A1, double A2, double Test, double Exams)
        {
            double answer = A1 + A2 + Test + Exams;
            return answer;
        }
        public static string CalculateRemark(double sum)
        {
            if (sum < 40)
            {
                return "Poor";
            }
            else if (sum < 55)
            {
                return  "Fair";
            }
            else if (sum < 65)
            {
                return "Good";
            }
            else if (sum < 75)
            {
                return "Very Good";
            }
            else //if (sum > 74)
            {
                return "Distintction";
            }
        }
        public static string CalculateGrade(double sum)
        {
            if (sum < 40)
            {
                return "E";
            }
            else if (sum < 55)
            {
                return "D";
            }
            else if (sum < 65)
            {
                return "C";
            }
            else if (sum < 75)
            {
                return "B";
            }
            else //if (sum > 74)
            {
                return "A";
            }
        }
        public static string Remark(double average)
        {
            string Message = "";
            if(average < 40)
            {
                Message = "Poor Result, Put In More And Better Effort To Achieve More!";

            }
            else if(average < 55)
            {
                Message = "Good Result But Put In More And Better Effort To Achieve More, Weldone!";
            }
            else if(average < 65)
            {
                Message = "Very Good Result, I believe You Can Even Do Much More Better Than This. Weldone!";
            }
            else if( average < 75)
            {
                Message = "Excellent Result Keep the Fire in You Burning!";
            }
            else if (average > 74)
            {
                Message = "Brilliant Result, absolutely exceptional keep it up!";
            }
            return Message;
        }
    }
}
