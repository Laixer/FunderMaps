#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Run the database migrator.

set -e

migra --exclude public --unsafe postgresql://user:password@host/fundermaps postgresql://postgres@localhost/fundermaps > diff.sql
