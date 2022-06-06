using Autonoma.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Autonoma.API.Commands.Validation
{
    public class ValidationResult
    {
        public bool IsValid { get; private set; }

        public List<ValidationError> Errors { get; set; }

        public ValidationResult()
        {
            IsValid = true;
            Errors = new List<ValidationError>();
        }

        public void AddValidationError(string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            IsValid = false;

            Errors.Add(new ValidationError { ErrorMessage = message });

        }

        public void AddRelatedEntityIdError(string dtoField, Guid fieldId)
        {
            if (string.IsNullOrEmpty(dtoField))
                throw new ArgumentNullException(nameof(dtoField));

            IsValid = false;

            Errors.Add(new ValidationError
            {
                Field = dtoField,
                ErrorMessage = $"Отсутствует экземпляр сущности с внешним идентификатором {fieldId}"
            });

        }

        public void AddApiValidationExternalIdError(string dtoField, string fieldExteralId)
        {
            if (string.IsNullOrEmpty(dtoField))
                throw new ArgumentNullException(nameof(dtoField));

            if (string.IsNullOrEmpty(fieldExteralId))
                throw new ArgumentNullException(nameof(fieldExteralId));

            IsValid = false;

            Errors.Add(new ValidationError
            {
                Field = dtoField,
                ErrorMessage = $"Отсутствует экземпляр сущности с внешним идентификатором {fieldExteralId}"
            });

        }

        public void AddRequiredFieldError(string field)
        {
            if (string.IsNullOrEmpty(field))
                throw new ArgumentNullException(nameof(field));

            IsValid = false;

            Errors.Add(new ValidationError { ErrorMessage = $"Поле {field} должно быть заполнено" });
        }


        public string GetValidationErrors()
        {
            if (Errors.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();

            foreach (var error in Errors.OrderBy(e => e.Field))
            {
                if (!string.IsNullOrEmpty(error.Field))
                    sb.AppendLine($"Поле {error.Field}. {error.ErrorMessage}");
                else
                    sb.AppendLine($"{error.ErrorMessage}");
            }

            return sb.ToString();
        }

        public string GetValiationErrorsHtml()
        {
            if (Errors.Count == 0)
                return null;

            StringBuilder sb = new StringBuilder();

            foreach (var error in Errors.OrderBy(e => e.Field))
            {
                if (!string.IsNullOrEmpty(error.Field))
                    sb.AppendLine($"Поле {error.Field}. {error.ErrorMessage}");
                else
                    sb.AppendLine($"{error.ErrorMessage}");
            }

            return HtmlHelper.ConvertNewLinesToHtmlLineBreaks(sb.ToString());
        }
    }
}
