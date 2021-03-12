#!/bin/bash
set -e

pg_dump -h localhost -U postgres -d fundermaps -s -N public -f database/fundermaps_base.sql
