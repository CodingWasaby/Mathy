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
        private readonly IJSRuntime js;
        public MathyAuthenticationStateProvider(HttpClient http, IJSRuntime JS)
        {
            this.http = http;
            js = JS;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = await LocalStorage.GetAsync<User>(js, "User");
            if (user == null)
            {
                var identity = new ClaimsIdentity();
                var cp = new ClaimsPrincipal(identity);
                return new AuthenticationState(cp);
            }
            else
            {
                var result = await http.PostJsonAsync<ResponseResult<User>>("User/" + user.Email + "/" + user.Password, null);
                await LocalStorage.SetAsync(js, "User", result.Data);
                var identity = result.Code == 0 ? new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, result.Data.Name) }, "all") : new ClaimsIdentity();
                var cp = new ClaimsPrincipal(identity);
                return new AuthenticationState(cp);
            }
        }

        public async Task NotifyAuthenticationState(User user = null)
        {
            if (user == null)
                await LocalStorage.DeleteAsync(js, "User");
            else
                await LocalStorage.SetAsync(js, "User", user);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
