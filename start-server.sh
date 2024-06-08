#!/bin/bash
cd /srv/Snippets/

# Pull changes (if any).
git pull

# Build the app.
dotnet publish Test -o ./publish

# Launch app.
./publish/Test
