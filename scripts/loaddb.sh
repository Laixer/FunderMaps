#!/bin/bash
# Copyright (c) 2021 Laixer B.V.
#
# Load the database from workspace.
#
# Run script from the workspace root and make
# sure the database runs on localhost.

set -e

if [ ! -d "./scripts" ]
then
    echo "Run script from the project directory"
    exit 1
fi

createuser -h localhost -U postgres -L fundermaps
createuser -h localhost -U postgres -I fundermaps_batch
createuser -h localhost -U postgres -I fundermaps_portal
createuser -h localhost -U postgres -I fundermaps_webapp
createuser -h localhost -U postgres -I fundermaps_webservice
createdb -h localhost -U postgres -O fundermaps fundermaps
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/fundermaps_base.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_application.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_geocoder.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_data.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_maplayer.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_report.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/precompute.sql
