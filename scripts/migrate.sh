#!/bin/bash
set -e

migra --exclude public --unsafe postgresql://user:password@host/fundermaps postgresql://postgres@localhost/fundermaps > diff.sql
