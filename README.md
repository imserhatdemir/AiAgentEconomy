# AiAgentEconomy — Autonomous Agent MVP

This repo provides an MVP foundation for an autonomous AI agent that owns a wallet, enforces spending policies, and purchases services from a marketplace. It is written to support both a quick-start build and an accompanying article, with a clear view of architecture, usage, and test flows.

## Purpose
- Give agents their own blockchain wallet and budget control.
- Discover and purchase marketplace services (ServicePurchase).
- Enforce spending policies (limits/allowlists) for safe spending.
- Model the on-chain submission flow (currently mocked) end to end.

## Architecture Overview
- **Clean Architecture layers**: Domain, Application, Infrastructure, API, AgentRuntime, Contracts.
- **Domain**: Agent, Wallet, AgentPolicy, Transaction, Marketplace (ServiceVendor/MarketplaceService), audit/ID base types.
- **Application**: Use-case services (`AgentService`, `WalletService`, `TransactionService`, `TransactionLifecycleService`, `AgentPolicyService`, `MarketplaceReadService`), repo/sender/verifier interfaces, custom exceptions.
- **Infrastructure**: EF Core DbContext + config, seed data, fake blockchain sender/verifier, dependency injection.
- **API**: ASP.NET Core Web API, Swagger, global exception middleware; controllers for Agent, Wallet, Policy, Transaction, Marketplace.
- **AgentRuntime**: Worker/loop scaffold (prepared for Semantic Kernel integration).
- **Contracts**: DTOs and request models (Agents, Wallets, Policies, Transactions, Marketplace).

## Technologies
- .NET 9, ASP.NET Core Web API
- EF Core (PostgreSQL target); `docker-compose` for Postgres + RabbitMQ (future use)
- Semantic Kernel (agent planning/decision baseline)
- Nethereum (planned); currently mocked sender/verifier

## Run
1) Requirements: .NET 9 SDK, Docker (for Postgres).
2) Database: `docker-compose up -d postgres` (optional RabbitMQ: `docker-compose up -d rabbitmq`).
3) API: from repo root `dotnet restore`, then `dotnet run --project AiAgentEconomy.API/AiAgentEconomy.API.csproj`.
4) Seed: auto on API start; VendorA(Service1=5 USDC, Service2=12), VendorB(ComputeBasic=3) are inserted.

## Example API Flow (Happy Path)
1) Create agent  
`POST /api/agents`
```json
{ "name": "Runtime Agent", "description": "Marketplace PoC" }
```
2) Set monthly budget  
`PUT /api/agents/{agentId}/budget`
```json
{ "monthlyBudget": 50 }
```
3) Add wallet  
`POST /api/agents/{agentId}/wallet`
```json
{ "chain": "Arbitrum", "address": "0x1111111111111111111111111111111111111111", "type": "NonCustodial" }
```
4) Define policy (recommended)  
`PUT /api/agents/{agentId}/policy`
```json
{
  "name": "Default Spending",
  "isActive": true,
  "maxPerTransaction": 10,
  "dailyLimit": 20,
  "currency": "USDC",
  "allowedVendorsCsv": "VendorA,VendorB",
  "allowedServicesCsv": "Service1,ComputeBasic"
}
```
5) Create transaction (expected to be approved)  
`POST /api/agents/{agentId}/transactions`
```json
{
  "amount": 5,
  "type": "ServicePurchase",
  "currency": "USDC",
  "vendor": "VendorA",
  "serviceCode": "Service1"
}
```
   - Expectation: `status` = `Approved`, `vendorId` and `marketplaceServiceId` populated, `rejectionReason` empty.
6) Submit on-chain (mock)  
`POST /api/transactions/{transactionId}/submit`
```json
{ "chain": "Arbitrum", "network": "arbitrum-sepolia", "explorerUrl": null }
```
7) Settle  
`POST /api/transactions/{transactionId}/settle`

## Business Rules (Create + Lifecycle)
- Reject if agent is not active or `SpentThisMonth + Amount > MonthlyBudget`.
- For `ServicePurchase`: marketplace vendor/service must exist and be active; currency and price must exactly match.
- If a policy exists: must be active, currency-aligned, and `CanSpend` (per-tx, daily limit, allowlist) must pass. On pass: `Approve` + `AddSpent` + `AddDailySpend`.
- If no policy: monthly budget suffices; approve.
- Otherwise: reject and set a reason.

## Test Scenarios (manual)
- **Happy path**: Flow above (VendorA/Service1 = 5 USDC) → `Approved`.
- **Policy allowlist violation**: vendor outside allowlist → `Rejected`, reason `VENDOR_NOT_ALLOWED`.
- **Price mismatch**: different `amount` for the same service → `Rejected`, reason `MARKETPLACE_PRICE_MISMATCH`.
- **Budget exceeded**: `monthlyBudget` < `amount` → `Rejected`, reason `MONTHLY_BUDGET_EXCEEDED`.
- **Daily limit exceeded**: policy `dailyLimit` < (daily cumulative + new amount) → `Rejected`, reason `DAILY_LIMIT_EXCEEDED`.

## Roadmap (post-MVP)
- Real Nethereum integration, chain config, explorer URL generation.
- AgentRuntime planning/action loop (Semantic Kernel), queue-based orchestration (RabbitMQ).
- Direct vendor wallet routing and multi-currency support.
- Test expansion: integration tests, policy edge cases, marketplace snapshot validation.
