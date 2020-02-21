using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mathy.Client
{
    public class LocalStorage
    {
        private IJSRuntime jsRuntime;
        public LocalStorage(IJSRuntime js)
        {
            jsRuntime = js;
        }
        public ValueTask<T> GetAsync<T>(string key)
           => jsRuntime.InvokeAsync<T>("blazorLocalStorage.get", key);

        public ValueTask SetAsync(string key, object value)
            => jsRuntime.InvokeVoidAsync("blazorLocalStorage.set", key, value);

        public ValueTask DeleteAsync(string key)
            => jsRuntime.InvokeVoidAsync("blazorLocalStorage.delete", key);
    }
}

