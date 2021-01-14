# PrologMobile
Test project for PrologMobile

---
Project uses [.NET 5.0](https://dotnet.microsoft.com/download)

### Running
Please ensure you have .NET 5.0 installed on your system.

To run the project, navigate to the PrologMobile.Web folder and run 

```
dotnet restore
dotnet run
```

The project will be available at `http://localhost:5000` or `https://localhost:5001`

The project consists of one endpoint, `organizations/summaries`, which returns a JSON payload containg a list of summaries for organizations returned from the mock api.

hitting either `http://localhost:5000/organizations/summaries` or `https://localhost:5001/organizations/summaries` will work.

Note: I was recieving an "Over Rate Limit" message from the server when I was sending many requests over rapidly. To get around this, I had to limit the number of concurrent requests to the server, and I implemented a retry loop which waits one second before trying again.

### Tests
To run tests, navigate to the PrologMobile.Tests folder and run

```
dotnet restore
dotnet run
```

---

Thanks so much for the opportunity!
