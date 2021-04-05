#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Setup local database.

set -e

docker run --rm -e POSTGRES_HOST_AUTH_METHOD=trust -p 5432:5432 postgis/postgis:12-master