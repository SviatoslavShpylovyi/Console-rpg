#  Console-RPG

**An ASCII-based adventure game built in C#**  
Developed as part of the *Object-Oriented Design* course.

---

##  Overview

This project was created to apply object-oriented design patterns in a practical context through a playable console game.  
The primary objective was to explore how design patterns improve code organization, maintainability, and scalability.

---

##  Applied Design Patterns

The project integrates several classical design patterns:

| Pattern |
|----------|
| **Builder** | 
| **Factory** |
| **MVC (Model–View–Controller)** | 
| **Observer** | 
| **Singleton** | 
| **Strategy** |
|**few minor patterns**|

---

##  Technologies Used

| Category | Technology |
|-----------|-------------|
| **Language** | C# |
| **Networking** | TCP sockets |
| **Serialization** | JSON (for player and world data persistence) |
---

##  How to Run

```bash
# Clone the repository
git clone https://github.com/SviatoslavShpylovyi/Console-rpg.git

# Navigate to the project directory
cd Console-rpg/RPG

# Build the project
dotnet build

# Run the game
# Option 1: Local play (requires 2 instances of the terminal)
dotnet run

# Option 2: Multiplayer mode
dotnet run --player <ip:port_number>
dotnet run --server <port_number>
```
---
## How to play

| Command | Action |
|----------|---------|
| `I<index>R` | Equip item in **right hand** |
| `I<index>L` | Equip item in **left hand** |
| `I<index>D` | **Drop** an item from the inventory |
| `P<index>U` | **Drink** a potion |
| `H<index>D` | **Drop** an item from hand |

> **Note:**
> - `<index>` is **0-based** (e.g., `I0R` equips the first item).