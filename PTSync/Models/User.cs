using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PTSync.Models
{
    public class User
    {
        public String CompanyID { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public User()
        {
            CompanyID = "";
            Name = "";
            Password = "";
        }
        public User(string companyID, string name, string password)
        {
            CompanyID = companyID;
            Name = name;
            Password = password;
        }
    }
}
