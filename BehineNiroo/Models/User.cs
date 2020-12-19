using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehineNiroo.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage =".نام کاربری وارد نشده است")]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage =".کلمه عبور وارد نشده است")]
        public string Password { get; set; }


    }
}
