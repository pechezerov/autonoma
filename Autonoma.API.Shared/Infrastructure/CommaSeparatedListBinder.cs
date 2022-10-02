using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autonoma.API.Shared.Infrastructure
{
    public class CommaSeparatedListBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string firstValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (string.IsNullOrWhiteSpace(firstValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            try
            {
                List<T> model = firstValue.Split(",").Select(new Func<string, object>(ParseValue)).Cast<T>()
                    .ToList();
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (Exception ex)
            {
                if (!(ex is ArgumentException) && !(ex is FormatException) && !(ex is OverflowException))
                    throw;

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Could not parse list of values");
                bindingContext.Result = ModelBindingResult.Failed();
            }
            return Task.CompletedTask;
        }

        private static object ParseValue(string stringValue)
        {
            stringValue = stringValue.Trim();
            Type typeFromHandle = typeof(T);
            if (typeFromHandle.IsEnum)
            {
                return ParseEnum(stringValue);
            }
            if (typeFromHandle == typeof(int))
            {
                return int.Parse(stringValue);
            }
            if (typeFromHandle == typeof(long))
            {
                return long.Parse(stringValue);
            }
            if (typeFromHandle == typeof(string))
            {
                return stringValue;
            }
            throw new NotSupportedException("Type is not supported");
        }

        private static object ParseEnum(string stringValue)
        {
            object obj = Enum.Parse(typeof(T), stringValue);
            if (!Enum.IsDefined(typeof(T), obj))
            {
                throw new ArgumentException("Enum value is not defined");
            }
            return obj;
        }
    }
}
