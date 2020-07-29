using System.ComponentModel.DataAnnotations;
using System.Linq;
using WinUIDesktopApp.Helpers;

namespace WinUIDesktopApp.ViewModels
{
    public class FormViewModel : ValidationObservableRecipient
    {
        private string _name;
        private string _surname;
        private string _address;
        private string _city;
        private string _state;
        private string _zipCode;

        [System.ComponentModel.DefaultValue("")]
        [MinLength(5, ErrorMessage = "Name must be more than 4 characters")]
        public string Name
        {
            get { return _name; }
            set { SetAndValidate(ref _name, value); }
        }

        [System.ComponentModel.DefaultValue("")]
        [MinLength(5, ErrorMessage = "Surname must be more than 4 characters")]
        public string Surname
        {
            get { return _surname; }
            set { SetAndValidate(ref _surname, value); }
        }

        [System.ComponentModel.DefaultValue("")]
        [MinLength(1, ErrorMessage = "Address can not be empty")]
        public string Address
        {
            get { return _address; }
            set { SetAndValidate(ref _address, value); }
        }

        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        [CustomValidation(typeof(FormViewModel), "ValidateZipCode")]
        public string ZipCode
        {
            get { return _zipCode; }
            set { SetAndValidate(ref _zipCode, value); }
        }

        public FormViewModel()
        {
        }

        public static ValidationResult ValidateZipCode(string zipCode)
        {
            if (!zipCode.Any(x => char.IsLetter(x)))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "Zip code must contain numbers only");
            }
        }
    }
}
