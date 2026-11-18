# --- Giai đoạn 1: Build ứng dụng ---
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file csproj và restore các thư viện phụ thuộc
COPY ["DemoVectorSearchWithOllama.csproj", "./"]
RUN dotnet restore "DemoVectorSearchWithOllama.csproj"

# Copy toàn bộ mã nguồn còn lại và tiến hành Publish
COPY . .
RUN dotnet publish "DemoVectorSearchWithOllama.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- Giai đoạn 2: Chạy ứng dụng (Runtime) ---
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DemoVectorSearchWithOllama.dll"]