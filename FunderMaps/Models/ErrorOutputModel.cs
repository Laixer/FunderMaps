using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FunderMaps.Models
{
    public class ErrorOutputModel
    {
        public class Error
        {
            public int Code { get; set; }
            public string Message { get; set; }
        }

        public IList<Error> Errors { get; set; }

        public ErrorOutputModel()
        {
        }

        public ErrorOutputModel(int code, string message)
        {
            AddError(code, message);
        }

        public void AddError(int code, string message)
        {
            if (Errors == null)
            {
                Errors = new List<Error>();
            }

            Errors.Add(new Error { Code = code, Message = message });
        }

        public void AddError(int code)
        {
            AddError(code, null);
        }
    }
}
