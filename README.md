# SEIR Virus Simulation — Unity DOTS/ECS

<p align="center">
  <img src="https://img.shields.io/badge/Unity-DOTS%20%2F%20ECS-000000?style=for-the-badge&logo=unity&logoColor=white" />
  <img src="https://img.shields.io/badge/C%23-39%25-239120?style=for-the-badge&logo=csharp&logoColor=white" />
  <img src="https://img.shields.io/badge/Shaders-ShaderLab%20%2F%20HLSL-blue?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Model-SEIR-red?style=for-the-badge" />
  <img src="https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge" />
</p>

> Implementation of an **epidemic simulation** using Unity DOTS (Data-Oriented Technology Stack) and the ECS (Entity Component System) architecture, modeling the **SEIR** (Susceptible → Exposed → Infectious → Recovering) virus dynamics at scale.

---

## 🎬 Demo

<p align="center">
  <a href="https://www.youtube.com/watch?v=fIzTxpLmlbc">
    <img src="https://img.youtube.com/vi/fIzTxpLmlbc/0.jpg" alt="Watch the demo" width="640"/>
  </a>
</p>

<p align="center">
  <a href="https://www.youtube.com/watch?v=fIzTxpLmlbc">▶ Watch the presentation video on YouTube</a>
</p>

---

## 📖 Overview

**SEIR-Virus-Ecs** is a real-time epidemic simulation built on Unity's **DOTS** framework. Each individual in the population is represented as a lightweight ECS entity, making it possible to simulate large crowds while tracking the state transitions of every single agent with high accuracy.

The simulation implements the classic **SEIR compartmental model**, a standard in epidemiology, where every individual progresses through four states:

| State | Color | Description |
|-------|-------|-------------|
| **S** — Susceptible | 🟢 | Healthy individuals at risk of infection |
| **E** — Exposed | 🟡 | Infected but not yet contagious (incubation period) |
| **I** — Infectious | 🔴 | Actively spreading the disease to nearby agents |
| **R** — Recovering | 🔵 | Recovering and gaining immunity |

---

## ✨ Features

- 🦠 **Agent-based SEIR model** — each entity independently transitions through epidemic states based on proximity and probabilistic rules.
- ⚡ **Unity DOTS/ECS** — data-oriented architecture for cache-friendly, high-throughput simulation of thousands of agents.
- 🧵 **Burst-compiled Jobs** — state transition and movement logic run in parallel on multiple CPU cores.
- 🎨 **Custom Shaders (ShaderLab/HLSL)** — each agent's visual appearance updates dynamically to reflect its current epidemic state in real time.
- 🎛️ **Configurable Parameters** — infection rate, incubation period, recovery time, and population size all tunable from the Inspector.
- 📊 **Real-time Statistics** — live tracking of S/E/I/R population counts throughout the simulation.

---

## 🗂️ Project Structure

```
SEIR-Virus-Ecs/
├── Assets/
│   ├── Scripts/          # ECS Systems, Components, Authoring MonoBehaviours
│   ├── Shaders/          # Custom ShaderLab/HLSL shaders for state visualization
│   └── Scenes/           # Unity scenes
├── Packages/             # Unity package manifest (DOTS, Burst, Collections, etc.)
├── ProjectSettings/      # Unity project settings
└── LICENSE
```

---

## 🧠 How It Works

The simulation is composed of several ECS **Systems** running each frame:

1. **AgentSpawnerSystem** — spawns the initial population as ECS entities, assigning random positions and setting all agents to the `Susceptible` state (with a configurable number of initial `Infectious` seeds).
2. **MovementSystem** — moves each agent through the environment using Burst-compiled parallel jobs.
3. **InfectionSystem** — for each `Infectious` agent, checks proximity to `Susceptible` neighbours and applies a probabilistic transmission rate to trigger the `S → E` transition.
4. **StateTransitionSystem** — advances agents through the `E → I` and `I → R` transitions based on configurable timer components (incubation and recovery durations).
5. **VisualizationSystem** — updates each entity's shader parameters to reflect its current SEIR state through color and visual effects.

---

## 🚀 Getting Started

### Requirements

- Unity **2022.x** or newer (with DOTS packages support)
- Packages: `com.unity.entities`, `com.unity.burst`, `com.unity.collections`, `com.unity.mathematics`

### Setup

1. **Clone** the repository:
   ```bash
   git clone https://github.com/matteomarca99/SEIR-Virus-Ecs.git
   ```
2. **Open** the project in Unity Hub (Unity 2022.x+).
3. Unity will automatically restore packages from `Packages/manifest.json`.
4. **Open** the main scene from `Assets/Scenes/`.
5. Press **Play** to run the simulation.

---

## ⚙️ Configuration

Epidemic parameters can be adjusted in the Inspector on the **Simulation Settings** authoring component:

| Parameter | Description |
|-----------|-------------|
| `Population Size` | Total number of agents to spawn |
| `Initial Infected` | Number of agents starting in the `Infectious` state |
| `Infection Radius` | Proximity threshold for transmission to occur |
| `Transmission Rate` | Probability of infection per contact per frame |
| `Incubation Duration` | Time (seconds) spent in the `Exposed` state |
| `Recovery Duration` | Time (seconds) spent in the `Infectious` state before recovering |

---

## 📚 References

- Unity DOTS documentation — [docs.unity3d.com/Packages/com.unity.entities](https://docs.unity3d.com/Packages/com.unity.entities@latest)
- Unity Burst Compiler — [docs.unity3d.com/Packages/com.unity.burst](https://docs.unity3d.com/Packages/com.unity.burst@latest)

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Matteo Marcantoni** — [GitHub](https://github.com/matteomarca99)
