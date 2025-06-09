# **🚀 RESTful API: .NET 8 + SQL Server 2022 (Docker)**
這是一個基於 **ASP.NET Core** 的 RESTful API，使用 **SQL Server 2022** 作為後端資料庫，並透過 **Docker Compose** 進行容器化部署。

## 🌟 專案特色
✅ **基於 ASP.NET Core 8**，提供 RESTful API 支援 `CRUD（建立 / 讀取 / 更新 / 刪除）`

✅ **採用分層架構**（Controller → Service → Repository），提升程式碼可維護性與擴展性

✅ **JWT 設定優化**：將 JWT 設定封裝到強型別的 `JwtSettings` 類別，Token 過期時間移至 `appsettings.json`，敏感資訊透過 `.env` 管理

✅ **使用 JWT 身份驗證**，此專案示範驗證保護 `/users/{id} 的 PUT 與 DELETE 2個api端點` (自訂 JWT 驗證回應中介軟體)

✅ **完善的 Swagger 文件**：所有 API 端點皆有 `summary` 註解，並設定載入 XML 文件以產生更詳細的 API 文件

✅ **容器化部署**（Docker Compose 一鍵啟動 `.NET 8 API + SQL Server` ），並提供生產環境的多階段建置 Dockerfile (`api/Dockerfile.Production`) 與 `docker-compose.prod.yaml`

✅ **SQL Server 2022 整合**，使用 **EF Core Migration** 進行資料庫初始化與版本控制

✅ **Adminer** 作為 Web UI 管理資料庫工具，簡單易用(本機如有安裝SSMS，亦可連線 127.0.0.1 管理資料庫，帳密請參考api/appsettings.json)

✅ **支援 API 測試**（Postman、cURL、PowerShell）

✅ **提供 HTML 頁面呼叫使用 API**（使用者列表:user-list.html, 新增:user-add.html, 編輯:user-edit.html）

