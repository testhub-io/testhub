using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHubUploadEndpoint.DataModel;


namespace TestsHubUploadEndpoint
{
    public class PostgressDataLoader : IDataLoader
    {
        NpgsqlConnection _connection;

        private NpgsqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {                    
                    var connString = "Host=localhost;Username=postgres;Password=changeme;Database=test-hub";
                    _connection = new NpgsqlConnection(connString);
                    _connection.Open();
                    return _connection;
                }

                return _connection;
            }
        }

        public void Add(TestRun testRun)
        {
            // Insert some data
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText = @"INSERT INTO public.test_runs(name)
                                            VALUES(@name)  RETURNING id";
                cmd.Parameters.AddWithValue("name", testRun.TestRunName);
                cmd.ExecuteNonQuery();
            }
        }

        public void Add(TestCase testCase)
        {
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = Connection;
                cmd.CommandText = @"INSERT INTO public.test_case(
		                                                test_run, name, status, file, classname, system_out)
                                                        VALUES(@test_run, @name, @status, @file, @classname, @system_out)";
                cmd.Parameters.AddWithValue("test_run", testCase.TestRunId);
                cmd.Parameters.AddWithValue("name", testCase.Name);
                cmd.Parameters.AddWithValue("status", testCase.Status);
                cmd.Parameters.AddWithValue("file", testCase.File);
                cmd.Parameters.AddWithValue("classname", testCase.ClassName);
                cmd.Parameters.AddWithValue("system_out", testCase.SystemOut);
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
