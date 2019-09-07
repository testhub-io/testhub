using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHubUploadEndpoint.DataModel;

namespace TestsHubUploadEndpoint
{
    class PostgressDataLoader : IDataLoader
    {
        

        
            
        public void Add(TestRun testRun)
        {
            stirng connString = "Host=postgres;Username=changeme;Password=mypass;Database=test-hub";
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO data (some_field) VALUES (@p)";
                    cmd.Parameters.AddWithValue("p", "Hello world");
                    cmd.ExecuteNonQuery();
                }

                // Retrieve all rows
                using (var cmd = new NpgsqlCommand("SELECT some_field FROM data", conn))
                using (var reader = cmd.ExecuteReader())
                    while (reader.Read())
                        Console.WriteLine(reader.GetString(0));
            }
        }

        public void Add(TestCase testCase)
        {
            throw new NotImplementedException();
        }
    }
}
