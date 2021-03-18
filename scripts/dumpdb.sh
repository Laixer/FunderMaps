#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Dump the database to workspace.
#
# Run script from the workspace root and make
# sure the database runs on localhost.

set -e

pg_dump -h localhost -U postgres -d fundermaps -s -N public -f database/fundermaps_base.sql
pg_dump -h localhost -U postgres -d fundermaps -a -n application -f database/data/seed_application.sql
pg_dump -h localhost -U postgres -d fundermaps -a -n geocoder -f database/data/seed_geocoder.sql
pg_dump -h localhost -U postgres -d fundermaps -a -n data -f database/data/seed_data.sql
pg_dump -h localhost -U postgres -d fundermaps -a -n maplayer -f database/data/seed_maplayer.sql
pg_dump -h localhost -U postgres -d fundermaps -a -n report -f database/data/seed_report.sql
