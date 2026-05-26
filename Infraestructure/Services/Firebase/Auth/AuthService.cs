using Aplication.Interfaces.Firebase.Authentication;
using Firebase.Auth;
using Firebase.Auth.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Services.Firebase.Auth
{

    public class AuthService : IAuthService
    {

        private readonly FirebaseAuthClient _client;
        public AuthService()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyBwF2wZSchCkmVCWyXlY8w0To8UBxj_lew",
                AuthDomain = "gestoralimentacionzoo.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };
            _client = new FirebaseAuthClient(config);
        }
        public async Task resetEmailPasswordAsync(string email)
        {
            try
            {
                await _client.ResetEmailPasswordAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw;
            }
        }
        public async Task<string> loginAsync(string email, string password)
        {
            var user = await _client.SignInWithEmailAndPasswordAsync(email, password);
            return user.User.Uid;
        }
    }
}

