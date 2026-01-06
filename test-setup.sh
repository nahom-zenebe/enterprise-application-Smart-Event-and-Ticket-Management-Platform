#!/bin/bash

# Bash script to test Docker setup
echo "Testing Docker Setup..."
echo ""

allTestsPassed=true

# Test 1: Check if services are running
echo "Test 1: Checking if services are running..."
services=("smart-platform-postgres" "smart-platform-keycloak" "smart-platform-keycloak-db")
for service in "${services[@]}"; do
    if docker ps --format "{{.Names}}" | grep -q "^${service}$"; then
        echo "  ✓ $service is running"
    else
        echo "  ✗ $service is NOT running"
        allTestsPassed=false
    fi
done

# Test 2: Check PostgreSQL connection
echo ""
echo "Test 2: Testing PostgreSQL connection..."
if docker-compose exec -T postgres psql -U postgres -c "SELECT version();" > /dev/null 2>&1; then
    echo "  ✓ PostgreSQL is accessible"
else
    echo "  ✗ PostgreSQL connection failed"
    allTestsPassed=false
fi

# Test 3: Check Keycloak health
echo ""
echo "Test 3: Testing Keycloak health..."
if curl -f -s http://localhost:8080/health/ready > /dev/null 2>&1; then
    echo "  ✓ Keycloak is healthy and ready"
else
    echo "  ⚠ Keycloak may still be starting up. Wait 60-90 seconds and try again."
fi

# Test 4: Check Keycloak admin console
echo ""
echo "Test 4: Testing Keycloak admin console..."
if curl -f -s http://localhost:8080 > /dev/null 2>&1; then
    echo "  ✓ Keycloak admin console is accessible"
else
    echo "  ⚠ Keycloak admin console may still be starting up"
fi

# Test 5: Check database exists
echo ""
echo "Test 5: Checking if database exists..."
if docker-compose exec -T postgres psql -U postgres -lqt 2>/dev/null | grep -q smarteventplatformdb; then
    echo "  ✓ Database 'SmartEventPlatformDb' exists"
else
    echo "  ⚠ Database may be created on first backend run"
fi

echo ""
if [ "$allTestsPassed" = true ]; then
    echo "=== All critical tests passed! ==="
    echo "Next steps:"
    echo "1. Access Keycloak: http://localhost:8080"
    echo "2. Login with admin/admin"
    echo "3. Create test users and configure realm"
    echo "4. See DOCKER_SETUP.md for detailed instructions"
else
    echo "=== Some tests failed ==="
    echo "Check logs: docker-compose logs"
    echo "Restart services: docker-compose restart"
fi

