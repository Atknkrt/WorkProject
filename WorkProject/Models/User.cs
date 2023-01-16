using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WorkProject.Models
{
    public class User
    {
        
        public string UserID { get; set; }
        [Required(ErrorMessage = "Please enter user name.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter user surname.")]
        public string UserSurname { get; set; }    
    }
}