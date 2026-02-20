using tl2_tp8_2025_michdeaver.Interfaces;

namespace tl2_tp8_2025_michdeaver.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        private readonly IUserRepository _userRepo;
        private readonly IHttpContextAccessor _httpContextAccesor;

        public AuthenticationServices(IUserRepository userRepo, IHttpContextAccessor http)
        {
            _userRepo = userRepo;
            _httpContextAccesor = http;
        }
        public bool Login(string username, string password)
        {
            var context = _httpContextAccesor.HttpContext;
            var user = _userRepo.GetUser(username, password);
            if(user is not null)
            {
                if(context is null)
                    throw new InvalidOperationException("HttpContext no esta disponible.");
                
                context.Session.SetString("IsAuthenticated", "true");
                context.Session.SetString("Username", user.Username);                
                context.Session.SetString("Nombre", user.Nombre);
                context.Session.SetString("Rol", user.Rol);

                return true;
            }

            return false;
        }
        public void Logout()
        {
            var context = _httpContextAccesor.HttpContext;

            if(context is null)
                throw new InvalidOperationException("HttpContext no esta disponible.");

            context.Session.Clear();
        }
        public bool IsAuthenticated()
        {
            var context = _httpContextAccesor.HttpContext;

            if(context is null)
                throw new InvalidOperationException("HttpContext no esta disponible.");

            return context.Session.GetString("IsAuthenticated") == "true";
        }
        public bool HasAccessLevel(string requiredAccessLevel)
        {
            var context = _httpContextAccesor.HttpContext;

            if(context is null)
                throw new InvalidOperationException("HttpContext no esta disponible.");

            return context.Session.GetString("Rol") == requiredAccessLevel;
        }
    }
}