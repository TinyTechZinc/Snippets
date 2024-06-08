#!/bin/bash
cd /srv/Snippets/
pwd

# Remove existing changes
git restore .

# Pull changes (if any).
git pull

# Try to keep the execute permissions
chmod a+x start-server.sh

# Build the app.
dotnet publish Test -o ./publish

# Launch app.
./publish/Test
