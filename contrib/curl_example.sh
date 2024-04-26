#!/bin/bash
# Copyright (c) 2021-2023 Laixer B.V.
#
# Example script to show how to use the Laixer API with curl.

# Get a token
curl -X POST \
    -H "Content-Type: application/json" \
    -d '{"email":"", "password": ""}' \
    http://localhost:5000/api/auth/signin | jq

# Get a single address
curl -H "Content-Type: application/json" \
    -H 'authorization: Bearer <TOKEN>' \
    http://localhost:5000/api/geocoder/NL.IMBAG.PAND.XXX | jq
