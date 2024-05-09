# CryTraCtor
Bachelor's Thesis - Forensic Analysis of Anonymization Principles of Cryptocurrency Networks

## Initial Setup
1. Create your own `.env` file. You can use the `.env.example` file as a template.
2. Have Docker installed and run `docker-compose up -d postgres` in the root directory. This creates and a PostgreSQL 
database container.
3. Apply migrations to the database. The tool named `dotnet-ef` must be available in the terminal.
Then run `dotnet ef database update --connection "<connectionString>"`in the CryTraCtor.Database project root.
The connection string should be in the following format:
`Server=postgres;Port=5432;Database=postgres;User Id=postgres;Password=postgres;`.
4. Connect the application to the database
- If you want to run the application in a container, you can run `docker compose up -d`.
- If you want to run the application locally, either make sure environment variables from the `.env` file are sourced, 
or add the database connection string to the CryTraCtor.API project. (for example as .NET User Secrets)