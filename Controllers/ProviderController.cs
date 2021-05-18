using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using PomeloHealthApi.Database;
using PomeloHealthApi.Models;

namespace PomeloHealthApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProviderController : ControllerBase
    {
        private readonly ILogger<ProviderController> _logger;
        private readonly DatabaseContext _context;

        public ProviderController(ILogger<ProviderController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Clinic>> GetAll()
        {
            IEnumerable providers = _context.Providers.ToList();
            if (providers == null)
            {
                return NotFound();
            }
            return Ok(providers);
        }

        [HttpGet("{providerId}")]
        public async Task<ActionResult<Provider>> Get(string providerId)
        {
            var provider = await _context.Providers.FindAsync(providerId);
            if (provider == null)
            {
                return NotFound();
            }
            return provider;
        }

        [HttpPost]
        public ActionResult<Provider> Post(string firstname, string lastname)
        {
            Provider provider = new Provider()
            {
                Id = firstname + " " + lastname,
                Firstname = firstname,
                Lastname = lastname
            };
            try
            {
                _context.Providers.Add(provider);
                _context.SaveChanges();
            } catch (Exception e)
            {
                if (e is MySqlException || e is DbUpdateException)
                {
                    return Problem("Unable to create a new provider.");
                }
                throw;
            }
            return Ok(provider);
        }

        [HttpDelete("{providerId}")]
        public async Task<ActionResult> Delete(string providerId)
        {
            var provider = await _context.Providers.FindAsync(providerId);
            if (provider == null)
            {
                return NotFound();
            }
            _context.Providers.Remove(provider);
            _context.SaveChanges();
            return Ok();
        }
    }
}