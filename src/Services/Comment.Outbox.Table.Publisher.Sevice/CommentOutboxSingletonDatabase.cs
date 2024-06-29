using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comment.Outbox.Table.Publisher.Sevice
{
    public static class CommentOutboxSingletonDatabase
    {
        static IDbConnection dbConnection;
        static IConfiguration configuration;

        static CommentOutboxSingletonDatabase() => dbConnection = new SqlConnection("Data Source=ONURCAN;Integrated Security=True;Initial Catalog = BlogApp-Microservices-Comment-Outbox;Trust Server Certificate=True;");
        public static IDbConnection Connection
        {
            get
            {
                if (dbConnection.State == ConnectionState.Closed)
                    dbConnection.Open();
                return dbConnection;
            }
        }

        static bool dataReaderState = true;
        public static async Task<IEnumerable<T>> QueryAsync<T>(string sql) => await dbConnection.QueryAsync<T>(sql);
        public static async Task<int> ExecuteAsync(string sql) => await dbConnection.ExecuteAsync(sql);
        public static void DataReaderBusy() => dataReaderState = false;
        public static void DataReaderReady() => dataReaderState = true;
        public static bool DataReaderState => dataReaderState;

    }
}
