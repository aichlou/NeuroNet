# NeuroNet

NeuroNet is a from-scratch neural network framework in C# (.NET 8), built for learning, experimentation & architectural exploration.
This project is unfinished on purpose, because learning is the point.

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

# License
MIT License - Copyright (c) 2026 Aichlou