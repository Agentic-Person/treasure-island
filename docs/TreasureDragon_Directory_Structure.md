# 🗂️ TreasureDragon Directory Structure Reference

## 🎯 **Complete Asset Organization with Sweet Icons**

```
Assets/_Project/
├── 🎬 Scenes/
│   ├── 👑 Masters/
│   │   └── MasterGame_Orchestrator.unity     ← 🎮 Final integration scene
│   ├── 🌍 Environments/
│   │   ├── EnvironmentWorkshop.unity         ← 🔧 Asset assembly scene (CREATE HERE!)
│   │   └── IslandCombatEnvironment.unity     ← ✅ Finalized environment
│   └── 🧪 Testing/
│       └── DragonFlightTest.unity            ← 🐉 Your existing dragon scene
├── 🏗️ Models/
│   ├── 🏘️ Architecture/                      ← Civilian/environment assets
│   │   ├── 🏠 Buildings/
│   │   │   ├── 🏡 Houses/
│   │   │   ├── 🛍️ Shops/
│   │   │   ├── ⛩️ Temples/
│   │   │   └── 🏗️ Infrastructure/
│   │   ├── 👥 People/
│   │   │   ├── 👨‍👩‍👧‍👦 Civilians/
│   │   │   ├── 🤖 NPCs/
│   │   │   ├── 🤝 Merchants/
│   │   │   └── 👷 Workers/
│   │   └── 📦 Props/
│   │       ├── 🪑 Furniture/
│   │       ├── 🎨 Decorations/
│   │       ├── 🔨 Tools/
│   │       └── 🚗 Vehicles/
│   ├── ⚔️ Combat/                             ← Military/combat assets
│   │   ├── 🏰 Buildings/
│   │   │   ├── 🗼 Towers/
│   │   │   ├── 🛡️ Fortifications/
│   │   │   └── ⚔️ MilitaryStructures/
│   │   ├── 👨‍💼 Units/
│   │   │   ├── 🗡️ Soldiers/
│   │   │   ├── 🏹 Archers/
│   │   │   └── 💂 Guards/
│   │   └── 🔫 Weapons/
│   │       ├── 🎯 TowerWeapons/
│   │       └── ⚔️ UnitWeapons/
│   ├── 🌲 Environment/                        ← Natural environment assets
│   │   ├── 🗺️ Terrain/
│   │   │   ├── 🏝️ Islands/
│   │   │   ├── 🌄 Landscapes/
│   │   │   └── 📊 Heightmaps/
│   │   ├── 🌿 Vegetation/
│   │   │   ├── 🌳 Trees/
│   │   │   ├── 🌲 Bushes/
│   │   │   ├── 🌱 Grass/
│   │   │   └── 🌺 Flowers/
│   │   └── 🪨 Natural_Props/
│   │       ├── 🗿 Rocks/
│   │       ├── 💧 Water_Features/
│   │       └── ⛰️ Cliffs/
│   └── 🐉 Dragon/
│       ├── 🎭 Models/
│       ├── 🎬 Animations/
│       └── ✨ Effects/
├── 🎨 Materials/
│   ├── 🏘️ Architecture/
│   │   ├── 🧱 BuildingMaterials/
│   │   ├── 👔 PeopleMaterials/
│   │   └── 📦 PropMaterials/
│   ├── ⚔️ Combat/
│   │   ├── 🏰 BuildingMaterials/
│   │   ├── 👤 UnitMaterials/
│   │   └── 🔫 WeaponMaterials/
│   ├── 🌲 Environment/
│   │   ├── 🗺️ TerrainMaterials/
│   │   ├── 🌿 VegetationMaterials/
│   │   └── 💧 WaterMaterials/
│   └── 🐉 Dragon/
├── 🖼️ Textures/
│   ├── 🏘️ Architecture/
│   ├── ⚔️ Combat/
│   ├── 🌲 Environment/
│   └── 🐉 Dragon/
├── 🧩 Prefabs/
│   ├── 🏘️ Architecture/
│   │   ├── 🏠 Buildings/
│   │   ├── 👥 People/
│   │   └── 📦 Props/
│   ├── ⚔️ Combat/
│   │   ├── 👨‍💼 Units/
│   │   ├── 🗼 Towers/
│   │   ├── 👥 Squads/
│   │   └── 🔗 Networks/
│   ├── 🌲 Environment/
│   │   ├── 🗺️ Terrain_Sections/
│   │   ├── 🌿 Vegetation_Groups/
│   │   └── 🪨 Natural_Features/
│   └── 🐉 Dragon/
│       ├── 🎭 DragonPrefabs/
│       └── ✨ DragonEffects/
├── 🔊 Audio/
│   ├── 🏘️ Architecture/
│   ├── ⚔️ Combat/
│   ├── 🌲 Environment/
│   └── 🐉 Dragon/
└── 📥 ImportedAssets/                        ← Staging area for Asset Store imports
    ├── 🏝️ AssetStore_Islands/
    ├── 👥 AssetStore_Characters/
    └── 🌍 Environment_Import/
```

---

## 🎯 **Quick Scene Creation Guide**

### **Step 1: Create EnvironmentWorkshop Scene**
```
In Unity:
1. File → New Scene
2. Save As: Assets/_Project/Scenes/Environments/EnvironmentWorkshop.unity
3. This is your 🔧 ASSET ASSEMBLY scene
```

### **Step 2: Scene Purpose**
```
🔧 EnvironmentWorkshop.unity:
✅ Import Asset Store island prefab here
✅ Reduce tree/vegetation complexity
✅ Add buildings and characters
✅ Create organized prefabs
✅ Experiment safely without breaking anything
```

### **Step 3: Workflow**
```
🏝️ Import island → 🔧 Workshop Scene → 🌿 Optimize vegetation → 🏠 Add buildings → 
👥 Add characters → 🧩 Create prefabs → ✅ Finalize as IslandCombatEnvironment.unity
```

---

## 🎮 **Scene Categories Explained**

### **🌍 Environments/** 
- **EnvironmentWorkshop.unity** ← 🔧 CREATE YOUR SCENE HERE!
- **IslandCombatEnvironment.unity** ← ✅ Final optimized environment

### **👑 Masters/**
- **MasterGame_Orchestrator.unity** ← 🎮 Final game integration

### **🧪 Testing/**
- **DragonFlightTest.unity** ← 🐉 Your existing dragon scene

---

## 🎨 **Icon Legend**

| Icon | Category | Purpose |
|------|----------|---------|
| 🏘️ | Architecture | Civilian buildings, people, props |
| ⚔️ | Combat | Military units, towers, weapons |
| 🌲 | Environment | Natural terrain, vegetation, rocks |
| 🐉 | Dragon | Dragon models, animations, effects |
| 🔧 | Workshop | Safe experimentation space |
| ✅ | Production | Finalized, optimized assets |
| 📥 | Staging | Temporary import area |

---

**Create your EnvironmentWorkshop.unity scene in the 🌍 Environments/ folder and start assembling your island paradise!** 🎯 