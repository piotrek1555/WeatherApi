## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or later

### Setup

The application should work without any additional configuration. 
The database file is included in the solution and the appsettings.json file contains a 'SqliteDbPath' entry that points to the database file.
If you want to use a different database file, you need to apply migrations, e.g. using the 'Update-Database' command.


1. Open the solution in Visual Studio.
2. Restore the dependencies
3. Build the solution
4. Run the application


## API Endpoints

### User Registration

- **Endpoint**: `POST /api/registration`
- **Description**: Registers a new user.

Samlpe request:

```
curl -X 'POST' \
  'https://localhost:7199/api/registration' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "firstName": "john",
  "lastName": "doe",
  "password": "passwd",
  "country": "mt"
}'
```

Sample response:

```
{
  "firstName": "john",
  "lastName": "doe",
  "userName": "jodo522275",
  "country": "mt"
}
```

### User Login

- **Endpoint**: `POST /api/login`
- **Description**: Authenticates a user and returns a JWT token.

Sample request:

```
curl -X 'POST' \
  'https://localhost:7199/api/login' \
  -H 'accept: text/plain' \
  -H 'Content-Type: application/json' \
  -d '{
  "username": "jodo522275",
  "password": "passwd"
}'
```

Sample response:

```
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJMb25naXR1ZGUiOiIxNC4zNzU0IiwiTGF0aXR1ZGUiOiIzNS45Mzc1IiwibmJmIjoxNzIyNjE3NzMxLCJleHAiOjE3MjI2MTg2MjksImlzcyI6ImFwaS5iZWFyZXIuYXV0aCIsImF1ZCI6ImFwaS5iZWFyZXIuYXV0aCJ9.kTn6CnBUclW_1C-W8E6CxArnfzmR-Xmy4osnWhfvRWY"
}
```


### Get Weather

- **Endpoint**: `GET /api/weather`
- **Description**: Retrieves weather information for the country where the user is registered in our system.
- **Authorization**: Endpoint requires a valid JWT token in the Authorization header.


Sample request:

```
curl -X 'GET' \
  'https://localhost:7199/api/Weather' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJMb25naXR1ZGUiOiIxNC4zNzU0IiwiTGF0aXR1ZGUiOiIzNS45Mzc1IiwibmJmIjoxNzIyNjE3NzMxLCJleHAiOjE3MjI2MTg2MjksImlzcyI6ImFwaS5iZWFyZXIuYXV0aCIsImF1ZCI6ImFwaS5iZWFyZXIuYXV0aCJ9.kTn6CnBUclW_1C-W8E6CxArnfzmR-Xmy4osnWhfvRWY'
```

Sample response:

```
{
  "latitude": 35.9375,
  "longitude": 14.375,
  "generationtime_ms": 0.028967857360839844,
  "utc_offset_seconds": 0,
  "timezone": "GMT",
  "timezone_abbreviation": "GMT",
  "elevation": 39,
  "hourly_units": {
    "time": "iso8601",
    "temperature_2m": "°C"
  },
  "hourly": {
    "time": [
      "2024-08-02T00:00",
      "2024-08-02T01:00"    
    ],
    "temperature_2m": [
      25.6,
      25.3      
    ]
  }
}
```