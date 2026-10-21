using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public static class Db
    {
        // Connection Values.
        public const string ConnString = "server=127.0.0.1;port=3306;database=client_schedule;uid=sqlUser;pwd=Passw0rd!";

        public static MySqlConnection Open()
        {
            MySqlConnection conn = new MySqlConnection(ConnString);
            conn.Open();
            return conn;
        }

        public static MySqlCommand Cmd(string sql, MySqlConnection conn, MySqlTransaction tx = null)
        {
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Transaction = tx;
            return cmd;
        }

        public static int NextId(MySqlConnection conn, MySqlTransaction tx, string table, string idColumn)
        {
            using (MySqlCommand cmd = Cmd("SELECT IFNULL(MAX(" + idColumn + "), 0) + 1 FROM `" + table + "`", conn, tx))
            {
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    public static class UserRepo
    {
        // Returns userID if matching username + password, else null.
        public static int? Login(string userName, string password)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlCommand cmd = Db.Cmd("SELECT userId, userName, password FROM `user` WHERE userName = @u AND active = 1", conn))
            {
                cmd.Parameters.AddWithValue("@u", userName);
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        if (r["userName"].ToString() == userName && r["password"].ToString() == password)
                        {
                            return Convert.ToInt32(r["userId"]);
                        }
                    }
                }
            }
            return null;
        }
    }

    public static class CustomerRepo
    {
        public static List<Customer> GetAll()
        {
            List<Customer> list = new List<Customer>();
            string sql =
                "SELECT c.customerId, c.customerName, c.active, a.addressId, a.address, a.address2, a.postalCode, a.phone, ci.city, co.country " +
                "FROM customer c " +
                "JOIN address a ON c.addressId = a.addressId " +
                "JOIN city ci ON a.cityId = ci.cityId " +
                "JOIN country co ON ci.countryId = co.countryId " +
                "ORDER BY c.customerName";

            using (MySqlConnection conn = Db.Open())
            using (MySqlCommand cmd = Db.Cmd(sql, conn))
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Customer
                    {
                        CustomerId = Convert.ToInt32(r["customerId"]),
                        AddressId = Convert.ToInt32(r["addressId"]),
                        Name = r["customerName"].ToString(),
                        Address = r["address"].ToString(),
                        Address2 = r["address2"].ToString(),
                        City = r["city"].ToString(),
                        Country = r["country"].ToString(),
                        PostalCode = r["postalCode"].ToString(),
                        Phone = r["phone"].ToString(),
                        Active = r["active"] != DBNull.Value && Convert.ToBoolean(r["active"])
                    });
                }
            }
            return list;
        }

        public static bool IsDuplicateCustomer(Customer c)
        {
            return GetAll().Any(x =>
                x.CustomerId != c.CustomerId &&
                string.Equals(x.Name, c.Name, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(x.Address, c.Address, StringComparison.OrdinalIgnoreCase) &&
                x.Phone == c.Phone);
        }

        public static void Add(Customer c, string user)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    int countryId = FindOrCreateCountry(conn, tx, c.Country, user);
                    int cityId = FindOrCreateCity(conn, tx, c.City, countryId, user);

                    // Customer points to an address.  The address row is then created first.
                    int addressId = Db.NextId(conn, tx, "address", "addressId");
                    using (MySqlCommand cmd = Db.Cmd(
                        "INSERT INTO address (addressId, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                        "VALUES (@id, @address, @address2, @cityId, @postal, @phone, UTC_TIMESTAMP(), @user, UTC_TIMESTAMP(), @user)", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@id", addressId);
                        cmd.Parameters.AddWithValue("@address", c.Address);
                        cmd.Parameters.AddWithValue("@address2", c.Address2);
                        cmd.Parameters.AddWithValue("@cityId", cityId);
                        cmd.Parameters.AddWithValue("@postal", c.PostalCode);
                        cmd.Parameters.AddWithValue("@phone", c.Phone);
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.ExecuteNonQuery();
                    }

                    int customerId = Db.NextId(conn, tx, "customer", "customerId");
                    using (MySqlCommand cmd = Db.Cmd(
                        "INSERT INTO customer (customerId, customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                        "VALUES (@id, @name, @addressId, @active, UTC_TIMESTAMP(), @user, UTC_TIMESTAMP(), @user)", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@id", customerId);
                        cmd.Parameters.AddWithValue("@name", c.Name);
                        cmd.Parameters.AddWithValue("@addressId", addressId);
                        cmd.Parameters.AddWithValue("@active", c.Active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();   // Transaction that saves all customer information or none of it.
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public static void Update(Customer c, string user)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    int countryId = FindOrCreateCountry(conn, tx, c.Country, user);
                    int cityId = FindOrCreateCity(conn, tx, c.City, countryId, user);

                    using (MySqlCommand cmd = Db.Cmd(
                        "UPDATE address SET address = @address, address2 = @address2, cityId = @cityId, postalCode = @postal, phone = @phone, " +
                        "lastUpdate = UTC_TIMESTAMP(), lastUpdateBy = @user WHERE addressId = @addressId", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@address", c.Address);
                        cmd.Parameters.AddWithValue("@address2", c.Address2);
                        cmd.Parameters.AddWithValue("@cityId", cityId);
                        cmd.Parameters.AddWithValue("@postal", c.PostalCode);
                        cmd.Parameters.AddWithValue("@phone", c.Phone);
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@addressId", c.AddressId);
                        cmd.ExecuteNonQuery();
                    }

                    using (MySqlCommand cmd = Db.Cmd(
                        "UPDATE customer SET customerName = @name, active = @active, lastUpdate = UTC_TIMESTAMP(), lastUpdateBy = @user " +
                        "WHERE customerId = @customerId", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@name", c.Name);
                        cmd.Parameters.AddWithValue("@active", c.Active ? 1 : 0);
                        cmd.Parameters.AddWithValue("@user", user);
                        cmd.Parameters.AddWithValue("@customerId", c.CustomerId);
                        cmd.ExecuteNonQuery();
                    }

                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public static void Delete(Customer c)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlTransaction tx = conn.BeginTransaction())
            {
                try
                {

                    using (MySqlCommand cmd = Db.Cmd("DELETE FROM appointment WHERE customerId = @id", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@id", c.CustomerId);
                        cmd.ExecuteNonQuery();
                    }
                    using (MySqlCommand cmd = Db.Cmd("DELETE FROM customer WHERE customerId = @id", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@id", c.CustomerId);
                        cmd.ExecuteNonQuery();
                    }
                    using (MySqlCommand cmd = Db.Cmd(
                        "DELETE FROM address WHERE addressId = @id AND NOT EXISTS (SELECT 1 FROM customer WHERE addressId = @id)", conn, tx))
                    {
                        cmd.Parameters.AddWithValue("@id", c.AddressId);
                        cmd.ExecuteNonQuery();
                    }
                    tx.Commit();
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        private static int FindOrCreateCountry(MySqlConnection conn, MySqlTransaction tx, string country, string user)
        {
            using (MySqlCommand find = Db.Cmd("SELECT countryId FROM country WHERE country = @name", conn, tx))
            {
                find.Parameters.AddWithValue("@name", country);
                object found = find.ExecuteScalar();
                if (found != null) return Convert.ToInt32(found);
            }

            int id = Db.NextId(conn, tx, "country", "countryId");
            using (MySqlCommand cmd = Db.Cmd(
                "INSERT INTO country (countryId, country, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                "VALUES (@id, @name, UTC_TIMESTAMP(), @user, UTC_TIMESTAMP(), @user)", conn, tx))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", country);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
            }
            return id;
        }

        private static int FindOrCreateCity(MySqlConnection conn, MySqlTransaction tx, string city, int countryId, string user)
        {
            using (MySqlCommand find = Db.Cmd("SELECT cityId FROM city WHERE city = @name AND countryId = @countryId", conn, tx))
            {
                find.Parameters.AddWithValue("@name", city);
                find.Parameters.AddWithValue("@countryId", countryId);
                object found = find.ExecuteScalar();
                if (found != null) return Convert.ToInt32(found);
            }

            int id = Db.NextId(conn, tx, "city", "cityId");
            using (MySqlCommand cmd = Db.Cmd(
                "INSERT INTO city (cityId, city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                "VALUES (@id, @name, @countryId, UTC_TIMESTAMP(), @user, UTC_TIMESTAMP(), @user)", conn, tx))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", city);
                cmd.Parameters.AddWithValue("@countryId", countryId);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.ExecuteNonQuery();
            }
            return id;
        }
    }

    public static class AppointmentRepo
    {
        public static List<Appointment> GetAll()
        {
            List<Appointment> list = new List<Appointment>();
            string sql =
                "SELECT a.appointmentId, a.customerId, a.userId, c.customerName, u.userName, a.title, a.description, a.location, a.contact, " +
                "a.url, a.`type` AS AppointmentType, a.`start` AS AppointmentStart, a.`end` AS AppointmentEnd " +
                "FROM appointment a " +
                "JOIN customer c ON a.customerId = c.customerId " +
                "JOIN `user` u ON a.userId = u.userId " +
                "ORDER BY a.`start`";

            using (MySqlConnection conn = Db.Open())
            using (MySqlCommand cmd = Db.Cmd(sql, conn))
            using (MySqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                {
                    list.Add(new Appointment
                    {
                        AppointmentId = Convert.ToInt32(r["appointmentId"]),
                        CustomerId = Convert.ToInt32(r["customerId"]),
                        UserId = Convert.ToInt32(r["userId"]),
                        CustomerName = r["customerName"].ToString(),
                        UserName = r["userName"].ToString(),
                        Title = r["title"].ToString(),
                        Description = r["description"].ToString(),
                        Location = r["location"].ToString(),
                        Contact = r["contact"].ToString(),
                        Url = r["url"].ToString(),
                        Type = r["AppointmentType"].ToString(),
                        StartUtc = DateTime.SpecifyKind(Convert.ToDateTime(r["AppointmentStart"]), DateTimeKind.Utc),
                        EndUtc = DateTime.SpecifyKind(Convert.ToDateTime(r["AppointmentEnd"]), DateTimeKind.Utc)
                    });
                }
            }
            return list;
        }

        public static List<Appointment> GetUpcoming(int userId, int minutes)
        {
            DateTime now = DateTime.UtcNow;
            DateTime limit = now.AddMinutes(minutes);

            return GetAll()
                .Where(a => a.UserId == userId && a.StartUtc >= now && a.StartUtc <= limit)
                .OrderBy(a => a.StartUtc)
                .ToList();
        }

        // True if the appointment has an already scheduled appointment.
        public static bool HasAppointmentOverlap(DateTime startUtc, DateTime endUtc, int customerId, int userId, int ignoreAppointmentId)
        {
            return GetAll().Any(a =>
                a.AppointmentId != ignoreAppointmentId &&
                (a.CustomerId == customerId || a.UserId == userId) &&
                a.StartUtc < endUtc &&
                a.EndUtc > startUtc);
        }

        public static void Add(Appointment a, string user)
        {
            using (MySqlConnection conn = Db.Open())
            {
                int id = Db.NextId(conn, null, "appointment", "appointmentId");
                using (MySqlCommand cmd = Db.Cmd(
                    "INSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, `type`, url, `start`, `end`, " +
                    "createDate, createdBy, lastUpdate, lastUpdateBy) " +
                    "VALUES (@id, @customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, " +
                    "UTC_TIMESTAMP(), @user, UTC_TIMESTAMP(), @user)", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@customerId", a.CustomerId);
                    cmd.Parameters.AddWithValue("@userId", a.UserId);
                    cmd.Parameters.AddWithValue("@title", a.Title);
                    cmd.Parameters.AddWithValue("@description", a.Description);
                    cmd.Parameters.AddWithValue("@location", a.Location);
                    cmd.Parameters.AddWithValue("@contact", a.Contact);
                    cmd.Parameters.AddWithValue("@type", a.Type);
                    cmd.Parameters.AddWithValue("@url", a.Url);
                    cmd.Parameters.AddWithValue("@start", a.StartUtc);
                    cmd.Parameters.AddWithValue("@end", a.EndUtc);
                    cmd.Parameters.AddWithValue("@user", user);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void Update(Appointment a, string user)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlCommand cmd = Db.Cmd(
                "UPDATE appointment SET customerId = @customerId, title = @title, description = @description, location = @location, " +
                "contact = @contact, `type` = @type, url = @url, `start` = @start, `end` = @end, " +
                "lastUpdate = UTC_TIMESTAMP(), lastUpdateBy = @user WHERE appointmentId = @id", conn))
            {
                cmd.Parameters.AddWithValue("@customerId", a.CustomerId);
                cmd.Parameters.AddWithValue("@title", a.Title);
                cmd.Parameters.AddWithValue("@description", a.Description);
                cmd.Parameters.AddWithValue("@location", a.Location);
                cmd.Parameters.AddWithValue("@contact", a.Contact);
                cmd.Parameters.AddWithValue("@type", a.Type);
                cmd.Parameters.AddWithValue("@url", a.Url);
                cmd.Parameters.AddWithValue("@start", a.StartUtc);
                cmd.Parameters.AddWithValue("@end", a.EndUtc);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@id", a.AppointmentId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void Delete(int appointmentId)
        {
            using (MySqlConnection conn = Db.Open())
            using (MySqlCommand cmd = Db.Cmd("DELETE FROM appointment WHERE appointmentId = @id", conn))
            {
                cmd.Parameters.AddWithValue("@id", appointmentId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
