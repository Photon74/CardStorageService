using CardStorageService.Data;
using CardStorageService.Models;
using CardStorageService.Models.Requests;
using CardStorageService.Models.Responses;
using CardStorageService.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CardStorageService.Services.Imp
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IDictionary<string, SessionInfo> _sessionsCache = new Dictionary<string, SessionInfo>();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public const string SecretCode = "z63H~i0}ZxIQlQpu";

        public AuthenticateService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public SessionInfo GetSessionInfo(string sessionToken)
        {
            SessionInfo sessionInfo;

            lock (_sessionsCache)
            {
                _sessionsCache.TryGetValue(sessionToken, out sessionInfo);
            }

            if (sessionInfo == null)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();

                var session = context.AccountSessions.FirstOrDefault(session => session.SessionToken == sessionToken);

                if (session == null) return null;

                var account = context.Accounts.FirstOrDefault(account => account.AccountId == session.AccountId);

                sessionInfo = GetSessionInfo(account, session);

                if (sessionInfo != null)
                {
                    lock (_sessionsCache)
                    {
                        _sessionsCache[sessionToken] = sessionInfo;
                    }
                }
            }
            return sessionInfo;
        }

        public AuthenticationResponse Login(AuthenticationRequest authenticationRequest)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CardStorageServiceDbContext>();

            var account = !string.IsNullOrWhiteSpace(authenticationRequest.Login)
                ? FindAccountByLogin(authenticationRequest.Login, context)
                : null;

            if (account == null)
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.UserNotFound
                };
            }

            if (!PasswordUtils.VerifyPassword(authenticationRequest.Password, account.PasswordSalt, account.PasswordHash))
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.InvalidPassword
                };
            }

            var session = new AccountSession
            {
                AccountId = account.AccountId,
                SessionToken = CreateSessionToken(account),
                TimeCreated = DateTime.UtcNow,
                IsClosed = false
            };

            context.AccountSessions.Add(session);
            context.SaveChanges();

            var sessionInfo = GetSessionInfo(account, session);

            lock (_sessionsCache)
            {
                _sessionsCache[sessionInfo.SessionToken] = sessionInfo; 
            }

            return new AuthenticationResponse
            {
                Status = AuthenticationStatus.Success,
                SessionInfo = sessionInfo
            };
        }

        private Account FindAccountByLogin(string login, CardStorageServiceDbContext context)
        {
            return context.Accounts.FirstOrDefault(acc => acc.EMail == login);
        }

        private string CreateSessionToken(Account account)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
            byte[] key = Encoding.ASCII.GetBytes(SecretCode);

            SecurityTokenDescriptor securityTokenDescriptor = new()
            {
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Email, account.EMail)
                })
            };

            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }

        private SessionInfo GetSessionInfo(Account account, AccountSession accountSession)
        {
            return new SessionInfo
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.AccountId,
                    EMail = account.EMail,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondName = account.SecondName,
                    Locked = account.Locked
                }
            };
        }
    }
}
