# Game Mechanics

### Project Page: *https://prokschf.github.io/OpenMobileTBS_V1/*


## Turns

The game is turn based. Every player in the game gets one turn after another in a set order.

There is a separate event at the beginning of each turn not associated with any player but for "world events"

## Game Map

The game map is modeled after a network (graph) consisting of nodes and edges. 

**MapTiles** represent a "field" where any game object can reside at a given turn.

**TravelConnections** represent possible movement between nodes. The **weight** of a TravelConnection determines the cost.

## Concepts

### Cities

Cities reside on a specific **MapTile**. They can *work* surrounding MapTiles each turn. This results in the accumulation of certain **Ressources**.
Cities can spend resources to produce **Units**.

### Units

Units reside on a specific **MapTile**. Only one unit can occupy a MapTile at a time. Units can perform certain actions. Units have 
- **Hit Points**
- **Movement**
- **Defense Strength**
- **Attack Strength**

#### Unit Actions

- **Move on Land**
    - Moving on land is associated with a certain movement cost that is determined by the **MapTile**
- **Move on Water**
- **Attack**
    - Units can attack by moving onto another unit of a different player. The Attack will be executed and 
- **Build a City** (Settlers only)
    - Consumes a unit and creates a new **City** on the spot


### Technologies

### Policies

## Ressources

Ressources are values that are produced by each players civilization. They are accumulated over time to achieve certain goals. **MapTiles** are 

- Food
- Production
- Gold
- Culture
- Science