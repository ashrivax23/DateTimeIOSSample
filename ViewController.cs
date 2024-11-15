using System;
using System.Collections.Generic;
using System.Globalization;
using UIKit;
using Foundation;

namespace DateTimeFormatIOSTest
{
    public class ViewController : UIViewController
    {
        private UIDatePicker datePicker;
        private UILabel localeLabel;
        private UILabel dateFormatLabel;
        private UILabel calendarLabel;
        private NSObject? willEnterForegroundNotification;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.BackgroundColor = UIColor.White;

            SetupUI();
            SubscribeToForegroundNotification();
        }

        private void SetupUI()
        {
            // Create and configure labels
            localeLabel = new UILabel
            {
                Frame = new CoreGraphics.CGRect(20, 50, View.Frame.Width - 40, 30),
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.SystemFontOfSize(14)
            };

            dateFormatLabel = new UILabel
            {
                Frame = new CoreGraphics.CGRect(20, 90, View.Frame.Width - 40, 30),
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.SystemFontOfSize(14)
            };

            calendarLabel = new UILabel
            {
                Frame = new CoreGraphics.CGRect(20, 130, View.Frame.Width - 40, 30),
                TextAlignment = UITextAlignment.Left,
                Font = UIFont.SystemFontOfSize(14)
            };

            // Create and configure date picker
            datePicker = new UIDatePicker();
            datePicker.Frame = new CoreGraphics.CGRect(20, 180, View.Frame.Width - 40, 200);
            datePicker.PreferredDatePickerStyle = UIDatePickerStyle.Wheels;
            datePicker.Mode = UIDatePickerMode.Date;

            // Add event handler for date picker value changes
            datePicker.ValueChanged += (sender, e) =>
            {
                var selectedDate = datePicker.Date;
                Console.WriteLine($"Selected date: {selectedDate}");
            };

            // Add all views to the main view
            View.AddSubview(localeLabel);
            View.AddSubview(dateFormatLabel);
            View.AddSubview(calendarLabel);
            View.AddSubview(datePicker);

            // Initial update of labels
            UpdateLabels();
        }

        private void SubscribeToForegroundNotification()
        {
            // Subscribe to willEnterForeground notification
            willEnterForegroundNotification = UIApplication.Notifications.ObserveWillEnterForeground((sender, args) =>
            {
                Console.WriteLine("App will enter foreground - updating labels");
                UpdateLabels();
            });
        }

        private void UpdateLabels()
        {
            try
            {
                var currentLocale = NSLocale.CurrentLocale;
                var deviceCalendar = NSCalendar.CurrentCalendar;

                if (NSLocale.PreferredLanguages.Length > 0)
                {
                    var language = NSLocale.PreferredLanguages[0];
                    var culture = CultureInfo.GetCultureInfo(language);
                    datePicker.Locale = new NSLocale(culture.Name);

                    var dateFormatter = new NSDateFormatter
                    {
                        Locale = datePicker.Locale,
                        DateStyle = NSDateFormatterStyle.Medium
                    };

                    // Update UI on main thread
                    InvokeOnMainThread(() =>
                    {
                        localeLabel.Text = $"Current Locale: {culture.Name}";
                        dateFormatLabel.Text = $"Date Format: {dateFormatter.DateFormat}";
                        calendarLabel.Text = $"Calendar: {deviceCalendar.Identifier}";
                    });

                    // Log to console for debugging
                    Console.WriteLine($"Updated - Current locale: {culture.Name}");
                    Console.WriteLine($"Updated - Date format: {dateFormatter.DateFormat}");
                    Console.WriteLine($"Updated - Calendar: {deviceCalendar.Identifier}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating labels: {ex.Message}");
                InvokeOnMainThread(() =>
                {
                    localeLabel.Text = "Error updating locale information";
                });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Unsubscribe from the notification when the view controller is disposed
                willEnterForegroundNotification?.Dispose();
                willEnterForegroundNotification = null;
            }
            base.Dispose(disposing);
        }
    }
}