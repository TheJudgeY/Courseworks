﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Validators
{
    public class Result<T>
    {
        public bool IsSuccessful { get; }
        public T Data { get; }
        public string Message { get; }

        public Result(bool isSuccessful, T data)
        {
            IsSuccessful = isSuccessful;
            Data = data;
        }

        public Result(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }

        public Result(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}
