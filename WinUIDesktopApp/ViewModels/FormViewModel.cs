using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.Input;
using WinUIDesktopApp.Core.Contracts.Services;
using WinUIDesktopApp.Core.Models;
using WinUIDesktopApp.Helpers;

namespace WinUIDesktopApp.ViewModels
{
    public class FormViewModel : ValidationObservableRecipient
    {
        private readonly ISampleDataService _sampleDataService;
        private long _orderID = new Random().Next(11000, 11500);
        private DateTimeOffset _orderDate = DateTime.Now;
        private DateTimeOffset _requiredDate = DateTime.Now;
        private DateTimeOffset _shippedDate = DateTime.Now;
        private TimeSpan _orderTime = DateTime.Now.TimeOfDay;
        private TimeSpan _requiredTime = DateTime.Now.TimeOfDay;
        private TimeSpan _shippedTime = DateTime.Now.TimeOfDay;

        private string _shipperName;
        private string _shipperPhone;
        private double _freight;
        private string _company;
        private string _shipTo;
        private double _orderTotal;
        private string _status = "Shipped";
        private ICommand _saveOrderCommand;

        public long OrderID
        {
            get { return _orderID; }
            set { _orderID = value; }
        }

        [Required]
        public DateTimeOffset OrderDate
        {
            get { return _orderDate; }
            set { _orderDate = value; }
        }

        [Required]
        public TimeSpan OrderTime
        {
            get { return _orderTime; }
            set { _orderTime = value; }
        }

        [Required]
        public DateTimeOffset RequiredDate
        {
            get { return _requiredDate; }
            set { _requiredDate = value; }
        }

        [Required]
        public TimeSpan RequiredTime
        {
            get { return _requiredTime; }
            set { _requiredTime = value; }
        }

        [Required]
        public DateTimeOffset ShippedDate
        {
            get { return _shippedDate; }
            set { _shippedDate = value; }
        }

        [Required]
        public TimeSpan ShippedTime
        {
            get { return _shippedTime; }
            set { _shippedTime = value; }
        }

        [Required]
        public string ShipperName
        {
            get { return _shipperName; }
            set { SetAndValidate(ref _shipperName, value); }
        }

        [Required]
        [Phone]
        public string ShipperPhone
        {
            get { return _shipperPhone; }
            set { SetAndValidate(ref _shipperPhone, value); }
        }

        [CustomValidation(typeof(FormViewModel), "ValidateFreight")]
        public double Freight
        {
            get { return _freight; }
            set { SetAndValidate(ref _freight, value); }
        }

        [Required]
        public string Company
        {
            get { return _company; }
            set { SetAndValidate(ref _company, value); }
        }

        [Required]
        public string ShipTo
        {
            get { return _shipTo; }
            set { SetAndValidate(ref _shipTo, value); }
        }

        public double OrderTotal
        {
            get { return _orderTotal; }
            set { _orderTotal = value; }
        }

        [Required]
        public string Status
        {
            get { return _status; }
            set { SetAndValidate(ref _status, value); }
        }

        public IEnumerable<string> StatusValues { get; } = new List<string>()
        {
            "Shipped",
            "Closed"
        };

        public ICommand SaveOrderCommand => _saveOrderCommand ?? (_saveOrderCommand = new RelayCommand(SaveOrder));

        public FormViewModel(ISampleDataService sampleDataService)
        {
            _sampleDataService = sampleDataService;
            Status = StatusValues.First();
        }

        private void SaveOrder()
        {
            ValidateProperties(new Dictionary<string, object>()
            {
                { nameof(ShipperName), ShipperName },
                { nameof(ShipperPhone), ShipperPhone },
                { nameof(Freight), Freight },
                { nameof(Company), Company },
                { nameof(ShipTo), ShipTo }
            });

            if (HasErrors)
            {
                return;
            }

            _sampleDataService.GetSampleOrderAsync(new SampleOrder()
            {
                OrderID = OrderID,
                OrderDate = new DateTime(OrderDate.Year, OrderDate.Month, OrderDate.Day, OrderTime.Hours, OrderTime.Minutes, OrderTime.Seconds),
                RequiredDate = new DateTime(RequiredDate.Year, RequiredDate.Month, RequiredDate.Day, RequiredTime.Hours, RequiredTime.Minutes, RequiredTime.Seconds),
                ShippedDate = new DateTime(ShippedDate.Year, ShippedDate.Month, ShippedDate.Day, ShippedTime.Hours, ShippedTime.Minutes, ShippedTime.Seconds),
                ShipperName = ShipperName,
                ShipperPhone = ShipperPhone,
                Freight = Freight,
                Company = Company,
                ShipTo = ShipTo,
                OrderTotal = OrderTotal,
                Status = Status
            });

            OrderID = new Random().Next(11000, 11500);
            OrderDate = DateTime.Now;
            RequiredDate = DateTime.Now;
            ShippedDate = DateTime.Now;
            ShipperName = string.Empty;
            ShipperPhone = string.Empty;
            Freight = 0;
            Company = string.Empty;
            ShipTo = string.Empty;
            OrderTotal = 0;
            Status = StatusValues.First();
            ClearErrors();
        }

        public static ValidationResult ValidateFreight(double freight)
        {
            if (freight > 0)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(
                    "Freight must be greater than zero");
            }
        }
    }
}
