![version](https://img.shields.io/github/v/release/aichlou/neuronet?include_prereleases&logo=github&label=Latest%20Release)
![license](https://img.shields.io/github/license/aichlou/neuronet)
![issues](https://img.shields.io/github/issues/aichlou/neuronet)

# NeuroNet

NeuroNet is a from-scratch neural network framework in C# (.NET 8), built for learning, experimentation & architectural exploration.

# Stage of the Project

NeuroNet is currently in its **early pre-release stage**.

At this point, the project provides a **working core implementation** for creating, saving, loading, and running simple neural networks. The internal architecture is being actively refined and may change significantly between versions.

## What works
- Core neural network structure
- Network creation and execution
- Serialization and deserialization of networks
- Command-line based execution (CLI)

## What does not work yet
- Training / learning algorithms
- Public API stability
- Advanced network types or neuron variants
- External integrations or bindings

Breaking changes are expected. This pre-release is primarily intended for **development, experimentation, and architectural validation**, not for production use.

# Requirements

- .NET SDK 8.0 or newer
  [Download](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Git [Download for Windows](https://gitforwindows.org/)
- Functional Computer (optional) 

# Getting Started

How to run the Project:

## Linux & MacOS & Windows

Clone the Repository:
```
git clone https://github.com/aichlou/NeuroNet.git
```
Go into the folder and start the Programm
```
cd NeuroNet/src
dotnet run --project NeuroNet.CLI
```

# Roadmap



### Overview

- v0.1  Genesis
- v0.2  Structure
- v0.3  Learning 
- v0.4  Vision
- v0.5  Expansion
- v1.0  Stability
- v1.1  Ecosystem

### v0.1 — Core Functionality
Networks can be created, executed and stored.

<details>
<summary>Details</summary>

- create networks
- run networks
- customise networks
- save & load networks

</details>


### v0.2 — Architecture & Stability 
Internal restructuring and usability improvements.

<details>
<summary>Details</summary>

- split into `.Core` and `.CLI`
- improved CLI UI
- return feature
- stability improvements

</details>

### v0.3 — Learning <- On the way
Networks gain the ability to adapt.

<details>
<summary>Details</summary>

- automatic learning
- network editing
- internal library system

</details>

### v0.4 — Visualization
Better developer experience.

<details>
<summary>Details</summary>

- spectre.console UI
- network visualization

</details>

### v0.5 — Extensibility
The system becomes modular.

<details>
<summary>Details</summary>

- more neuron types
- more network architectures
- new functions
- redesigned structure

</details>

### v1.0 — Stable Release
Polished, documented, stable.

### v1.1 — Ecosystem
Public API and project website.

<br>

# License
MIT License - Copyright (c) 2026 Aichlou