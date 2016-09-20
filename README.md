# PokeApp

## Setup
This sample requires a special configuration value called `IssuerSigningKey` that it uses to sign JWT tokens. 

You can add this setting by either making use of ASP.NET Secret Manager or inside the `appsetting.json` file:

### Using the ASP.NET Secret Manager (recommended)
From the same directory of `project.json`, execute the following command:
```
dotnet user-secrets set JwtAuthenticationOptions:IssuerSigningKey superdupersecretkey123
```