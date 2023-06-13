using Microsoft.IdentityModel.Tokens;
using Saas.Core.Infrastructure.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Saas.Core.Infrastructure.Utilities
{
    public class TokenHelper
    {
        // 密钥，注意不能太短
        public static string secretKey { get; set; }


        /// <summary>
        /// 生成JWT字符串
        /// </summary>
        /// <param name="user">当前用户</param>
        /// <param name="tokenExpireTime">token有效期 单位:秒</param>
        /// <returns></returns>
        public static LoginResponseDto GetJWT(CurrentUserDto user, int tokenExpireTime = 600, bool isClient = false)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti,user.UserId),
                // 令牌颁发时间
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                 // 过期时间 从配置文件获取
                new Claim(JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(tokenExpireTime)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,Dns.GetHostName() ?? ""), //签发者
                new Claim(JwtRegisteredClaimNames.Aud,"User"), //接收者
            };

            if (isClient)
            {
                //客户端JWT凭证内容
                claims.Add(new Claim("client_LoginName", user.LoginName ?? ""));
                claims.Add(new Claim("client_Sub", "客户端"));
                claims.Add(new Claim("client_IsAdmin", user.IsAdmin.ToString()));
            }
            else
            {
                //普通用户JWT凭证
                claims.Add(new Claim("UserId", user.UserId ?? ""));
                claims.Add(new Claim("UserName", user.UserName ?? ""));
                claims.Add(new Claim("LoginName", user.LoginName ?? ""));
                claims.Add(new Claim("AvaterPath", user.AvaterPath ?? ""));
                claims.Add(new Claim("Sub", user.Sub ?? ""));
                claims.Add(new Claim("IsAdmin", user.IsAdmin.ToString()));
            }

            // 密钥
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: claims,// 声明的集合
                               //expires: .AddSeconds(36), // token的有效时间
                signingCredentials: creds
                );
            var handler = new JwtSecurityTokenHandler();
            // 生成 jwt字符串
            var strJWT = handler.WriteToken(jwt);
            return new LoginResponseDto
            {
                Token = strJWT,
                Expire = tokenExpireTime,
            };
        }

    }
}
