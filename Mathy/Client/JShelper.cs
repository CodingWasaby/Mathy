using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mathy.Client
{
    public class JShelper
    {
        private readonly IJSRuntime JS;
        public JShelper(IJSRuntime js)
        {
            JS = js;
        }

        public async Task Alert(string msg)
        {
            await JS.InvokeVoidAsync("AlertMessage", msg);
        }

        public async Task Log(object obj)
        {
            await JS.InvokeVoidAsync("console.log", obj);
        }

        public async Task ClickButton(string id)
        {
            await JS.InvokeVoidAsync("ClickButton", id);
        }

        public async Task ClearContent(string id)
        {
            await JS.InvokeVoidAsync("ClearContent", id);
        }

        public async Task InitEditor(string content)
        {
            await JS.InvokeVoidAsync("InitEditor", content);
        }
    }


}
