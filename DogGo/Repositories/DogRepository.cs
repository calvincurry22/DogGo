using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class DogRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT d.Id AS DogId, OwnerId, d.Name AS DogName, o.Name AS OwnerName, d.Breed, Notes, ImageURL
                        FROM Dog d
                        JOIN Owner o ON o.Id = d.OwnerId
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();
                    while (reader.Read())
                    {
                        Dog dog = new Dog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Name = reader.GetString(reader.GetOrdinal("DogName")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }

                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }

                    reader.Close();

                    return dogs;
                }
            }
        }

        //public Owner GetOwnerById(int id)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                SELECT o.Id AS OwnerId, Email, o.Name AS OwnerName, Address, NeighborhoodId, Phone, n.Name AS Area
        //                FROM Owner o
        //                JOIN Neighborhood n ON n.Id = NeighborHoodId
        //                WHERE o.Id = @id
        //            ";

        //            cmd.Parameters.AddWithValue("@id", id);

        //            SqlDataReader reader = cmd.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                Owner owner = new Owner
        //                {
        //                    Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
        //                    Email = reader.GetString(reader.GetOrdinal("Email")),
        //                    Name = reader.GetString(reader.GetOrdinal("OwnerName")),
        //                    Address = reader.GetString(reader.GetOrdinal("Address")),
        //                    NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
        //                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
        //                    Neighborhood = new Neighborhood
        //                    {
        //                        Id = reader.GetInt32(reader.GetOrdinal("NeighborhoodId")),
        //                        Area = reader.GetString(reader.GetOrdinal("Area"))
        //                    }
        //                };

        //                reader.Close();
        //                return owner;
        //            }
        //            else
        //            {
        //                reader.Close();
        //                return null;
        //            }
        //        }
        //    }
        //}
      

        public void AddDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO DOG ([Name], OwnerId, Breed, Notes, ImageUrl)
                    OUTPUT INSERTED.ID
                    VALUES (@name, @ownerId, @breed, @notes, @imageUrl);
                ";

                    cmd.Parameters.AddWithValue("@name", dog.Name);
                    cmd.Parameters.AddWithValue("@ownerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@breed", dog.Breed);
                    
                    

                    if(dog.ImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", dog.ImageUrl);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@imageUrl", DBNull.Value);
                    }

                    if (dog.ImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@notes", dog.Notes);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@notes", DBNull.Value);
                    }

                    int id = (int)cmd.ExecuteScalar();

                    dog.Id = id;
                }
            }
        }

        //public void UpdateOwner(Owner owner)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                    UPDATE Owner
        //                    SET 
        //                        [Name] = @name, 
        //                        Email = @email, 
        //                        Address = @address, 
        //                        Phone = @phone, 
        //                        NeighborhoodId = @neighborhoodId
        //                    WHERE Id = @id";

        //            cmd.Parameters.AddWithValue("@name", owner.Name);
        //            cmd.Parameters.AddWithValue("@email", owner.Email);
        //            cmd.Parameters.AddWithValue("@address", owner.Address);
        //            cmd.Parameters.AddWithValue("@phone", owner.Phone);
        //            cmd.Parameters.AddWithValue("@neighborhoodId", owner.NeighborhoodId);
        //            cmd.Parameters.AddWithValue("@id", owner.Id);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

        public void DeleteOwner(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Owner
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", ownerId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
