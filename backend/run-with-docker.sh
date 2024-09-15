#!/bin/bash

VOLUMES=(
  "taskio-logs"
  "taskio-dataprotection-keys"
  "taskio-ssl-certificate"
  "taskio-pgdata"
)

for VOLUME in "${VOLUMES[@]}"; 
do
  echo "Creating volume: $VOLUME"
  docker volume create "$VOLUME"
done

echo "Starting compose: $VOLUME"
docker-compose up --build
