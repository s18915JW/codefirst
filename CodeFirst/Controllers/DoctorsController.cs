using CodeFirst.DTOs.Requests;
using CodeFirst.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    public class DoctorsController : ControllerBase
    {
        private IDbService dbService;

        public DoctorsController(IDbService service) => dbService = service;

        [HttpGet]
        public IActionResult GetDoctors() => Ok(dbService.GetDoctors());

        [HttpPost("update")]
        public IActionResult UpdateDoctor([FromBody] DoctorUpdateRequest d) => Ok(dbService.UpdateDoctor(d));

        [HttpPost("add")]
        public IActionResult AddDoctor([FromBody] DoctorAddRequest d) => Ok(dbService.AddDoctor(d));

        [HttpDelete("delete")]
        public IActionResult DeleteDoctor([FromBody] DoctorDeleteRequest d) => Ok("Deleted " + dbService.DeleteDoctor(d));
    }

}