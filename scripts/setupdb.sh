#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Setup local database.

set -e

if [ ! -d "./scripts" ]
then
    echo "Run script from the project directory"
    exit 1
fi

docker run --rm --name fm_local -e POSTGRES_HOST_AUTH_METHOD=trust -p 5432:5432 -d postgis/postgis:12-master
