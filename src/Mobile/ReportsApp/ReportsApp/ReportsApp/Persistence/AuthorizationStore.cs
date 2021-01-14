using Ardalis.GuardClauses;
using ReportsApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ReportsApp.Persistence
{
    public class AuthorizationStore : IAuthorizationStore
    {
        public async Task<string> GetAuthorizationToken()
        {
            try
            {
                var token = await SecureStorage.GetAsync(AuthorizationConstants.AuthorizatioTokenKey);
                if (token != null) return token;

                return default;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task SetAuthorizationInfo(string token)
        {
            try
            {
                Guard.Against.NullOrWhiteSpace(token, nameof(token));
                await SecureStorage.SetAsync(AuthorizationConstants.AuthorizatioTokenKey, token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task SetAuthorizationInfo(UserInfo activeUser)
        {
            try
            {
                Guard.Against.Null(activeUser, nameof(activeUser));
                await SecureStorage.SetAsync(AuthorizationConstants.UserInfoKey, JsonSerializer.Serialize(activeUser));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public void RemoveAuthorizationInfo()
        {
            try
            {
                SecureStorage.Remove(AuthorizationConstants.UserInfoKey);
                SecureStorage.Remove(AuthorizationConstants.AuthorizatioTokenKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }

    public class AuthorizationConstants
    {
        public const string AuthorizatioTokenKey = "token";
        public const string UserInfoKey = "user";
    }
}
