# Docker Setup Guide for Smart Event & Ticket Management Platform

This guide will help you set up the entire backend infrastructure using Docker, including Keycloak for authentication and PostgreSQL for the database.

## Prerequisites

- Docker Desktop installed and running (Windows/Mac) or Docker Engine (Linux)
- Docker Compose v2.0 or higher
- At least 4GB of available RAM
- Ports 5432, 8080, 5000, and 5001 available

## Quick Start

1. **Start all services:**
   ```bash
   docker-compose up -d
   ```

2. **Check service status:**
   ```bash
   docker-compose ps
   ```

3. **View logs:**
   ```bash
   # All services
   docker-compose logs -f
   
   # Specific service
   docker-compose logs -f keycloak
   docker-compose logs -f backend-api
   docker-compose logs -f postgres
   ```

## Services Overview

### 1. PostgreSQL Database
- **Container:** `smart-platform-postgres`
- **Port:** 5432
- **Database:** `SmartEventPlatformDb`
- **Username:** `postgres`
- **Password:** `1904`
- **Connection String:** `Host=localhost;Port=5432;Database=SmartEventPlatformDb;Username=postgres;Password=1904`

### 2. Keycloak Identity Provider
- **Container:** `smart-platform-keycloak`
- **Port:** 8080
- **Admin Console:** http://localhost:8080
- **Admin Username:** `admin`
- **Admin Password:** `admin`
- **Realm:** `dotnet-realm`
- **Client ID:** `dotnet-api`

### 3. Backend API (Optional)
- **Container:** `smart-platform-api`
- **Ports:** 5000 (HTTP), 5001 (HTTPS)
- **Swagger UI:** http://localhost:5000/swagger (when running)

## Keycloak Setup

### Initial Configuration

1. **Access Keycloak Admin Console:**
   - Open http://localhost:8080 in your browser
   - Click "Administration Console"
   - Login with:
     - Username: `admin`
     - Password: `admin`

2. **Verify Realm Configuration:**
   - The `dotnet-realm` should be automatically imported
   - Navigate to: **Realm Settings** → Select `dotnet-realm`
   - Verify the realm is active

3. **Create Test Users:**

   **Admin User:**
   - Go to **Users** → **Add user**
   - Username: `admin`
   - Email: `admin@example.com`
   - Click **Save**
   - Go to **Credentials** tab → Set password: `admin123` (temporary)
   - Toggle **Temporary** to OFF
   - Go to **Role Mappings** → Assign role: `Admin`

   **Buyer User:**
   - Go to **Users** → **Add user**
   - Username: `buyer`
   - Email: `buyer@example.com`
   - Click **Save**
   - Go to **Credentials** tab → Set password: `buyer123` (temporary)
   - Toggle **Temporary** to OFF
   - Go to **Role Mappings** → Assign role: `Buyer`

   **Seller User:**
   - Go to **Users** → **Add user**
   - Username: `seller`
   - Email: `seller@example.com`
   - Click **Save**
   - Go to **Credentials** tab → Set password: `seller123` (temporary)
   - Toggle **Temporary** to OFF
   - Go to **Role Mappings** → Assign role: `Seller`

4. **Get Access Token (for testing):**

   Using curl:
   ```bash
   curl -X POST http://localhost:8080/realms/dotnet-realm/protocol/openid-connect/token \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "client_id=dotnet-api" \
     -d "client_secret=your-client-secret-change-this" \
     -d "username=admin" \
     -d "password=admin123" \
     -d "grant_type=password"
   ```

   Or using Postman:
   - Method: POST
   - URL: `http://localhost:8080/realms/dotnet-realm/protocol/openid-connect/token`
   - Body (x-www-form-urlencoded):
     - `client_id`: `dotnet-api`
     - `client_secret`: `your-client-secret-change-this`
     - `username`: `admin` (or `buyer` or `seller`)
     - `password`: `admin123` (or `buyer123` or `seller123`)
     - `grant_type`: `password`

   **Example for Buyer:**
   ```bash
   curl -X POST http://localhost:8080/realms/dotnet-realm/protocol/openid-connect/token \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "client_id=dotnet-api" \
     -d "client_secret=your-client-secret-change-this" \
     -d "username=buyer" \
     -d "password=buyer123" \
     -d "grant_type=password"
   ```

   **Example for Seller:**
   ```bash
   curl -X POST http://localhost:8080/realms/dotnet-realm/protocol/openid-connect/token \
     -H "Content-Type: application/x-www-form-urlencoded" \
     -d "client_id=dotnet-api" \
     -d "client_secret=your-client-secret-change-this" \
     -d "username=seller" \
     -d "password=seller123" \
     -d "grant_type=password"
   ```

