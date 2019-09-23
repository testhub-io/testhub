using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using TestsHub.Data.DataModel;


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
            throw new NotImplementedException();
        }

        public void Add(TestCase testCase)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _connection?.Close();
        }
    }
}
