# PowerShell script to start Docker services for Smart Platform
Write-Host "Starting Docker services..." -ForegroundColor Green

# Check if Docker is running
try {
    docker ps | Out-Null
    Write-Host "Docker is running" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Docker is not running. Please start Docker Desktop." -ForegroundColor Red
    exit 1
}

# Start services
Write-Host "`nStarting PostgreSQL, Keycloak, and Backend services..." -ForegroundColor Yellow
docker-compose up -d

Write-Host "`nWaiting for services to be healthy..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check service status
Write-Host "`nService Status:" -ForegroundColor Cyan
docker-compose ps

Write-Host "`n=== Setup Instructions ===" -ForegroundColor Green
Write-Host "1. Keycloak Admin Console: http://localhost:8080" -ForegroundColor White
Write-Host "   Username: admin" -ForegroundColor White
Write-Host "   Password: admin" -ForegroundColor White
Write-Host "`n2. Wait 60-90 seconds for Keycloak to fully initialize" -ForegroundColor Yellow
Write-Host "`n3. Check logs: docker-compose logs -f keycloak" -ForegroundColor White
Write-Host "`n4. See DOCKER_SETUP.md for detailed configuration steps" -ForegroundColor White

Write-Host "`n=== Quick Test ===" -ForegroundColor Green
Write-Host "Test Keycloak health: curl http://localhost:8080/health/ready" -ForegroundColor White
Write-Host "Test PostgreSQL: docker-compose exec postgres psql -U postgres -c 'SELECT version();'" -ForegroundColor White

