namespace PixelParbaj_CORE.Code
{
    public class SessionManager
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            
            _httpContextAccessor = httpContextAccessor;
        }

        public void StartSession(string email)
        {
            _httpContextAccessor.HttpContext.Session.SetString("user", email);
        }

        public bool IsSessionActive()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("user") != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCurrentSession()
        {
            if (_httpContextAccessor.HttpContext.Session.GetString("user") != null)
            {
                return _httpContextAccessor.HttpContext.Session.GetString("user");
            }
            else
            {
                return null;
            }
        }

        public void TerminateSession()
        {
            try
            {
                //Terminate the Session
                _httpContextAccessor.HttpContext.Session.Clear();
            }
            catch (Exception)
            {
                //
            }
        }
    }
}