✅ **提供 Swagger UI** 提供直觀的 API 測試介面 (使用 Dockerfile 安裝 Swashbuckle.AspNetCore，確保 Swagger 可用）

✅ **熱重載**：開發時支援 `dotnet watch run` ，程式碼異動會 hot-reload

---

## 📦 環境需求
✔ **Docker Desktop**（用於容器化 `.NET API` 和 `SQL Server`）  
✔ **.NET 8 SDK**（Docker Compose 啟動自動下載 mcr.microsoft.com/dotnet/sdk:8.0 與容器化部署.NET SDK）  
✔ **Postman**（可選，亦可使用 cURL，測試 REST API）

---

## **🔹 目錄結構**
```
.
├── README.md                    # 專案說明文件
├── docker-compose.yaml          # Docker 啟動設定檔(開發環境, 搭配 api/Dockerfile)
├── docker-compose.prod.yaml     # Docker 啟動設定檔(生產環境, 搭配 api/Dockerfile.Production)
├── api/                         # .NET API 原始碼
│   ├── Dockerfile               # 開發環境 .NET API 的 Dockerfile
│   ├── Dockerfile.Production    # 生產環境 .NET API 的 Dockerfile (多階段建置)
│   ├── appsettings.json         # 應用程式設定 (JWT 敏感資訊已移至 .env)
│   ├── api.csproj               # .NET 項目設定
│   ├── Program.cs               # 主 API 程式
│   ├── Models/                  # 子目錄:資料模型
│   │   ├── JwtSettings.cs          # JWT 設定強型別類別
│   ├── Middleware/              # 子目錄:中介軟體
│   │   ├── JwtAuthMiddleware.cs    # JWT 驗證自訂回應中介軟體
│   ├── Data/                    # 子目錄:資料庫上下文與遷移
│   │   ├── ApplicationDbContext.cs # EF Core 資料庫上下文
│   │   ├── Migrations/             # EF Core 遷移檔案
│   ├── Repositories/            # 子目錄:資料庫操作介面與實作
│   │   ├── IUserRepository.cs      # 使用者資料庫操作介面
│   │   ├── UserRepository.cs       # 使用者資料庫操作實作
│   ├── Services/                # 子目錄:服務類 程式
│   │   ├── DatabaseService.cs      # 資料庫 服務
│   │   ├── JwtService.cs           # JWT驗證 服務
│   │   ├── AuthService.cs          # 認證服務
│   │   ├── UserService.cs          # 使用者服務
│   ├── Controllers/             # 子目錄:API 控制器
│       ├── AuthController.cs       # 認證相關API
│       ├── UsersController.cs      # 使用者相關API
├── www/                         # html頁面(呼叫 users 相關REST-API功能測試)
│   ├── user-list.html           # 使用者 列表 html頁面
│   ├── user-add.html            # 使用者 新增 頁面
│   ├── user-edit.html           # 使用者 修改 頁面

```

---

## **🛠 安裝與運行**
### **1️⃣ 下載專案**
```sh
git clone https://github.com/cchhss123/dotnet_restapi_layered.git
cd dotnet_restapi_layered
```

### **2️⃣ 啟動 Docker (開發環境)**
使用 `docker-compose` 來啟動 SQL Server 2022 和 `.NET API`：
```sh
docker-compose up -d
```
✅ **成功啟動後：**
- `.NET API` 在 `http://localhost:3000`
- `API Swagger UI` 在 `http://localhost:3000/swagger`
- `Adminer` 在 `http://localhost:8080`

### **3️⃣ 啟動 Docker (生產環境)**
使用 `docker-compose.prod.yaml` 來啟動 SQL Server 2022 和 `.NET API`：
```sh
docker-compose -f docker-compose.prod.yaml up -d --build
```
✅ **成功啟動後：**
- `.NET API` 在 `http://localhost:3000`
- `API Swagger UI` 在 `http://localhost:3000/swagger`
- `Adminer` 在 `http://localhost:8080`

### **4️⃣ 資料庫初始化 (EF Core Migration)**
專案已配置 EF Core Migration，首次運行時會自動建立資料庫和表。
若需手動更新資料庫，請在 `api` 目錄下執行：
```sh
dotnet ef database update
```
---

## **🐳 Docker 部署方式詳解**

本專案提供兩種 Docker 容器部署方式，分別適用於生產環境和開發環境：

### **1️⃣ 生產環境部署 (Production)**

*   **Dockerfile**: [`api/Dockerfile.Production`](api/Dockerfile.Production)
*   **Docker Compose**: [`docker-compose.prod.yaml`](docker-compose.prod.yaml)

**機制與作用：**

*   **多階段建置 (Multi-stage builds)**：[`api/Dockerfile.Production`](api/Dockerfile.Production) 採用了多階段建置。
    *   **建置階段 (Build Stage)**：使用包含完整 .NET SDK 的映像 (`mcr.microsoft.com/dotnet/sdk:8.0`) 來編譯和發佈應用程式。這個階段會產生最佳化的發行版本。
    *   **執行階段 (Runtime Stage)**：使用僅包含 .NET 執行環境的較小映像 (`mcr.microsoft.com/dotnet/aspnet:8.0`) 來運行已發佈的應用程式。
*   **優勢**：
    *   **減小映像體積**：最終的生產映像只包含必要的執行環境和應用程式檔案，不包含編譯工具和原始碼，大幅縮小了映像大小。
    *   **提升安全性**：減少了攻擊面，因為生產映像中不包含開發工具和原始碼。
    *   **最佳化效能**：發佈的是最佳化後的應用程式版本。
*   [`docker-compose.prod.yaml`](docker-compose.prod.yaml) 配置了使用 [`api/Dockerfile.Production`](api/Dockerfile.Production) 來建置 `api` 服務，包含生產環境所需的配置。

### **2️⃣ 開發環境部署 (Development)**

*   **Dockerfile**: [`api/Dockerfile`](api/Dockerfile)
*   **Docker Compose**: [`docker-compose.yaml`](docker-compose.yaml)

**機制與作用：**

*   **單階段建置**：[`api/Dockerfile`](api/Dockerfile) 通常用於開發階段，它直接在包含 .NET SDK 的映像中建置並執行應用程式。
*   **快速迭代**：這個 Dockerfile 通常會配置 Volume Mounts，將本機的原始碼目錄掛載到容器中。這樣開發者在修改程式碼後，可以透過 `dotnet watch run` 等工具實現熱重載 (Hot Reload)，無需重新建置整個 Docker 映像即可看到變更，大幅提升開發效率。
*   **偵錯便利**：由於容器內包含 SDK，方便進行遠端偵錯等開發操作。
*   [`docker-compose.yaml`](docker-compose.yaml) 配置了使用 [`api/Dockerfile`](api/Dockerfile) 來建置 `api` 服務，並可能包含開發時所需的工具 (如 Adminer) 和相關的配置，以利於開發和測試。

---

##  API 測試（RESTful API）

使用 Postman、cURL 或 PowerShell 測試 API，或使用 Swagger UI `http://localhost:3000/swagger`

### 📌 API 端點：
|  方法  |  路徑                           | 描述          | 認證要求          |
|--------|--------------------------------|---------------|---------------|
| POST   | `/auth`                       | 認證API帳號，取得 JWT Token | ❌ 不需要 |
| GET    | `/users`                       | 取得所有使用者  | ❌ 不需要 |
| GET    | `/users/{id}`                  | 取得特定使用者  | ❌ 不需要 |
| POST   | `/users`                       | 新增使用者      | ❌ 不需要 |
| PUT    | `/users/{id}`                  | 更新使用者資料  | ✅ 需要 JWT |
| DELETE | `/users/{id}`                  | 刪除使用者    | ✅ 需要 JWT |


📌 PUT 和 DELETE 端點 需要 JWT Token 認證，請先執行 /auth 來取得 Token。
   執行API /auth 時，POST 測試帳密資料為 {"Account": "api", "Password": "test"}

### **📌 取得 JWT Token**
```sh
curl -X POST http://localhost:3000/auth -H "Content-Type: application/json" -d '{"Account": "api", "Password": "test"}'
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
$headers = @{ "Content-Type" = "application/json" }
Invoke-WebRequest -Uri "http://localhost:3000/auth" -Method POST -Headers $headers -Body '{"Account": "api", "Password": "test"}'
```

成功回應範例：

```json
{
  "message": "Login successful",
  "token": "your.jwt.token"
}
```
📌 請記住 your.jwt.token，後續 API 需要使用！


### **📌 取得所有使用者**
```sh
curl -X GET http://localhost:3000/users
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
Invoke-WebRequest -Uri "http://localhost:3000/users" -Method GET
```

### **📌 取得特定id使用者(例如:id=1)**
```sh
curl -X GET http://localhost:3000/users/1
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
Invoke-WebRequest -Uri "http://localhost:3000/users/1" -Method GET
```

### **📌 新增使用者**
```sh
curl -X POST http://localhost:3000/users -H "Content-Type: application/json" -d '{"name": "andrew","email": "cchhss123@hotmail.com"}'
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
$headers = @{ "Content-Type" = "application/json" }
Invoke-WebRequest -Uri "http://localhost:3000/users" -Method POST -Headers $headers -Body '{"name": "andrew","email": "cchhss123@hotmail.com"}'
```

### **📌 更新使用者（需要 JWT Token）**
```sh
curl -X PUT http://localhost:3000/users/1 -H "Authorization: Bearer your.jwt.token" -H "Content-Type: application/json" -d '{"name": "andy", "email": "andy@example.com"}'
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
$headers = @{ "Authorization" = "Bearer your.jwt.token"; "Content-Type" = "application/json" }
Invoke-WebRequest -Uri "http://localhost:3000/users/1" -Method PUT -Headers $headers -Body '{"name": "andy", "email": "andy@example.com"}'
```

### **📌 刪除使用者（需要 JWT Token）**
```sh
curl -X DELETE http://localhost:3000/users/6 -H "Authorization: Bearer your.jwt.token"
```
如果是 WINDOWS PowerShell 使用以下指令
```sh
$headers = @{ "Authorization" = "Bearer your.jwt.token" }
Invoke-WebRequest -Uri "http://localhost:3000/users/1" -Method DELETE -Headers $headers
```

---

## **使用html頁面呼叫使用API(使用者列表/使用者 新增 修改 刪除)**

瀏覽器 開啟 www/user-list.html 檔案，可進行相關API呼叫使用與測試，
包含: 使用者列表(含 使用者 編輯&刪除功能)，使用者新增 等功能。

---

## **🛠 docker容器 停止與刪除**
停止並刪除所有容器：
```sh
docker-compose down
```





