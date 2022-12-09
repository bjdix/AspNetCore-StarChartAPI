using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{   
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
                _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.FirstOrDefault(x => x.Id == id);

            if (celestialObject == null)
                return NotFound();

            celestialObject.Satellites = _context.CelestialObjects
                .Where(x => x.OrbitedObjectId == id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(x => x.Name == name);

            if (celestialObjects == null)
                return NotFound(name);

            foreach (var celestialObject in celestialObjects)
                celestialObject.Satellites = _context.CelestialObjects
                    .Where(x => x.Id == celestialObject.Id).ToList();

            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();

            if(celestialObjects == null)
                return NotFound();

            foreach (var celestialObject in celestialObjects)
                celestialObject.Satellites = _context.CelestialObjects
                    .Where(x => x.Id == celestialObject.Id).ToList();

            return Ok(celestialObjects);
        }
    }
}