### Update Client Secret

1. Go to **Clients** → Select `dotnet-api`
2. Go to **Credentials** tab
3. Copy the **Secret** value
4. Update `Backend/src/SmartPlatform.Api/appsettings.json`:
   ```json
   {
     "Keycloak": {
       "Authority": "http://localhost:8080/realms/dotnet-realm",
       "Audience": "dotnet-api",
       "ClientSecret": "your-actual-secret-here"
     }
   }
   ```

## Backend Configuration

### Option 1: Run Backend in Docker

The backend is included in `docker-compose.yml`. It will:
- Automatically connect to PostgreSQL
- Use Keycloak for authentication
- Run migrations on startup

**Start with backend:**
```bash
docker-compose up -d
```

### Option 2: Run Backend Locally

1. **Update `appsettings.json`:**
   ```json
   {
     "Keycloak": {
       "Authority": "http://localhost:8080/realms/dotnet-realm",
       "Audience": "dotnet-api",
       "RequireHttpsMetadata": false
     },
     "ConnectionStrings": {
       "Postgres": "Host=localhost;Port=5432;Database=SmartEventPlatformDb;Username=postgres;Password=1904"
     }
   }
   ```

2. **Start only infrastructure services:**
   ```bash
   docker-compose up -d postgres keycloak keycloak-db
   ```

3. **Run backend locally:**
   ```bash
   cd Backend
   dotnet run
   ```

## Testing the Setup

### 1. Test Database Connection
```bash
docker-compose exec postgres psql -U postgres -d SmartEventPlatformDb -c "SELECT version();"
```

### 2. Test Keycloak Health
```bash
curl http://localhost:8080/health/ready
```

### 3. Test Backend API

**Public endpoint (no auth required):**
```bash
curl http://localhost:5000/api/public
```

**Protected endpoint (requires auth):**
```bash
# First get token (see Keycloak Setup section)
TOKEN="your-access-token-here"

curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/api/secure
```

**Admin endpoint:**
```bash
curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/admin
```

**Buyer endpoint:**
```bash
curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/buyer
```

**Seller endpoint:**
```bash
curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/seller
```

## Troubleshooting

### Keycloak not starting
- Check logs: `docker-compose logs keycloak`
- Ensure keycloak-db is healthy: `docker-compose ps`
- Wait 60-90 seconds for Keycloak to fully initialize

### Database connection errors
- Verify PostgreSQL is running: `docker-compose ps postgres`
- Check connection string in appsettings.json
- Ensure database exists: `docker-compose exec postgres psql -U postgres -l`

### Backend authentication fails
- Verify Keycloak is accessible: `curl http://localhost:8080/health/ready`
- Check Authority URL in appsettings.json matches Keycloak URL
- Verify client secret is correct
- Check token includes required roles

### Port conflicts
- Stop services using the ports:
  ```bash
  # Windows
  netstat -ano | findstr :8080
  taskkill /PID <PID> /F
  
  # Linux/Mac
  lsof -ti:8080 | xargs kill
  ```

## Stopping Services

```bash
# Stop all services
docker-compose down

# Stop and remove volumes (⚠️ deletes data)
docker-compose down -v
```

## Useful Commands

```bash
# Restart a specific service
docker-compose restart keycloak

# View real-time logs
docker-compose logs -f backend-api

# Execute command in container
docker-compose exec postgres psql -U postgres

# Rebuild backend image
docker-compose build backend-api

# Check service health
docker-compose ps
```

## Security Notes

⚠️ **Important:** This setup is for **development only**. For production:

1. Change all default passwords
2. Use strong client secrets
3. Enable HTTPS
4. Use environment variables for secrets
5. Restrict network access
6. Use proper SSL certificates
7. Enable Keycloak security features (brute force protection, etc.)

## Next Steps

1. Configure Keycloak realm and create users with roles:
   - **Admin** - Full system access
   - **Buyer** - Can purchase tickets and attend events
   - **Seller** - Can create and manage events
2. Update backend appsettings.json with correct Keycloak settings
3. Test authentication flow for each role
4. Run database migrations
5. Test API endpoints:
   - `/admin` - Requires Admin role
   - `/buyer` - Requires Buyer role
   - `/seller` - Requires Seller role

For more information, see the main [README.md](README.md).

