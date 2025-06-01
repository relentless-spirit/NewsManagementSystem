using System.ComponentModel.DataAnnotations;

namespace NewsManagementSystem.Models
{
    public class UpdateSystemAccountRequest
    {
        public short AccountID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 100 characters")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "The input is not a valid email address")]
        public string AccountEmail { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public int? AccountRole { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters")]
        public string AccountPassword { get; set; }
    }
    }


