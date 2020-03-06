using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MiniStore.Core.Dto
{
    public class LoginDto
    {
        public string FirstName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string LastName { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Address { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Gender { get; set; }
    }
}
