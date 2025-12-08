# 🤖 AiAgentEconomy  
**Autonomous AI Agent Economy – AI Agents with Wallets & On-Chain Spending**  
C# .NET 9 · Semantic Kernel · Arbitrum Blockchain · Worker Runtime Architecture

---

## 📌 Vizyon

2026 sonrası dijital ekonomide yapay zekalar artık yalnızca konuşmayacak — **harcama yapacak.**  
Bu proje, yapay zeka ajanlarının kendi **cüzdanlarına sahip olduğu**, **bütçe yönettiği**, **servis satın aldığı** ve **otonom ekonomik aktörler haline geldiği** bir altyapı geliştirir.

> **AI Agent = Cüzdanı olan, karar verebilen, blockchain üzerinde işlem yapabilen dijital çalışan.**

Bu repo geleceğin **AI Agent Economy** dünyasının temelini atmak için oluşturulmuş açık geliştirme ortamıdır.

---

## 🚀 Amaçlar

- AI ajanları oluşturma ve yönetme
- Ajanlara **wallet** bağlama (Arbitrum Testnet)
- Harcama limitleri + kurallar (policy) tanımlayabilme
- Marketplace’ten servis tarama & satın alma algoritması
- **Semantic Kernel** ile karar verme + planlama
- Blockchain üzerinden ödeme / transfer

---

## 🏗 Mimarinin Büyük Resmi

AiAgentEconomy.sln
│
├─ src/
│ ├─ AiAgentEconomy.Domain # Core Entities, Enums, Value Objects
│ ├─ AiAgentEconomy.Application # Use Cases, Services, Interfaces
│ ├─ AiAgentEconomy.Infrastructure # EF Core, Repositories, Blockchain (Nethereum)
│ ├─ AiAgentEconomy.Api # ASP.NET Web API (CRUD + UI Gateway)
│ ├─ AiAgentEconomy.AgentRuntime # Worker Service -> Semantic Kernel Agent Loop
│ └─ AiAgentEconomy.Contracts # Shared DTOs/Events (opsiyonel)
│
└─ tests/
└─ AiAgentEconomy.Tests # Unit/Integration Tests

📌 **Mimari prensip**

| Katman | Sorumluluk | Bağımlılık |
|---|---|---|
| **Domain** | İşin kalbi | Kimseye bağlı değil |
| **Application** | Use-case ve arayüz sözleşmeleri | Domain’e bağlı |
| **Infrastructure** | DB, Blockchain, External IO | Domain + Application |
| **Api** | Public HTTP yüzü | Domain + Application + Infrastructure |
| **AgentRuntime** | Arka plan AI Agent çalışma motoru | Domain + Application + Infrastructure |

---

## 🧩 Ana Bileşenler

| Modül | Açıklama |
|---|---|
| **Domain** | Agent, Wallet, ServiceVendor, Policy, Transaction modelleri |
| **Application** | Agent oluşturma, Wallet yönetimi, Agent Engine service interface |
| **Infrastructure** | EF Core + Repository + Nethereum Blockchain Implementasyonu |
| **Api** | REST endpointler (Agent/Wallet/Service/Transaction) |
| **AgentRuntime** | Worker service → periyodik Agent çalıştırma loopu |
| **Semantic Kernel** | Karar verme & Tool bazlı işlem tetikleme |

---

## 🛠 Teknolojiler

- .NET 9 Web API
- Semantic Kernel (AI Reasoning)
- Entity Framework Core
- SQL / PostgreSQL esnek yapı
- Arbitrum Blockchain (Nethereum)
- Worker Service Background Loop
- Clean Architecture + SOLID

---
