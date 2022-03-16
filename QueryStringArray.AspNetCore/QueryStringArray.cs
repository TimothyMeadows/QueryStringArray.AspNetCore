using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using QueryStringArray.AspNetCore.Options;

namespace QueryStringArray.AspNetCore
{
    public sealed class QueryStringArray : IModelBinder
    {
        private readonly QueryStringArrayOptions _options;

        public QueryStringArray(IOptions<QueryStringArrayOptions> options)
        {
            _options = options?.Value ?? new QueryStringArrayOptions();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            {
                var content = value.FirstValue;
                if (string.IsNullOrEmpty(content) || !(content.StartsWith(_options.OpenTag) && content.EndsWith(_options.CloseTag)))
                {
                    bindingContext.Model =
                        Array.CreateInstance((bindingContext.ModelType.GetElementType() ?? default)!, 0);
                    return Task.CompletedTask;
                }

                var type = bindingContext.ModelType.GetElementType();
                var converter = TypeDescriptor.GetConverter(type!);
                var values = Array.ConvertAll(
                    content.Substring(1, content.Length - 2)
                        .Split(new[] { _options .Separator }, StringSplitOptions.RemoveEmptyEntries),
                    x => converter.ConvertFromString(x?.Trim()));

                var typedValues = Array.CreateInstance(type, values.Length);
                values.CopyTo(typedValues, 0);

                bindingContext.Result = ModelBindingResult.Success(typedValues);
                return Task.CompletedTask;
            }
        }
    }
}
