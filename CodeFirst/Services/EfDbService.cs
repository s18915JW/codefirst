using CodeFirst.DTOs.Requests;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CodeFirst.Services
{
    public class EfDbService : IDbService
    {
        private CodeFirstContext dbContext;

        public EfDbService(CodeFirstContext context) => dbContext = context;

        public IEnumerable<Doctor> GetDoctors() => dbContext.Doctor.ToList();

        public Doctor UpdateDoctor([FromBody] DoctorUpdateRequest d)
        {
            var doctor = new Doctor
            {
                IdDoctor = d.IdDoctor,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
            };

            dbContext.Attach(doctor);
            dbContext.Entry(doctor).Property("FirstName").IsModified = true;
            dbContext.Entry(doctor).Property("LastName").IsModified = true;
            dbContext.Entry(doctor).Property("Email").IsModified = true;

            dbContext.SaveChanges();
            return doctor;
        }

        public Doctor AddDoctor([FromBody] DoctorAddRequest d)
        {
            var doctor = new Doctor
            {
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email,
            };

            dbContext.Doctor.Add(doctor);

            dbContext.SaveChanges();
            return doctor;
        }

        public Doctor DeleteDoctor([FromBody] DoctorDeleteRequest d)
        {
            var doctor = dbContext.Doctor
                .Where(x => (x.IdDoctor == d.IdDoctor && x.FirstName == d.FirstName && x.LastName == d.LastName))
                .FirstOrDefault();

            // get all prescriptions
            var prescriptions = dbContext.Prescription.Where(x => x.IdDoctor == doctor.IdDoctor).ToList();

            // remove from prescriptions_medicaments
            foreach (var p in prescriptions)
            {
                var pres_meds = dbContext.PrescriptionMedicament.Where(x => x.IdPrescription == p.IdPrescription).ToList();
                foreach (var pm in pres_meds)
                    dbContext.Remove(pm);
            }

            // remove prescriptions
            foreach (var p in prescriptions)
                dbContext.Remove(p);

            // remove doctor
            dbContext.Remove(doctor);

            dbContext.SaveChanges();
            return new Doctor
            {
                IdDoctor = d.IdDoctor,
                FirstName = d.FirstName,
                LastName = d.LastName
            };
        }

    }

}