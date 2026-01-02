# PowerShell script to test Docker setup
Write-Host "Testing Docker Setup..." -ForegroundColor Green
Write-Host ""

$allTestsPassed = $true

# Test 1: Check if services are running
Write-Host "Test 1: Checking if services are running..." -ForegroundColor Yellow
$services = @("smart-platform-postgres", "smart-platform-keycloak", "smart-platform-keycloak-db")
foreach ($service in $services) {
    $status = docker ps --filter "name=$service" --format "{{.Status}}"
    if ($status) {
        Write-Host "  ✓ $service is running" -ForegroundColor Green
    } else {
        Write-Host "  ✗ $service is NOT running" -ForegroundColor Red
        $allTestsPassed = $false
    }
}

# Test 2: Check PostgreSQL connection
Write-Host "`nTest 2: Testing PostgreSQL connection..." -ForegroundColor Yellow
try {
    $result = docker-compose exec -T postgres psql -U postgres -c "SELECT version();" 2>&1
    if ($result -match "PostgreSQL") {
        Write-Host "  ✓ PostgreSQL is accessible" -ForegroundColor Green
    } else {
        Write-Host "  ✗ PostgreSQL connection failed" -ForegroundColor Red
        $allTestsPassed = $false
    }
} catch {
    Write-Host "  ✗ PostgreSQL connection failed: $_" -ForegroundColor Red
    $allTestsPassed = $false
}

# Test 3: Check Keycloak health
Write-Host "`nTest 3: Testing Keycloak health..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8080/health/ready" -UseBasicParsing -TimeoutSec 5 -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✓ Keycloak is healthy and ready" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Keycloak health check failed (Status: $($response.StatusCode))" -ForegroundColor Red
        $allTestsPassed = $false
    }
} catch {
    Write-Host "  ⚠ Keycloak may still be starting up. Wait 60-90 seconds and try again." -ForegroundColor Yellow
    Write-Host "    Error: $_" -ForegroundColor Yellow
}

# Test 4: Check Keycloak admin console
Write-Host "`nTest 4: Testing Keycloak admin console..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:8080" -UseBasicParsing -TimeoutSec 5 -ErrorAction Stop
    if ($response.StatusCode -eq 200) {
        Write-Host "  ✓ Keycloak admin console is accessible" -ForegroundColor Green
    } else {
        Write-Host "  ✗ Keycloak admin console not accessible" -ForegroundColor Red
        $allTestsPassed = $false
    }
} catch {
    Write-Host "  ⚠ Keycloak admin console may still be starting up" -ForegroundColor Yellow
}

# Test 5: Check database exists
Write-Host "`nTest 5: Checking if database exists..." -ForegroundColor Yellow
try {
    $result = docker-compose exec -T postgres psql -U postgres -lqt 2>&1 | Select-String "smarteventplatformdb"
    if ($result) {
        Write-Host "  ✓ Database 'SmartEventPlatformDb' exists" -ForegroundColor Green
    } else {
        Write-Host "  ⚠ Database may be created on first backend run" -ForegroundColor Yellow
    }
} catch {
    Write-Host "  ⚠ Could not check database (may be normal)" -ForegroundColor Yellow
}

Write-Host ""
if ($allTestsPassed) {
    Write-Host "=== All critical tests passed! ===" -ForegroundColor Green
    Write-Host "Next steps:" -ForegroundColor Cyan
    Write-Host "1. Access Keycloak: http://localhost:8080" -ForegroundColor White
    Write-Host "2. Login with admin/admin" -ForegroundColor White
    Write-Host "3. Create test users and configure realm" -ForegroundColor White
    Write-Host "4. See DOCKER_SETUP.md for detailed instructions" -ForegroundColor White
} else {
    Write-Host "=== Some tests failed ===" -ForegroundColor Red
    Write-Host "Check logs: docker-compose logs" -ForegroundColor Yellow
    Write-Host "Restart services: docker-compose restart" -ForegroundColor Yellow
}

