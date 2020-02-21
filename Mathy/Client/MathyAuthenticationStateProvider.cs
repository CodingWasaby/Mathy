using Mathy.Shared;
using Mathy.Shared.Entity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mathy.Client
{
    public class MathyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient http;
        private readonly LocalStorage localStorage;
        public MathyAuthenticationStateProvider(HttpClient http, LocalStorage localStorage)
        {
            this.http = http;
            this.localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await localStorage.GetAsync<User>("User");
            if (user == null)
            {
                var identity = new ClaimsIdentity();
                var cp = new ClaimsPrincipal(identity);
                return new AuthenticationState(cp);
            }
            else
            {
                var result = await http.PostJsonAsync<ResponseResult<User>>("User/" + user.Email + "/" + user.Password, null);
                await localStorage.SetAsync("User", result.Data);
                var identity = result.Code == 0 ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, result.Data.Name) }, "all") : new ClaimsIdentity();
                var cp = new ClaimsPrincipal(identity);
                http.DefaultRequestHeaders.Add("UesToken", "test");
                return new AuthenticationState(cp);
            }
        }

        public async Task NotifyAuthenticationState(User user = null)
        {
            if (user == null)
                await localStorage.DeleteAsync("User");
            else
                await localStorage.SetAsync("User", user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
