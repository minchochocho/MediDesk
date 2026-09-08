# MediDesk

MediDesk는 환자, 진료 기록, 처방 정보를 관리하는 Windows 데스크톱 애플리케이션입니다. WPF 클라이언트가 ASP.NET Core Web API와 통신하고, API는 Entity Framework Core를 통해 PostgreSQL에 데이터를 저장합니다.

> 현재 개발 중인 프로젝트입니다. 아래의 [현재 확인된 제한 사항](#현재-확인된-제한-사항)을 먼저 확인하세요.

## 주요 기능

- 환자 목록 조회, 등록, 수정, 삭제
- 환자별 진료 기록 조회 및 등록
- 진료별 처방 목록 조회
- 환자 → 진료 → 처방으로 이어지는 데이터 관리
- 환자 또는 진료 삭제 시 하위 데이터 연쇄 삭제

현재 API에는 환자·진료·처방의 조회, 등록, 수정, 삭제 엔드포인트가 구현되어 있습니다. 클라이언트 UI에서 연결된 기능 범위는 API 전체 기능보다 제한적입니다.

## 기술 스택

- .NET 10
- WPF (`net10.0-windows`)
- ASP.NET Core Web API (`net10.0`)
- Entity Framework Core 10
- PostgreSQL / Npgsql
- OpenAPI 문서 생성

## 프로젝트 구조

```text
MediDesk/
├─ MediDesk.Api/              # REST API와 데이터베이스 접근
│  ├─ Controllers/            # Patient, Visit, Prescription API
│  ├─ Data/                   # EF Core DbContext
│  ├─ DTOs/                   # 요청 및 응답 모델
│  ├─ Migrations/             # 데이터베이스 마이그레이션
│  └─ Models/                 # 데이터 엔터티
├─ MediDesk.Client/           # WPF 데스크톱 클라이언트
│  ├─ Models/                 # API 응답 모델
│  ├─ Services/               # HttpClient 기반 API 호출
│  └─ Views/                  # 환자, 진료, 처방 화면
└─ MediDesk.slnx
```

## 실행 전 준비

다음 환경이 필요합니다.

- Windows
- .NET 10 SDK
- PostgreSQL
- EF Core CLI 도구 (`dotnet-ef`)

설치 상태를 확인합니다.

```powershell
dotnet --version
dotnet ef --version
```

`dotnet-ef`가 없다면 설치합니다.

```powershell
dotnet tool install --global dotnet-ef --version 10.*
```

## 데이터베이스 설정

1. PostgreSQL에 `medidesk_db` 데이터베이스를 생성합니다.
2. 연결 문자열을 설정합니다.

저장소의 `MediDesk.Api/appsettings.json`에는 로컬 연결 문자열이 들어 있습니다. 실제 비밀번호를 저장소에 커밋하지 말고, 개발 환경에서는 환경 변수 또는 .NET User Secrets 사용을 권장합니다.

현재 PowerShell 세션에만 연결 문자열을 지정하려면 다음과 같이 실행합니다.

```powershell
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Port=5432;Database=medidesk_db;Username=postgres;Password=<YOUR_PASSWORD>"
```

그다음 기존 마이그레이션을 적용합니다.

```powershell
dotnet ef database update --project MediDesk.Api
```

## 실행 방법

저장소 루트에서 패키지를 복원합니다.

```powershell
dotnet restore MediDesk.slnx
```

첫 번째 터미널에서 API를 실행합니다.

```powershell
dotnet run --project MediDesk.Api --launch-profile http
```

HTTP 프로필의 기본 API 주소는 `http://localhost:5200`입니다. 개발 환경의 OpenAPI 문서는 `http://localhost:5200/openapi/v1.json`에서 확인할 수 있습니다.

두 번째 터미널에서 WPF 클라이언트를 실행합니다.

```powershell
dotnet run --project MediDesk.Client
```

클라이언트의 서비스 클래스에는 API 기본 주소가 `http://localhost:5200/`으로 지정되어 있으므로, 다른 포트를 사용하려면 `MediDesk.Client/Services` 아래의 서비스 설정도 변경해야 합니다.

## API 경로

### 환자

- `GET /api/patients` — 전체 환자 조회
- `GET /api/patients/{id}` — 환자 한 명 조회
- `POST /api/patients` — 환자 등록
- `PUT /api/patients/{id}` — 환자 수정
- `DELETE /api/patients/{id}` — 환자 삭제

### 진료

- `GET /api/visits` — 전체 진료 기록 조회
- `GET /api/visits/{id}` — 진료 기록 한 건 조회
- `GET /api/visits/patient/{patientId}` — 환자별 진료 기록 조회
- `POST /api/visits` — 진료 기록 등록
- `PUT /api/visits/{id}` — 진료 기록 수정
- `DELETE /api/visits/{id}` — 진료 기록 삭제

### 처방

- `GET /api/prescriptions` — 전체 처방 조회
- `GET /api/prescriptions/visit/{visitId}` — 진료별 처방 조회
- `POST /api/prescriptions` — 처방 등록
- `PUT /api/prescriptions/{id}` — 처방 수정
- `DELETE /api/prescriptions/{id}` — 처방 삭제

## 데이터 관계

```text
Patient 1 ── N Visit 1 ── N Prescription
```

- 환자를 삭제하면 해당 환자의 진료 기록이 함께 삭제됩니다.
- 진료 기록을 삭제하면 해당 진료의 처방이 함께 삭제됩니다.

이는 EF Core 마이그레이션에 `Cascade` 삭제로 설정되어 있으므로 운영 데이터에 적용할 때 주의해야 합니다.

## 현재 확인된 제한 사항

- WPF 클라이언트는 현재 빌드되지 않습니다. `PrescriptionWindow`가 `PrescriptionAddWindow(visit)`를 호출하지만, `PrescriptionAddWindow`에는 매개변수를 받는 생성자가 구현되어 있지 않습니다.
- 처방 등록 창의 UI 및 저장 로직이 아직 구현되어 있지 않습니다.
- 환자 목록에서 진료 목록 창은 열리지만, 진료 목록에서 처방 목록 창으로 이동하는 이벤트는 현재 연결되어 있지 않습니다.
- 클라이언트의 API 주소가 각 서비스 클래스에 하드코딩되어 있습니다.
- API에 인증 및 권한 검사가 없습니다. 신뢰할 수 없는 네트워크나 실제 의료 데이터 환경에 그대로 배포하면 안 됩니다.
- 요청 DTO에 필수값, 길이, 범위 등의 서버 측 유효성 검사가 정의되어 있지 않습니다.
- 자동화 테스트 프로젝트가 없습니다.
- 저장소에 라이선스 파일이 없습니다.

## 보안 주의 사항

- `appsettings.json`에 실제 데이터베이스 비밀번호를 보관하거나 커밋하지 마세요.
- 이미 공유된 자격 증명이 있다면 PostgreSQL 비밀번호를 교체하고 Git 기록 노출 여부도 확인하세요.
- 실제 환자 정보를 다루기 전 인증, 권한 관리, 암호화, 감사 로그, 백업 및 관련 법규 준수 방안을 마련해야 합니다.
- API는 현재 HTTP 주소를 사용합니다. 운영 환경에서는 HTTPS를 구성하세요.

## 빌드 확인

API만 빌드하려면 다음 명령을 사용합니다.

```powershell
dotnet build MediDesk.Api/MediDesk.Api.csproj
```

전체 솔루션 빌드는 위에 적은 클라이언트 컴파일 오류가 해결된 뒤 실행할 수 있습니다.

```powershell
dotnet build MediDesk.slnx
```

## 라이선스

현재 별도의 라이선스가 명시되어 있지 않습니다.
