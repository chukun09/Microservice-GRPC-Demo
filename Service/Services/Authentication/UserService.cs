using Core.Entites;
using Core.Helper;
using Core.Helpers;
using DomainService.AuthenticationService.Input;
using DomainService.Services.AuthenticationService;
using DomainService.UnitOfWorks.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Core.Constants;
using Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DomainService.Services.AuthenticationService.Input;
using Microsoft.AspNetCore.Http;
using AutoMapper.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Connections;

namespace Service.Services.Authentication
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signInManager;
        private readonly IConfiguration _config;
        private readonly AuthenticationDbContext _dbContext;
        //private readonly IMapper _mapper;

        public UserService(UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, IConfiguration config,
            AuthenticationDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            //_mapper = mapper;
            _dbContext = dbContext;
        }

        //public async Task CreateAsync(CreateUserInput input)
        //{
        //    var newUser = _mapper.Map<UserEntity>(input);
        //    var result = await _userManager.CreateAsync(newUser, input.Password);
        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(newUser, Constants.Role.EMPLOYEE);
        //        _dbContext.SaveChanges();
        //    }
        //    else throw new CustomException("Sign up failed", 404);
        //}
        private async Task<DomainService.AuthenticationService.Input.SignInResult> GenerateJwtToken(UserEntity user, UserInfoApi userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]));
            var tokenHandler = new JwtSecurityTokenHandler();
            // Add roles to token
            var userRoleIds = _dbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
            var roles = _dbContext.Roles.Where(x => userRoleIds.Contains(x.Id)).Select(x => x.Name).ToList();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserName", user.UserName),
                    new Claim("UserId", user.Id),
                    //new Claim("Role", JsonConvert.SerializeObject(roles), JsonClaimValueTypes.JsonArray),
                    new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(roles), JsonClaimValueTypes.JsonArray)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Issuer"],
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshTokenEntity()
            {
                JwtId = token.Id,
                IsUsed = false,
                UserId = user.Id,
                AddedDate = DateTime.UtcNow.AddHours(7),
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                Token = ObjectHelper.RandomString(25) + Guid.NewGuid()
            };

            await _dbContext.RefreshTokenEntities.AddAsync(refreshToken);
            _dbContext.SaveChanges();

            return new DomainService.AuthenticationService.Input.SignInResult()
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken.Token,
                UserName = user.UserName,
                UserId = user.Id,
                FirstName = user.FirstName ?? "",
                LastName = user.LastName ?? ""
            };
        }
        public async Task<DomainService.AuthenticationService.Input.SignInResult?> RefreshTokenAsync(TokenRequest input)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _config["JWT:Issuer"],
                    ValidAudience = _config["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]))
                };

                // This validation function will make sure that the token meets the validation parameters
                // and its an actual jwt token not just a random string
                var principal = jwtTokenHandler.ValidateToken(input.AccessToken, tokenValidationParameters, out var validatedToken);

                // Now we need to check if the token has a valid security algorithm
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if (result == false)
                    {
                        return null;
                    }
                }

                // Will get the time stamp in unix time
                var utcExpiryDate = long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                // we convert the expiry date from seconds to the date
                var expDate = ObjectHelper.UnixTimeStampToDateTime(utcExpiryDate);

                if (expDate.AddHours(6) < DateTime.UtcNow) throw new CustomException("We cannot refresh this since the token has not expired", 400);

                // Check the token we got if its saved in the db
                var storedRefreshToken = await _dbContext.RefreshTokenEntities.FirstOrDefaultAsync(x => x.Token == input.RefreshToken);

                if (storedRefreshToken == null) throw new CustomException("Refresh token doesnt exist", 400);

                // Check the date of the saved token if it has expired
                if (DateTime.UtcNow > storedRefreshToken.ExpiryDate) throw new CustomException("Token has expired, user needs to relogin", 400);

                // check if the refresh token has been used
                if (storedRefreshToken.IsUsed) throw new CustomException("Token has been used", 400);

                // Check if the token is revoked
                if (storedRefreshToken.IsRevoked) throw new CustomException("Token has been revoked", 400);

                //// we are getting here the jwt token id
                //var jti = principal.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

                //// check the id that the recieved token has against the id saved in the db
                //if (storedRefreshToken.JwtId != jti) throw new CustomException("The token doenst mateched the saved token", 400);
                storedRefreshToken.IsUsed = true;
                _dbContext.RefreshTokenEntities.Update(storedRefreshToken);
                _dbContext.SaveChanges();

                var objUser = await _userManager.FindByIdAsync(storedRefreshToken.UserId);
                var objCSDG = new UserInfoApi();
                objCSDG.UserId = objUser.Id;
                return await GenerateJwtToken(objUser, objCSDG);
            }
            catch (CustomException ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        public async Task<UserEntity> SignUpAsync(SignUpInput input, CancellationToken ct)
        {
            // Step 1: Map input to UserEntity
            //var newUser = _mapper.Map<UserEntity>(input);
            var newUser = new UserEntity()
            {
                UserName = input.UserName,
                //Password = input.Password,
                Email = input.Email,
                FirstName = input.FirstName,
                LastName = input.LastName,
            };
            // Step 2: Create a new user using UserManager
            var createResult = await _userManager.CreateAsync(newUser, input.Password);

            // Step 3: Check if user creation succeeded
            if (createResult.Succeeded)
            {
                // Step 4: Add user to default role (e.g., EMPLOYEE)
                await _userManager.AddToRoleAsync(newUser, Constants.Role.EMPLOYEE);

                // Step 5: Save changes to the database
                await _dbContext.SaveChangesAsync(ct);
                var result = await _signInManager.UserManager.FindByNameAsync(newUser.UserName);
                return result;
            }
            else
            {
                // Step 6: Handle user creation failure
                throw new CustomException($"Sign up failed. {createResult.Errors?.FirstOrDefault()?.Description}", 400);
            }
        }

        public async Task<DomainService.AuthenticationService.Input.SignInResult> SignInAsync(SignInInput input)
        {
            var objUser = await _signInManager.UserManager.FindByNameAsync(input.Username);
            var response = new HttpResponseFeature();

            var features = new FeatureCollection();
            features.Set<IHttpResponseFeature>(response);
            var context = new DefaultHttpContext();

            if (objUser == null) throw new CustomException("Tài khoản không tồn tại trong hệ thống.", 400);
            //if (!objUser.IsActive) throw new CustomException("Tài khoản đang bị khóa.", 400);
            var rsSignIn = await _signInManager.PasswordSignInAsync(objUser, input.Password, input.IsRememberMe, lockoutOnFailure: false);
            if (rsSignIn.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(objUser);
                if (roles == null) throw new CustomException("Tài khoản không có quyền truy cập.", 400);
                var objCSDG = new UserInfoApi();
                objCSDG.UserId = objUser.Id;
                var result = await GenerateJwtToken(objUser, objCSDG);
                return result;
            }
            else
            {
                throw new CustomException("Tài khoản hoặc mật khẩu không chính xác.", 400);
            }
        }

        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
