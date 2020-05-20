using System.ComponentModel.DataAnnotations;

namespace CodeFirst.DTOs.Requests
{
    public class DoctorAddRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
    }

}