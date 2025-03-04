#!/bin/bash

# print commands before execution
#set -x

# Load environment variables
. ./.env

POSTGRES_HOST=localhost #name resolution is not available outside the Docker network
DB_CONNECTION_STRING="Server=$POSTGRES_HOST;Port=$POSTGRES_PORT;Database=$POSTGRES_DB;Username=$POSTGRES_USER;Password=$POSTGRES_PASSWORD"

dotnet ef database update --connection "$DB_CONNECTION_STRING" --project ./src/CryTraCtor.Database
