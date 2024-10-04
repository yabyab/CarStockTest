using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;

using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Controllers;

namespace WebApplication1{
    public class JwtAuthenticationManager{
        public JwtAuthResponse? Authenticate (string dealername, string email, int? dealerid)
        {
            //Validate dealerName and email in hardcode.
            // TODO update it into Dealer DB Search for user validation
            int resDealerId;
            if(dealerid == null){
                DealerRepository dealerRepo = new DealerRepository(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build());
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

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_VALID_MIN);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new List<Claim>
                {
                    new Claim("dealername", dealername)

                }),
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new JwtAuthResponse
            {
                token = token,
                user_id = resDealerId,
                duration = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
            };
        }
    }
}