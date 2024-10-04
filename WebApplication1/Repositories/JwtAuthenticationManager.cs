using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Data.SQLite;
using System.Security.Claims;
using System.Text;

using WebApplication1.Models;
using WebApplication1.Repositories;
using Dapper;
using System.Diagnostics;

namespace WebApplication1{
    public class JwtAuthenticationManager{
        private IConfiguration _configuration;

        public JwtAuthenticationManager(){
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        public JwtAuthResponse? Authenticate (string dealername, string email, int? dealerid)
        {
            string jwtIssure = _configuration.GetSection("JwtSettings")["Issuer"];
            
            int resDealerId;
            if(dealerid == null){
                DealerRepository dealerRepo = new DealerRepository(_configuration);
                Dealer res = dealerRepo.GetDealerByNameAndEmail(dealername, email);
                Console.WriteLine(res == null ? "Nothing can be query from LocalController" : res.Print());
                
                if (res == null){
                    return null;
                }else{
                    resDealerId = res.dealerid;
                }
            }else{
                resDealerId = (int)dealerid;
            }

            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(Constants.JWT_TOKEN_VALID_MIN);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("dealerId", resDealerId.ToString())

                }),
                Issuer = jwtIssure,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            bool isValidSave = this.InsertUpdateDealerLoginToken(token, resDealerId, tokenExpiryTimeStamp.ToString("yyyy-MM-dd HH:mm:ss"));
            if(!isValidSave){
                throw new Exception("Cannot Save Generated Token.");
            }

            return new JwtAuthResponse
            {
                token = token,
                user_id = resDealerId,
                expired_at = tokenExpiryTimeStamp.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }

        public async Task<bool> matchDealerToken(string token, int dealerid)
        {
            using(var connection = GetConnection())
            {
                connection.OpenAsync();
                string sqlStr = @"SELECT *
                        FROM DealerToken 
                        WHERE token = @token;";
                var extToken = await connection.QueryFirstOrDefaultAsync<DealerToken>(
                    sqlStr,
                    new {token = token}
                );
                if(extToken != null){
                    DateTime dtExpire = DateTime.Parse(extToken.expired_at);
                    // DateTime.Compare(t0, t1), if t0 > t1 = 1, t0 == t1 = 0, t0 < t1 = -1
                    int cmp = DateTime.Compare(DateTime.UtcNow, dtExpire);
                    return cmp >= 0;
                }else{
                    return false;
                }
            }
        }

        private bool InsertUpdateDealerLoginToken(string token, int dealerid, string expired_at)
        {
            using(var connection = GetConnection())
            {
                string sqlStr = "";
                connection.Open();
                using(var transaction = connection.BeginTransaction()){
                    sqlStr = @"SELECT *
                        FROM DealerToken 
                        WHERE dealerid = @dealerId;";
                    var extToken = connection.QueryFirstOrDefault<DealerToken>(
                        sqlStr,
                        new {dealerId = dealerid}, transaction);
                    int res;
                    if(extToken != null){
                        extToken.token = token;
                        extToken.expired_at = expired_at;
                        sqlStr = @"
                        UPDATE DealerToken SET token=@token, expired_at=@expired_at
                        WHERE id=@id AND dealerid=@dealerid;";
                        res = connection.Execute(
                            sqlStr,
                            extToken,
                            transaction
                        );
                    }else{
                        sqlStr = @"
                        INSERT INTO DealerToken (dealerid, token, expired_at)
                        VALUES (@dealerid, @token, @expired_at);";
                        res = connection.Execute(sqlStr,
                        new{
                            dealerid = dealerid,
                            token = token,
                            expired_at = expired_at
                        },
                        transaction);
                    }

                    if(res <= 0){
                        transaction.Rollback();
                        connection.Close();
                        return false;
                    }else{
                        transaction.Commit();   
                        connection.Close();
                        return true;
                    }
                }
            }
        }

        private SQLiteConnection  GetConnection()
        {
            return new SQLiteConnection (_configuration.GetConnectionString(Constants.CONN_STRING_SECTION));
        }
    }
}