# PokeApp

## Setup
This sample requires a special configuration value called `IssuerSigningKey` that it uses to sign JWT tokens. 

You can add this setting by either making use of ASP.NET Secret Manager or inside the `appsetting.json` file:

### Using the ASP.NET Secret Manager (recommended)
From the same directory of `project.json`, execute the following command:
```
dotnet user-secrets set JwtAuthenticationOptions:IssuerSigningKey superdupersecretkey123
```

### Initialize the database
If the database does not exists yet, you need to run a create/update script. From the Nuget Package Manager Console, run:
```
Update-Database
```
and the database will be created.
