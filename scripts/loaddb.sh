#!/bin/bash
set -e

psql -h localhost -U postgres -d postgres -v ON_ERROR_STOP=1 < database/instance.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/fundermaps_base.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_geocoder.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_data.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/seed_maplayer.sql
psql -h localhost -U postgres -d fundermaps -v ON_ERROR_STOP=1 < database/data/precompute.sql
