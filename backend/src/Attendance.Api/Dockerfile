# ==========================================
# Attendance Management System
# .NET 10 Backend Docker Image
# ==========================================

# ==========================================
# BUILD STAGE
# ==========================================

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# ----------------------------------------------------------
# Copy project files
# ----------------------------------------------------------

COPY ["Attendance.Api/Attendance.Api.csproj", "Attendance.Api/"]
COPY ["Attendance.Application/Attendance.Application.csproj", "Attendance.Application/"]
COPY ["Attendance.Contracts/Attendance.Contracts.csproj", "Attendance.Contracts/"]
COPY ["Attendance.Domain/Attendance.Domain.csproj", "Attendance.Domain/"]
COPY ["Attendance.Infrastructure/Attendance.Infrastructure.csproj", "Attendance.Infrastructure/"]

# ----------------------------------------------------------
# Restore NuGet packages
# ----------------------------------------------------------

RUN dotnet restore "Attendance.Api/Attendance.Api.csproj"

# ----------------------------------------------------------
# Copy source code
# ----------------------------------------------------------

COPY . .

# ----------------------------------------------------------
# Publish
#
# Restore again here deliberately.
# This prevents NETSDK1064 errors caused by a
# partially populated NuGet cache between Docker layers.
# ----------------------------------------------------------

RUN dotnet publish "Attendance.Api/Attendance.Api.csproj" \
    -c Release \
    -o /app/publish


# ==========================================
# PRODUCTION STAGE
# ==========================================

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

# Copy published application
COPY --from=build /app/publish .

# Run the API
ENTRYPOINT ["dotnet", "Attendance.Api.dll"]