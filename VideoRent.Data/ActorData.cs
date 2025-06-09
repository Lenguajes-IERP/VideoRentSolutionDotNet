using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using VideoRent.Domain;

namespace VideoRent.Data
{
    public class ActorData
    {
        private readonly string connectionString;

        public ActorData(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // CREATE (Insert a new Actor)
        public void Create(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    INSERT INTO Actor (nombre_actor, apellidos_actor)
                    VALUES (@NombreActor, @ApellidosActor);
                    SELECT SCOPE_IDENTITY();";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreActor", actor.NombreActor);
                    command.Parameters.AddWithValue("@ApellidosActor", actor.ApellidosActor);

                    // ExecuteScalar is used to retrieve the single value returned by the query
                    // (the new actor_id)
                    object result = command.ExecuteScalar();
                    actor.ActorId = Convert.ToInt32(result);
                }
            }
        }

        // READ (Retrieve all Actors)
        public List<Actor> GetAllActors()
        {
            List<Actor> actors = new List<Actor>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                        SELECT actor_id, nombre_actor, apellidos_actor
                        FROM Actor;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            actors.Add(new Actor
                            {
                                ActorId = reader.GetInt32(reader.GetOrdinal("actor_id")),
                                NombreActor = reader.GetString(reader.GetOrdinal("nombre_actor")),
                                ApellidosActor = reader.GetString(reader.GetOrdinal("apellidos_actor"))
                            });
                        }
                    }
                }
            }
            return actors;
        }

        // READ (Retrieve an Actor by ID)
        public Actor GetActorById(int actorId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                        SELECT actor_id, nombre_actor, apellidos_actor
                        FROM Actor
                        WHERE actor_id = @ActorId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActorId", actorId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Actor
                            {
                                ActorId = reader.GetInt32(reader.GetOrdinal("actor_id")),
                                NombreActor = reader.GetString(reader.GetOrdinal("nombre_actor")),
                                ApellidosActor = reader.GetString(reader.GetOrdinal("apellidos_actor"))
                            };
                        }
                    }
                }
            }
            return null; // Actor not found
        }

        // UPDATE (Update an existing Actor)
        public bool UpdateActor(Actor actor)
        {
            if (actor == null)
            {
                throw new ArgumentNullException(nameof(actor));
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
                        UPDATE Actor
                        SET
                            nombre_actor = @NombreActor,
                            apellidos_actor = @ApellidosActor
                        WHERE
                            actor_id = @ActorId;";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NombreActor", actor.NombreActor);
                    command.Parameters.AddWithValue("@ApellidosActor", actor.ApellidosActor);
                    command.Parameters.AddWithValue("@ActorId", actor.ActorId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // DELETE (Delete an Actor by ID)
        public bool DeleteActor(int actorId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Actor WHERE actor_id = @ActorId;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ActorId", actorId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
