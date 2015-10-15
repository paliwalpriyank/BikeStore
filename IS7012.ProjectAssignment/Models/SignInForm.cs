using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IS7012.ProjectAssignment.Models
{
    public class SignInForm
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void Reset()
        {
            Password = "";
        }
    }
}