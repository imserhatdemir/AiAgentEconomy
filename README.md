# AiAgentEconomy

**Autonomous AI Agent Economy – AI Agents with Wallets & On-Chain Spending**
C# .NET 9 · Semantic Kernel · Arbitrum Blockchain · Worker Runtime Architecture

---

## Vision

In the post-2026 digital economy, artificial intelligence will not only communicate but also have the ability to spend. This project aims to develop an infrastructure where AI agents own their own wallets, manage budgets, purchase services, and operate as autonomous economic entities.

**AI Agent = A digital worker capable of decision-making and executing transactions on the blockchain with its own wallet.**

This repository is created as an open development environment that lays the foundation for the future of the AI Agent Economy.

---

## Objectives

* Create and manage AI agents
* Connect blockchain wallets to agents (Arbitrum Testnet)
* Define spending policies and limits
* Scan marketplace services and perform autonomous purchasing
* Execute decision-making and planning using Semantic Kernel
* Process payments and transfers via blockchain

---

## Architecture Overview

```
AiAgentEconomy.sln
│
├─ src/
│ ├─ AiAgentEconomy.Domain              # Core Entities, Enums, Value Objects
│ ├─ AiAgentEconomy.Application         # Use Cases, Services, Interfaces
│ ├─ AiAgentEconomy.Infrastructure      # EF Core, Repositories, Blockchain (Nethereum)
│ ├─ AiAgentEconomy.Api                 # ASP.NET Web API (CRUD + Gateway)
│ ├─ AiAgentEconomy.AgentRuntime        # Worker Service -> Semantic Kernel Agent Loop
│ └─ AiAgentEconomy.Contracts           # Shared DTOs/Events (optional)
│
└─ tests/
   └─ AiAgentEconomy.Tests              # Unit/Integration Tests
```

### Architectural Principles

| Layer          | Responsibility                                    | Dependency                                       |
| -------------- | ------------------------------------------------- | ------------------------------------------------ |
| Domain         | Core business layer                               | No dependencies                                  |
| Application    | Use cases and service abstractions                | Depends on Domain                                |
| Infrastructure | Database, Blockchain, External IO implementations | Depends on Domain + Application                  |
| Api            | Public HTTP interface                             | Depends on Domain + Application + Infrastructure |
| AgentRuntime   | Background AI agent processing engine             | Depends on Domain + Application + Infrastructure |

---

## Core Components

| Module          | Description                                                             |
| --------------- | ----------------------------------------------------------------------- |
| Domain          | Models for Agent, Wallet, ServiceVendor, Policy, Transaction            |
| Application     | Agent creation, wallet management, orchestration interfaces             |
| Infrastructure  | EF Core, Repository structure, Blockchain implementation with Nethereum |
| Api             | REST endpoints (Agent/Wallet/Service/Transaction)                       |
| AgentRuntime    | Worker service for scheduled agent execution loop                       |
| Semantic Kernel | Decision making and tool-based task execution                           |

---

## Technologies

* .NET 9 Web API
* Semantic Kernel (AI Reasoning)
* Entity Framework Core
* SQL / PostgreSQL compatible structure
* Arbitrum Blockchain via Nethereum
* Worker Service Background Runtime
* Clean Architecture pattern & SOLID principles

---
