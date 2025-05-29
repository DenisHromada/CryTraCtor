# CryTraCtor
Bachelor's Thesis - Deep Packet Inspection of Cryptocurrency Wallet Traffic

## Initial Setup
1. Create your own `.env` file. You can use the `.env.example` file as a template. (When modifying values, make sure to change launchSettings.json in the webapp project as well.)
2. Have Docker installed and run `docker compose -f compose.prod.yml up -d` in the root directory. This creates two containers. One with the PostgreSQL
database and one running the application.
3. Apply migrations to the database. The tool named `dotnet-ef` must be available in the terminal.
Then run `./scripts/migrate.sh`in the root directory of the solution.
4. The application should now be available at `localhost:5261`

### WSL Development Setup Remarks
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

- The application uses SharpPcap for pcap analysis, which relies on the `libpcap` system library. Without it, the application will not be able to analyse pcap files.
```bash
sudo apt-get update && sudo apt-get install -y libpcap-dev
```
