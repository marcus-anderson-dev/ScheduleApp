# Scheduling Application

Application was for my Software 2 Assignment.  A Windows desktop app for managing customers and booking appointments, used with MySQL DB. Built in C# using Win Forms.

## Features included with application.

- **Login screen in English or Spanish.** Language select automatically, dependent on computer's region settings. The Screen also displays the user's location with timezone.
- **Login History.** Successful logins are written to `Login_History.txt`.
- **Appointment alert.** Displays once a user sees any appoints after logging in.  Alert will display within the next 15 minutes.
- **Customer Records.** CRUD used for customer records.
  - Requirement fields are checked, and phone numbers may contain only digits and dashes.
  - Customers that are a duplicate, are rejected.
  - Removing a customer also deletes their appointments with a single transaction.
- **Appointments.** CRUD for appointments.
  - Must be between the Business Hours of 9:00 AM to 5:00 PM Eastern, Mon - Fri.
  - Overlapping customers will be rejected.
  - The URL must be a valid web address.
- **Time zones.** Times are stored in UTC and shown in each user's local time, including daylight saving time.
- **Calendar view.** A monthly calender that shows dates with an appoinment in bold.
- **Reports**, built with LINQ.

## Files and What they do

| File | What the file does |
| --- | --- |
| `Db.cs` | DB access: the connection, plus `UserRepo`, `CustomerRepo` and `AppointmentRepo` with SQL queries |
| `Models.cs` | Includes `Customer` & `Appointment` classes |
| `Helpers.cs` | Converts a time-zone, checks against business hours, and English/Spanish Text |
| `LoginForm.cs` | Login, detects users language, login history and appointment alert |
| `MainForm.cs` | Main Menu of Application |
| `CustomerForm.cs` | Customer records |
| `AppointmentForm.cs` | Appoints including their validation rules |
| `CalendarForm.cs` | Monthly Calender View|
| `ReportsForm.cs` | Three Different Reports |

## How to use with Database.

The app uses a MySQL 8 database named `client_schedule` with six tables: `country`, `city`, `address`, `customer`, `user` and `appointment`.

| Script | What the Script Does |
| --- | --- |
| `schema.sql` | Creates the database and all six tables |
| `setup.sql` | Adds sample customers and appointments, plus the `test` login |

## How to run this program.

**Requirements to run this application:** Windows with Visual Studio, .NET, and either Docker or MySQL 8.

### 1. Set up the database

**First Option for database setup** In the project folder, run:

```bash
docker compose up -d
```

Starts MySQL with the tables and the provided sample data loaded, uses the same user, password and port (3306) the app expects.

**Second Option for database setup.** Create a user named `sqlUser` with password `Passw0rd!`, then run `schema.sql` followed by `setup.sql` in MySQL Workbench or the `mysql` command line.

### 2. Run the app

1. Open `ScheduleApp.sln` in Visual Studio. The `MySql.Data` package is restored automatically.
2. Press **F5** to build and run.
3. Log in with username **test** and password **test**.

> The login in `Db.cs` is a local dev login that was required by the Software 2 Course. It's not used anywhere else.
