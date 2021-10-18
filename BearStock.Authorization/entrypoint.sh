#!/bin/bash

set -e

export PATH="$PATH:$HOME/.dotnet/tools/"

dotnet tool restore

until dotnet ef database update --context ApplicationDbContext; do
>&2 echo "ApplicationDbContext is starting up"
sleep 1
done

until dotnet ef database update --context ConfigurationDbContext; do
>&2 echo "ConfigurationDbContext is starting up"
sleep 1
done

until dotnet ef database update --context PersistedGrantDbContext; do
>&2 echo "PersistedGrantDbContext is starting up"
sleep 1
done

>&2 echo "SQL Server is up - executing command"
