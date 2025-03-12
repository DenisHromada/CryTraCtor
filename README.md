# CryTraCtor
Bachelor's Thesis - Forensic Analysis of Anonymization Principles of Cryptocurrency Networks

## Initial Setup
1. Create your own `.env` file. You can use the `.env.example` file as a template.
2. Have Docker installed and run `docker-compose up -d postgres` in the root directory. This creates and a PostgreSQL 
database container.
3. Apply migrations to the database. The tool named `dotnet-ef` must be available in the terminal.
Then run `./scripts/migrate.sh`in the root directory of the solution.
4. Connect the application to the database
- If you wish to run the application using docker, you can simply run `docker compose up -d`.
- If you wish to run the application locally, either make sure environment variables from the `.env` file are sourced, 
or add the database connection string to the CryTraCtor.API project. (for example as .NET User Secrets)

### WSL Development Setup Snippets
- Run Rider through Remote Development Gateway plugin in WSL

- Change permissions in `/etc/wsl.conf`
```
[automount]
options="metadata,umask=22,fmask=11"
```

- Have the dotnet tool and install the following:
```bash
dotnet tool install --global dotnet-ef
dotnet new install MudBlazor.Templates
```
