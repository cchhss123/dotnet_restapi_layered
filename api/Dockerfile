# 使用官方 .NET SDK 映像檔
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# 設定工作目錄
WORKDIR /app

# 複製專案文件（包括 api.csproj）
COPY ./api/api.csproj ./

# 安裝 Swashbuckle.AspNetCore
# RUN dotnet add package Swashbuckle.AspNetCore
# 執行 `dotnet restore` 來安裝所有 NuGet 套件
RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# 複製所有程式碼
#COPY ./api ./

# 建置專案
#RUN dotnet build --no-cache

# 執行專案
#CMD ["dotnet", "watch", "run", "--project", "/app/api.csproj"]
