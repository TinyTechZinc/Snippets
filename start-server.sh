#!/bin/bash
cd /srv/Snippets/
pwd

# Pull changes (if any).
git pull

# Try to keep the same permissions
chmod a+x start-server.sh

# Build the app.
dotnet publish Test -o ./publish

# Launch app.
./publish/Test
