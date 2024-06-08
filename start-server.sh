#!/bin/bash
cd /srv/Snippets/
pwd
whoami

# Pull changes (if any).
git restore .
git pull

# Build the app.
dotnet publish Test -o ./publish

# Launch app.
./publish/Test
