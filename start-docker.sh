#!/bin/bash

# Bash script to start Docker services for Smart Platform
echo "Starting Docker services..."

# Check if Docker is running
if ! docker ps > /dev/null 2>&1; then
    echo "ERROR: Docker is not running. Please start Docker."
    exit 1
fi

echo "Docker is running"

# Start services
echo ""
echo "Starting PostgreSQL, Keycloak, and Backend services..."
docker-compose up -d

echo ""
echo "Waiting for services to be healthy..."
sleep 10

# Check service status
echo ""
echo "Service Status:"
docker-compose ps

echo ""
echo "=== Setup Instructions ==="
echo "1. Keycloak Admin Console: http://localhost:8080"
echo "   Username: admin"
echo "   Password: admin"
echo ""
echo "2. Wait 60-90 seconds for Keycloak to fully initialize"
echo ""
echo "3. Check logs: docker-compose logs -f keycloak"
echo ""
echo "4. See DOCKER_SETUP.md for detailed configuration steps"

echo ""
echo "=== Quick Test ==="
echo "Test Keycloak health: curl http://localhost:8080/health/ready"
echo "Test PostgreSQL: docker-compose exec postgres psql -U postgres -c 'SELECT version();'"

