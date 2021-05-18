using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using PomeloHealthApi.Database;
using PomeloHealthApi.Models;
using PomeloHealthApi.Validators;

namespace PomeloHealthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly DatabaseContext _context;

        public AppointmentController(ILogger<AppointmentController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Appointment>> GetAll()
        {
            IEnumerable appointments = _context.Appointments
                .Include(i => i.Provider)
                .Include(i => i.Patient)
                .ToList();
            if (appointments == null)
            {
                return NotFound();
            }
            return Ok(appointments);
        }

        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<Appointment>> Get(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        [HttpPost]
        public ActionResult<Appointment> Post(string providerId, string patientId, DateTime startDate, DateTime endDate)
        {

            // validate dates in 15 minutes

            Provider provider = _context.Providers.Find(providerId);
            Patient patient = _context.Patients.Find(patientId);
            if (provider == null || patient == null)
            {
                return NotFound("Unable to create a new Appointment, incorrect provider or patient.");
            }

            Availability availability = _context.Availabilities.Where(r =>
                    r.Provider == provider
                    && (startDate >= r.StartDate && endDate <= r.EndDate)
                ).FirstOrDefault();
            if (availability == null)
            {
                return Problem("No availability for the given provider and dates");
            }

            Appointment conflictingAppointment = _context.Appointments.Where(r =>
                r.Provider == provider
                && (
                    (r.StartDate == startDate || r.EndDate == endDate)
                    || (startDate <= r.StartDate && (endDate > r.StartDate && endDate < r.EndDate))
                    || ((startDate > r.StartDate && startDate < r.EndDate) && endDate >= r.EndDate)
                    || ((startDate > r.StartDate && startDate < r.EndDate) && (endDate > r.StartDate && endDate < r.EndDate))
                    || ((r.StartDate > startDate && r.StartDate < endDate) && (r.EndDate > startDate && r.EndDate < endDate))
                )
             ).FirstOrDefault();

            Appointment appointment = new Appointment()
            {
                Provider = provider,
                Patient = patient,
                StartDate = startDate,
                EndDate = endDate
            };

            if (conflictingAppointment is Appointment || DateValidator.IsValid(appointment))
            {
                return Problem("Cannot add an appointment for the provided timeslot.");
            }

            try
            {
                _context.Appointments.Add(appointment);
                _context.SaveChanges();
            } catch (Exception e)
            {
                if (e is MySqlException || e is DbUpdateException)
                {
                    return Problem("Unable to create a new Appointment.");
                }
                throw;
            }
            return Ok(appointment);
        }

        [HttpDelete("{appointmentId}")]
        public async Task<ActionResult> Delete(int appointmentId)
        {
            var appointment = await _context.Appointments.FindAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound();
            }
            _context.Appointments.Remove(appointment);
            _context.SaveChanges();
            return Ok();
        }
    }
}