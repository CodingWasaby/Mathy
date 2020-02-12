using System;
using System.Collections.Generic;
using System.Text;

namespace Mathy.Shared
{
    public class ResponseResult<TData>
    {
        public ResponseResult()
        {

        }

        public ResponseResult(int code, string messge, TData data)
        {
            this.Code = code;
            this.Message = messge;
            this.Data = data;
        }

        public int Code { get; set; }        
        public string Message { get; set; }
        public TData Data { get; set; }
    }
}
