# Bitki Projesi

Bitki Projesi; bitki, taksonomi, etnobotanik, literatür ve ilgili veri kümelerini yöneten bir **ASP.NET Core Web API** arka ucu ile **Blazor** tabanlı bir ön yüzden oluşur.

## Proje Yapısı

```
./backend
  Bitki.Api            # ASP.NET Core Web API
  Bitki.Core           # Domain, arayüzler
  Bitki.Infrastructure # Veri erişimi, servisler
  Bitki.slnx           # Çözüm dosyası

./frontend
  Bitki.Blazor         # Blazor UI
```

## Ön Koşullar

- .NET SDK (proje sürümüyle uyumlu)
- PostgreSQL

## Kurulum

### 1) Veritabanı

`backend/Bitki.Api/appsettings.json` içindeki bağlantı dizesini kontrol edin:

```
Host=localhost;Port=5432;Database=botanik_pg;Username=postgres;Password=postgres
```

Gerekirse kendi ortamınıza göre güncelleyin.

### 2) Backend (API)

```bash
cd backend/Bitki.Api

dotnet restore

dotnet run
```

API varsayılan olarak `https://localhost:5001` üzerinden hizmet verir. Sağlık kontrolü için:

```
GET /health/db
```

### 3) Frontend (Blazor)

```bash
cd frontend/Bitki.Blazor

dotnet restore

dotnet run
```

Blazor uygulaması `appsettings.json` dosyasındaki `ApiSettings.BaseUrl` değerini kullanır. Backend URL’niz farklıysa bu değeri güncelleyin.

## Yapılandırma

- **Backend**: `backend/Bitki.Api/appsettings.json`
  - `ConnectionStrings.DefaultConnection`
  - `JwtSettings` (Issuer, Audience, SecretKey)
- **Frontend**: `frontend/Bitki.Blazor/appsettings.json`
  - `ApiSettings.BaseUrl`

## Geliştirme Notları

- Uygulama ilk çalışmada veritabanı seed işlemi yapar.
- JWT doğrulaması aktiftir; üretim ortamında `JwtSettings` değerlerini değiştirin.

## Lisans

Bu repo için lisans bilgisi belirtilmemiştir.
