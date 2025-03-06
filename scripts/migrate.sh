#!/bin/bash

# print commands before execution
#set -x

# Load environment variables
. ./.env

dotnet ef database update --connection "${DATABASE_CONNECTION_STRING}" --project ./src/CryTraCtor.Database
