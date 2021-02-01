using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMediaCore.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Telephono { get; set; }
        public bool IsActive { get; set; }
    }
}
