$volumes = @(
    "taskio-logs",
    "taskio-dataprotection-keys",
    "taskio-ssl-certificate",
    "taskio-pgdata"
)

foreach ($volume in $volumes) {
    Write-Output "Creating volume: $volume"
    docker volume create $volume
}

Write-Output "Starting compose"
docker-compose up --build