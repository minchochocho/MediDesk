# MediDesk

MediDesk는 환자, 진료 기록, 처방 정보를 관리하는 Windows 데스크톱 애플리케이션입니다. WPF 클라이언트가 ASP.NET Core Web API와 통신하고, API는 Entity Framework Core를 통해 PostgreSQL에 데이터를 저장합니다.

> 현재 개발 중인 프로젝트입니다. 아래의 [현재 확인된 제한 사항](#현재-확인된-제한-사항)을 먼저 확인하세요.

```text
WPF 화면 → HttpClient 서비스 → ASP.NET Core API → EF Core → PostgreSQL
```

포트폴리오 시연에는 실제 환자 정보가 아닌 가상 데이터만 사용합니다. 예시 흐름은 `환자 등록 → 진료 등록 → 진료 행 더블클릭 → 처방 등록 → 목록 재조회`입니다.

## 주요 기능

- 환자 목록 조회, 등록, 수정, 삭제
- 환자별 진료 기록 조회 및 등록
- 진료별 처방 목록 조회 및 등록
- 환자 → 진료 → 처방으로 이어지는 데이터 관리
- 환자 또는 진료 삭제 시 하위 데이터 연쇄 삭제

현재 API에는 환자·진료·처방의 조회, 등록, 수정, 삭제 엔드포인트가 구현되어 있습니다. WPF 클라이언트에서는 환자 목록에서 진료 목록을 열고, 진료 행을 더블클릭해 처방 목록과 처방 등록 창으로 이동할 수 있습니다. 클라이언트 UI에서 연결된 수정·삭제 기능 범위는 API 전체 기능보다 제한적입니다.

## 현재 구현 및 검증 상태

2026년 9월 17일 기준으로 다음 항목을 구현하거나 확인했습니다.

- `PrescriptionAddWindow`에 약 이름, 용량, 하루 복용 횟수, 복용 일수, 복용 안내 입력 UI 구현
- 처방 등록 시 필수 입력값과 양의 정수 여부를 클라이언트에서 검사
- 선택한 진료의 `VisitId`를 포함해 처방 등록 API를 호출하고, 성공 시 처방 목록을 다시 조회
- 진료 목록의 행을 더블클릭해 해당 진료의 처방 목록 창을 여는 흐름 연결
- 처방 생성·수정 DTO에 필수값, 문자열 길이, 숫자 범위 검증 추가
- 환자 생성·수정 DTO에 필수값, 문자열 길이, 생년월일 검증 추가
- 진료 생성·수정 DTO에 환자 번호, 필수값, 문자열 길이, 진료일 검증 추가
- 클라이언트의 환자 등록·수정·삭제, 진료 등록, 처방 등록에서 API 검증 오류를 사용자 메시지로 표시하도록 개선
- 환자·진료·처방 목록 조회 실패 시 내부 예외 문구 대신 연결·서버 오류 안내를 표시
- 잘못된 처방 생성 요청이 HTTP `400 Bad Request`와 필드별 검증 오류를 반환하는 것을 Postman에서 확인
- 유효한 처방 생성 요청이 생성된 처방 정보와 `PrescriptionId`를 반환하는 것을 Postman에서 확인
- API와 WPF 클라이언트 프로젝트가 각각 경고 및 오류 없이 빌드되는 것을 확인
- API DTO 유효성 검증 단위 테스트 6개 통과
- Windows 환경에서 복원·빌드·테스트를 실행하는 GitHub Actions 워크플로 추가(원격 실행 결과는 아직 확인 전)

처방 DTO의 현재 검증 범위는 다음과 같습니다.

- 진료 번호: 1 이상의 정수
- 약 이름: 필수, 최대 100자
- 용량: 필수, 최대 100자
- 하루 복용 횟수: 1~100
- 복용 일수: 1~3650
- 복용 안내: 최대 500자

복용 횟수와 복용 일수의 상한은 현재 방어적으로 정한 값이며, 확정된 의료 업무 규칙은 아닙니다.

환자 DTO의 현재 검증 범위는 다음과 같습니다.

- 환자 이름: 필수, 최대 100자
- 생년월일: 필수, 미래 날짜 금지
- 성별: 필수, 최대 20자
- 전화번호: 최대 30자
- 주소: 최대 300자

환자 관련 문자열 길이 제한은 현재 방어적으로 정한 값이며, 확정된 의료 업무 규칙은 아닙니다. 환자 API의 정상·실패 요청은 아직 Postman으로 별도 검증해야 합니다.

진료 DTO의 현재 검증 범위는 다음과 같습니다.

- 환자 번호: 생성 요청에서 1 이상의 정수
- 진료일: 필수(기본값 금지)
- 진료과·담당의: 필수, 각각 최대 100자
- 증상·진단·메모: 각각 최대 2000자

진료 관련 문자열 길이 제한은 현재 방어적으로 정한 값이며, 확정된 의료 업무 규칙은 아닙니다. 진료 요청 검증은 수동으로 확인했으며 자동화 테스트는 아직 없습니다.

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
├─ MediDesk.Api.Tests/        # DTO 유효성 검증 단위 테스트
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

저장소의 `MediDesk.Api/appsettings.json`에서 `DefaultConnection`은 비어 있습니다. 개발 환경에서는 `MediDesk.Api` 프로젝트에 설정된 .NET User Secrets에 `ConnectionStrings:DefaultConnection` 키로 로컬 연결 문자열을 저장하세요. Visual Studio에 **Manage User Secrets** 메뉴가 있으면 이를 사용하고, 메뉴가 없으면 `MediDesk.Api.csproj`의 `UserSecretsId`에 해당하는 로컬 User Secrets 파일을 열어 다음 형식으로 설정할 수 있습니다. User Secrets 파일은 저장소에 커밋하지 않습니다.

```json
{
  "ConnectionStrings:DefaultConnection": "Host=localhost;Port=5432;Database=medidesk_db;Username=postgres;Password=<YOUR_PASSWORD>"
}
```

대안으로 현재 PowerShell 세션에만 환경 변수를 지정할 수 있습니다. 명령 기록에 비밀번호가 남을 수 있으므로 공유 터미널에서는 주의하세요.

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

클라이언트의 기본 API 주소는 `http://localhost:5200/`이며 `MediDesk.Client/Services/ApiSettings.cs`에서 한 번만 정의합니다. 다른 주소를 사용하려면 클라이언트를 실행할 터미널에서 다음 환경 변수를 지정하세요.

```powershell
$env:MEDIDESK_API_BASE_URL = "http://localhost:5200/"
dotnet run --project MediDesk.Client
```

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

- 클라이언트 API 주소는 한 곳에서 설정할 수 있지만, 기본 실행 환경은 로컬 HTTP 주소입니다.
- API에 인증 및 권한 검사가 없습니다. 신뢰할 수 없는 네트워크나 실제 의료 데이터 환경에 그대로 배포하면 안 됩니다.
- 환자·진료·처방 생성·수정 DTO에는 기본적인 서버 측 유효성 검사가 있지만, 업무별 세부 규칙과 데이터베이스 제약은 아직 별도로 검토해야 합니다.
- 처방 수정·삭제 기능은 API에 구현되어 있지만 WPF 클라이언트 UI에는 아직 연결되어 있지 않습니다.
- 처방의 중복 등록을 판별하거나 방지하는 업무 규칙이 정의되어 있지 않습니다.
- 자동화 테스트는 DTO 유효성 검증에 한정됩니다. DB를 포함한 API 통합 테스트와 WPF UI 테스트는 아직 없습니다.
- 클라이언트 오류 안내는 빌드로 확인했으며, 각 실패 상황의 WPF 화면 동작은 수동 검증이 필요합니다.
- 저장소에 라이선스 파일이 없습니다.

## 보안 주의 사항

- `appsettings.json`에 실제 데이터베이스 비밀번호를 보관하거나 커밋하지 마세요.
- 이전 커밋에 데이터베이스 연결 문자열이 포함되어 있었습니다. 해당 자격 증명이 실제로 사용 중이라면 PostgreSQL 비밀번호를 교체하고 Git 기록 노출 여부도 확인하세요. 현재 파일에서 제거해도 이전 기록은 사라지지 않습니다.
- 실제 환자 정보를 다루기 전 인증, 권한 관리, 암호화, 감사 로그, 백업 및 관련 법규 준수 방안을 마련해야 합니다.
- API는 현재 HTTP 주소를 사용합니다. 운영 환경에서는 HTTPS를 구성하세요.

## 빌드 및 테스트

API와 WPF 클라이언트를 각각 빌드하려면 다음 명령을 사용합니다.

```powershell
dotnet build MediDesk.Api/MediDesk.Api.csproj
dotnet build MediDesk.Client/MediDesk.Client.csproj
```

전체 솔루션은 다음 명령으로 빌드합니다. 이 환경에서는 병렬 솔루션 빌드가 간헐적으로 오류 메시지 없이 종료되어 단일 MSBuild 노드 옵션을 사용합니다.

```powershell
dotnet build MediDesk.slnx -m:1
dotnet test MediDesk.Api.Tests/MediDesk.Api.Tests.csproj
```

테스트는 환자·진료·처방 생성 DTO의 필수값, 날짜, 숫자 범위와 정상값을 검사하며 PostgreSQL 연결은 필요하지 않습니다. 실제 API 응답과 WPF 화면은 별도로 수동 확인해야 합니다.

`.github/workflows/ci.yml`은 푸시와 PR에서 같은 빌드·테스트를 수행하도록 설정했습니다. 워크플로가 실제로 통과하는지는 GitHub에 반영한 뒤 확인해야 합니다.

## 라이선스

현재 별도의 라이선스가 명시되어 있지 않습니다.
