# SolarWatchApi

An API that provides sunrise and sunset times for any given city. Users can input a location and receive accurate daily solar data, making it useful for planning and scheduling.

# Features

- Retrieve sunrise and sunset times based on city name
- Uses OpenWeatherMap API for geolocation
- Fetches sun cycle data from the Sunrise-Sunset API
- Simple RESTful API endpoints

# Installation & Setup

1. Clone the repository

    git clone https://github.com/yourusername/solarwatch-api.git
    cd solarwatch-api

2. Install dependencies

Restore the required NuGet packages:
    dotnet restore

3. Configure API Keys
You need API keys to use external services. Update your configuration file (appsettings.json or environment variables):

{
  "OpenWeatherMapApiKey": "yourApiKey",
  "SunriseSunsetApiUrl": "https://api.sunrise-sunset.org/json"
}

Alternatively, you can set them as environment variables:

export OpenWeatherMapApiKey="yourApiKey"
export SunriseSunsetApiUrl="https://api.sunrise-sunset.org/json"

# Usage

1. Get Geolocation Data
Fetch city latitude and longitude using OpenWeatherMap API:
    GET http://api.openweathermap.org/geo/1.0/direct?q={city}&limit=5&appid=yourApiKey

2. Get Sunrise & Sunset Times
Use the retrieved latitude and longitude to fetch sun cycle data:
    GET https://api.sunrise-sunset.org/json?lat={latitude}&lng={longitude}&date={date}

3. Example API Response
{
  "city": "New York",
  "latitude": 40.7128,
  "longitude": -74.0060,
  "sunrise": "06:30 AM",
  "sunset": "07:45 PM"
}

# Database Connection

The project connects to a SQL Server database using the following configuration:

Server=localhost,1433;Database=SolarWatch;User Id=YourUserId;Password=SecretPassword;Encrypt=false;
Make sure your database is running, and update credentials as needed in appsettings.json.


# Contact
    GitHub: @takacsvilmos
    Email: vilmos.takacs@gmail.com
