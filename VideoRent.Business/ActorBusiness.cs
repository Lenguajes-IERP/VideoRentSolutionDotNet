using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoRent.Data;
using VideoRent.Domain;

namespace VideoRent.Business
{
    public class ActorBusiness
    {
        private readonly ActorData actorData;

        public ActorBusiness(string connectionString)
        {
            actorData = new ActorData(connectionString);
        }

        /// <summary>
        /// Creates a new actor in the database.
        /// </summary>
        /// <param name="actor">The Actor object to create.</param>
        /// <returns>The Actor object with its newly assigned ActorId.</returns>
        /// <remarks>Validation is omitted as per request. Ensure validation is handled at a higher layer if needed.</remarks>
        public Actor AddActor(Actor actor)
        {
            actorData.Create(actor);
            return actor;
        }

        /// <summary>
        /// Retrieves all actors from the database.
        /// </summary>
        /// <returns>A list of all Actor objects.</returns>
        public List<Actor> GetAllActors()
        {
            return actorData.GetAllActors();
        }

        /// <summary>
        /// Retrieves a specific actor by their ID.
        /// </summary>
        /// <param name="actorId">The ID of the actor to retrieve.</param>
        /// <returns>The Actor object if found, otherwise null.</returns>
        /// <remarks>Validation is omitted as per request. Ensure validation is handled at a higher layer if needed.</remarks>
        public Actor GetActorById(int actorId)
        {
            return actorData.GetActorById(actorId);
        }

        /// <summary>
        /// Updates an existing actor in the database.
        /// </summary>
        /// <param name="actor">The Actor object with updated information.</param>
        /// <returns>True if the actor was updated successfully, false otherwise.</returns>
        /// <remarks>Validation is omitted as per request. Ensure validation is handled at a higher layer if needed.</remarks>
        public bool UpdateActor(Actor actor)
        {
            return actorData.UpdateActor(actor);
        }

        /// <summary>
        /// Deletes an actor from the database by their ID.
        /// </summary>
        /// <param name="actorId">The ID of the actor to delete.</param>
        /// <returns>True if the actor was deleted successfully, false otherwise.</returns>
        /// <remarks>Validation is omitted as per request. Ensure validation is handled at a higher layer if needed.</remarks>
        public bool DeleteActor(int actorId)
        {
            return actorData.DeleteActor(actorId);
        }
    }
}
