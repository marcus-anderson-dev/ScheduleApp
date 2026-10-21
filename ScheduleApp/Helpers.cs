using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ScheduleApp
{

    public static class TimeZoneHelperFunction
    {
        private static TimeZoneInfo eastern;

        public static TimeZoneInfo Eastern
        {
            get
            {
                if (eastern == null)
                {
                    try { eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); }
                    catch (TimeZoneNotFoundException) { eastern = TimeZoneInfo.FindSystemTimeZoneById("America/New_York"); }
                }
                return eastern;
            }
        }


        public static DateTime ToUtc(DateTime localTime)
        {
            DateTime unspecified = DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecified, TimeZoneInfo.Local);
        }

        // Users local time.
        public static DateTime ToLocal(DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), TimeZoneInfo.Local);
        }

        public static DateTime ToEastern(DateTime utc)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Eastern);
        }

        public static bool IsWithinBusinessHours(DateTime startUtc, DateTime endUtc)
        {
            DateTime start = ToEastern(startUtc);
            DateTime end = ToEastern(endUtc);

            if (start.Date != end.Date) return false;
            if (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday) return false;

            return start.TimeOfDay >= new TimeSpan(9, 0, 0) && end.TimeOfDay <= new TimeSpan(17, 0, 0);
        }
    }

    public static class Lang
    {
        public static bool Spanish;

        // Dictionary that handles text English/Spanish
        private static readonly Dictionary<string, string[]> text = new Dictionary<string, string[]>
        {
            { "title",         new[] { "Scheduling Software - Log In", "Software de Citas - Iniciar Sesión" } },
            { "username",      new[] { "Username", "Usuario" } },
            { "password",      new[] { "Password", "Contraseña" } },
            { "login",         new[] { "Log In", "Iniciar sesión" } },
            { "exit",          new[] { "Exit", "Salir" } },
            { "location",      new[] { "Location:", "Ubicación:" } },
            { "error",         new[] { "Error", "Error" } },
            { "errEmpty",      new[] { "Please enter a username and a password.", "Por favor, ingrese un usuario y una contraseña." } },
            { "errMismatch",   new[] { "The username and password do not match.", "El usuario y la contraseña no coinciden." } },
            { "errDb",         new[] { "Could not connect to the database.", "No se pudo conectar con la base de datos." } },
            { "errUnexpected", new[] { "An unexpected error occurred.", "Ocurrió un error inesperado." } },
            { "errLog",        new[] { "The login could not be written to Login_History.txt.", "No se pudo escribir el inicio de sesión en Login_History.txt." } },
            { "reminder",      new[] { "Appointment Reminder", "Recordatorio de Cita" } },
            { "alertBody",     new[] { "You have an appointment within the next 15 minutes:", "Tiene una cita en los próximos 15 minutos:" } }
        };

        public static string T(string key)
        {
            return text[key][Spanish ? 1 : 0];
        }
    }

    public static class UI
    {
        // Sets the heading of a table column.
        public static void SetHeader(DataGridView grid, string columnName, string header)
        {
            if (grid.Columns[columnName] != null) grid.Columns[columnName].HeaderText = header;
        }
    }
}
