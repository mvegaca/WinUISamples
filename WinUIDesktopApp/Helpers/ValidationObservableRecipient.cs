using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Data;

namespace WinUIDesktopApp.Helpers
{
    public class ValidationObservableRecipient : ObservableRecipient, INotifyDataErrorInfo
    {
        private Dictionary<string, List<ValidationResult>> _errors = new Dictionary<string, List<ValidationResult>>();

        public bool HasErrors => _errors.Any();

        public IEnumerable<object> GetErrors(string propertyName)
            => _errors[propertyName];

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void SetAndValidate<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            var result = SetProperty(ref currentValue, newValue, propertyName);
            if (result)
            {
                ClearErrors(propertyName);
                var validationResults = new List<ValidationResult>();
                var validationResult = Validator.TryValidateProperty(newValue, new ValidationContext(this, null, null) { MemberName = propertyName }, validationResults);

                if (!validationResult)
                {
                    AddErrors(propertyName, validationResults);
                }
            }
        }

        private void AddErrors(string propertyName, IEnumerable<ValidationResult> validationResults)
        {
            if (_errors.TryGetValue(propertyName, out var propertyErrors))
            {
                propertyErrors.AddRange(validationResults);
            }
            else
            {
                _errors.Add(propertyName, new List<ValidationResult>(validationResults));
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            if (_errors.TryGetValue(propertyName, out var properyErrors))
            {
                properyErrors.Clear();
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
