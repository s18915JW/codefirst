using System.ComponentModel.DataAnnotations;

namespace CodeFirst.DTOs.Requests
{
    public class DoctorDeleteRequest
    {
        [Required]
        public int IdDoctor { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

}