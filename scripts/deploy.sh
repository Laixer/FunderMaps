#!/bin/bash
# Copyright (c) 2021-2023 Laixer B.V.
#
# Dump the database to workspace.
#
# Run script from the workspace root and make
# sure the database runs on localhost.

set -e

if [ ! -d "./scripts" ]
then
    echo "Run script from the project directory"
    exit 1
fi

directory="/opt/FunderMaps/"

if [ ! -d "$directory" ]; then
    mkdir -p "$directory"
    if [ $? -eq 0 ]; then
        echo "Directory created successfully."
    else
        echo "Failed to create directory."
        exit 1
    fi
else
    echo "Removing old application"
    rm -rf /opt/FunderMaps/*
fi

git pull
rm -rf /opt/FunderMaps/*

pushd src/FunderMaps.Worker/
dotnet publish -c Release -o /opt/FunderMaps/
popd

if [ -n "$(pidof systemd)" ]; then
    echo "Reloading systemd"
    systemctl daemon-reload
fi
