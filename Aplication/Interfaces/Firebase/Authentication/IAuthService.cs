using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Firebase.Authentication
{
    public interface IAuthService
    {
        public Task resetEmailPasswordAsync(string email);
        public Task<string> loginAsync(string email, string password);
    }
}
