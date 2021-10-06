#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Run the database migrator.

set -e

if [ ! -d "./scripts" ]
then
    echo "Run script from the project directory"
    exit 1
fi

# <target db> <source db>
migra --with-privileges --exclude public --unsafe postgresql://user:password@host/fundermaps postgresql://postgres@localhost/fundermaps > diff.sql
