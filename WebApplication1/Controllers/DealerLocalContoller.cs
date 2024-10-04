using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Controllers
{
    public class DealerLocalController
    {
        private string sqLiteConnStr = "";

        public DealerLocalController()
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            this.sqLiteConnStr = configuration.GetSection("ConnectionStrings")[Constants.CONN_STRING_SECTION];;
        }
        

        public Dealer GetDealerByNameAndEmail(string name, string email)
        {
            using(var connection = GetConnection())
            {
                var dealer = connection.QueryFirstOrDefault<Dealer>("SELECT * FROM Dealer WHERE dealername = @dealername AND dealeremail = @dealeremail", new{ dealername = name, dealeremail = email});
                return dealer;
            }
        }

        private SQLiteConnection  GetConnection()
        {
            return new SQLiteConnection (this.sqLiteConnStr);
        }
    }
}