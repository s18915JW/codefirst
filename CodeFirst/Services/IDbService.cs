using CodeFirst.DTOs.Requests;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CodeFirst.Services
{
    public interface IDbService
    {
        public IEnumerable<Doctor> GetDoctors();
        public Doctor AddDoctor([FromBody] DoctorAddRequest d);
        public Doctor UpdateDoctor([FromBody] DoctorUpdateRequest d);
        public Doctor DeleteDoctor([FromBody] DoctorDeleteRequest d);
    }

}