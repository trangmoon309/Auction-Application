using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityService
{
    public class RegisterViewModel
    {
        [Required]
        public string Email { get; set; }
        public string Button { get; set; }
        public string ReturnUrl { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}