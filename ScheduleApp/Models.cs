using System;
using System.ComponentModel;

namespace ScheduleApp
{

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message) { }
    }
    // Static Session class: Tracks user login
    public static class Session
    {
        public static int UserId;
        public static string UserName;
    }

    public class Customer
    {
        [Browsable(false)] public int CustomerId { get; set; }
        [Browsable(false)] public int AddressId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
    }

    public class Appointment
    {
        [Browsable(false)] public int AppointmentId { get; set; }
        [Browsable(false)] public int CustomerId { get; set; }
        [Browsable(false)] public int UserId { get; set; }
        [Browsable(false)] public DateTime StartUtc { get; set; }
        [Browsable(false)] public DateTime EndUtc { get; set; }

        [Browsable(false)] public string Description { get; set; }
        [Browsable(false)] public string Location { get; set; }
        [Browsable(false)] public string Contact { get; set; }
        [Browsable(false)] public string Url { get; set; }

        public string CustomerName { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

        // Converts timezones to user timezones
        public DateTime StartLocal { get { return TimeZoneHelperFunction.ToLocal(StartUtc); } }
        public DateTime EndLocal { get { return TimeZoneHelperFunction.ToLocal(EndUtc); } }
    }
}
