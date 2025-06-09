using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System; // Ensure System namespace is included for ArgumentNullException, etc.
using VideoRent.Business;
using VideoRent.Domain;

namespace VideoRent.API.Controllers
{
    [ApiController] 
    [Route("api/[controller]")] // ruta base /api/actors
    public class ActorsController : ControllerBase
    {
        private readonly ActorBusiness actorBusiness;
        // Constructor inyección de dependencia
        public ActorsController(IConfiguration configuration)
        {
            this.actorBusiness = new ActorBusiness(configuration["ConnectionStrings:VideoRentDB"]);
        }

        /// <summary>
        /// Gets all actors.
        /// GET: api/actors
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<Actor>), StatusCodes.Status200OK)]
        public ActionResult<List<Actor>> GetAllActors()
        {
            try
            {
                var actors = actorBusiness.GetAllActors();
                return Ok(actors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        /// <summary>
        /// Gets a specific actor by ID.
        /// GET: api/actors/{id}
        /// </summary>
        /// <param name="id">The ID of the actor to retrieve.</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Actor), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Actor> GetActorById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Actor ID must be a positive integer.");
            }
            try
            {
                var actor = actorBusiness.GetActorById(id);
                if (actor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }
                return Ok(actor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        /// <summary>
        /// Creates a new actor.
        /// POST: api/actors
        /// </summary>
        /// <param name="actor">The actor object to create.</param>
        [HttpPost]
        [ProducesResponseType(typeof(Actor), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Actor> AddActor([FromBody] Actor actor)
        {
            // TODO improve with a DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns detailed validation errors
            }

            // Basic null check for the actor object
            if (actor == null)
            {
                return BadRequest("Actor data is required.");
            }
            //validaciones
            if (string.IsNullOrWhiteSpace(actor.NombreActor) || string.IsNullOrWhiteSpace(actor.ApellidosActor))
            {
                return BadRequest("Actor's first name and last name are required.");
            }


            try
            {
                // The AddActor method in ActorBusiness will set the ActorId
                var createdActor = actorBusiness.AddActor(actor);
                // Returns 201 Created status and a link to the newly created resource
                return CreatedAtAction(nameof(GetActorById), new { id = createdActor.ActorId }, createdActor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating the actor in the database.");
            }
        }

        /// <summary>
        /// Updates an existing actor.
        /// PUT: api/actors/{id}
        /// </summary>
        /// <param name="id">The ID of the actor to update.</param>
        /// <param name="actor">The updated actor object.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateActor(int id, [FromBody] Actor actor)
        {
            if (id <= 0)
            {
                return BadRequest("Actor ID must be a positive integer.");
            }

            // Basic model state validation
            // TODO improve with DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the ID in the URL matches the ID in the body
            if (id != actor.ActorId)
            {
                return BadRequest("Actor ID in URL does not match Actor ID in body.");
            }

            // Basic null/empty checks
            if (actor == null || string.IsNullOrWhiteSpace(actor.NombreActor) || string.IsNullOrWhiteSpace(actor.ApellidosActor))
            {
                return BadRequest("Actor data (ID, first name, last name) is required.");
            }

            try
            {
                var existingActor = actorBusiness.GetActorById(id);
                if (existingActor == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                // Perform the update
                bool updated = actorBusiness.UpdateActor(actor);
                if (updated)
                {
                    return NoContent(); // 204 No Content for successful update
                }
                else
                {
                    // This could happen if the update query affected 0 rows, even if actor existed
                    // (e.g., if actor data was identical). Or if the actor was deleted by another process.
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update actor, or actor not found after initial check.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the actor in the database.");
            }
        }

        /// <summary>
        /// Deletes an actor by ID.
        /// DELETE: api/actors/{id}
        /// </summary>
        /// <param name="id">The ID of the actor to delete.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteActor(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Actor ID must be a positive integer.");
            }

            try
            {
                // Check if actor exists before attempting deletion (optional but good for 404 response)
                var actorExists = actorBusiness.GetActorById(id);
                if (actorExists == null)
                {
                    return NotFound($"Actor with ID {id} not found.");
                }

                bool deleted = actorBusiness.DeleteActor(id);
                if (deleted)
                {
                    return NoContent(); // 204 No Content for successful deletion
                }
                else
                {
                    // This scenario might occur if GetActorById returned something,
                    // but the Delete operation affected 0 rows (e.g., race condition).
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete actor.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the actor from the database.");
            }
        }
    }
}
