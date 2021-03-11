#!/bin/bash
set -e

psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d postgres < database/instance.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/fundermaps_base.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/fundermaps_permissions.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/data/seed_geocoder.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/data/seed_data.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/data/seed_maplayer.sql
psql -h localhost -U postgres -v ON_ERROR_STOP=1 -d fundermaps < database/data/precompute.sql
