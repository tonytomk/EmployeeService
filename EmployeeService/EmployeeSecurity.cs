using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EmployeeDataAccess;

namespace EmployeeService
{
    public class EmployeeSecurity
    {
        public static bool Login(string userName,string passWord)
        {
            using(EmployeeDBEntities entity =new EmployeeDBEntities())
            {
                return entity.Users.Any(user => user.Username.Equals(userName, StringComparison.OrdinalIgnoreCase)&& 
                user.Password.Equals(passWord,StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}