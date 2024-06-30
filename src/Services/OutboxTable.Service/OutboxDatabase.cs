using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Comment.Outbox.Table.Publisher.Sevice
{
    public class OutboxDatabase
    {
        static IDbConnection dbConnection;
        public OutboxDatabase(string connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }
        public static IDbConnection Connection
        {
            get
            {
                if (dbConnection.State == ConnectionState.Closed)
                    dbConnection.Open();
                return dbConnection;
            }
        }

        public bool dataReaderState = true;
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql) => await dbConnection.QueryAsync<T>(sql);
        public async Task<int> ExecuteAsync(string sql) => await dbConnection.ExecuteAsync(sql);
        public void DataReaderBusy() => dataReaderState = false;
        public void DataReaderReady() => dataReaderState = true;
        public bool DataReaderState => dataReaderState;

    }
}
