# TreasureDragon Project Organization Guide

## 🎯 Quick Reference for Asset Import & Organization

### **Simplified Asset Categories**

#### **🏰 Military Assets** (Combat folder)
- **Towers**: Stone towers, watchtowers, defensive structures
- **Personnel**: Guards, soldiers, military characters

#### **🏘️ Civilian Assets** (Architecture folder)  
- **Buildings**: Houses, cabins, shops, civilian structures
- **People**: Regular civilians, merchants, workers, NPCs

---

## 📁 **Folder Structure Overview**

```
_Project/
├── Models/
│   ├── Combat/Military/
│   │   ├── Towers/          ← Military structures only
│   │   └── Personnel/       ← Guards & soldiers
│   └── Architecture/
│       ├── Buildings/Civilian/  ← Houses & civilian buildings
│       └── People/Civilians/    ← Regular people
├── Prefabs/
│   ├── Combat/Military/
│   │   ├── Towers/          ← Ready-to-use tower prefabs
│   │   └── Personnel/       ← Military character prefabs
│   └── Architecture/
│       ├── Buildings/Civilian/  ← Building prefabs
│       └── People/Civilians/    ← Civilian character prefabs
└── Scenes/
    ├── Environments/
    │   └── TreasureIsland_Main.unity  ← Main development scene
    ├── Testing/             ← Test & prototype scenes
    └── Archive/             ← Old scene versions
```

---

## 🎨 **Color-Coded Folders**

The custom Dragon Folder Icons system provides visual organization:

- **🔴 Combat folders** - Red/Orange theme (military assets)
- **🔵 Architecture folders** - Blue theme (civilian assets)  
- **🟣 Dragon folders** - Purple theme (player-related)
- **🟡 Scene folders** - Yellow theme (scene management)
- **⚫ Scripts/Tools** - Gray theme (development tools)
- **🟢 Asset folders** - Green theme (models, materials, etc.)

---

## 🚀 **Recommended Asset Import Workflow**

### **Step 1: Select Assets**
From Toon Adventure Island package, handpick only what you need:
- Select specific tower FBX files for military
- Choose building FBX files for civilian areas
- Pick character models for both categories

### **Step 2: Import to Models Folders**
Drag selected FBX files directly to appropriate folders:
- Towers → `_Project/Models/Combat/Military/Towers/`
- Buildings → `_Project/Models/Architecture/Buildings/Civilian/`
- Guards → `_Project/Models/Combat/Military/Personnel/`
- Civilians → `_Project/Models/Architecture/People/Civilians/`

### **Step 3: Create Scene Prefabs**
1. Open `TreasureIsland_Main.unity` scene
2. Drag models from Models folders into scene hierarchy
3. Position and configure as needed
4. **Use terrain snapping tool**: Select objects, press `Ctrl+T`
5. Drag configured objects to corresponding Prefabs folders

### **Step 4: Scene Organization**
Organize scene hierarchy like this:
```
TreasureIsland_Main:
├── 🏝️ ENVIRONMENT (terrain, water, natural features)
├── 🏰 MILITARY_BASES (towers, defensive positions)
├── 🏘️ CIVILIAN_AREAS (houses, people, shops)
├── 📊 GAME_SYSTEMS (managers, controllers)
└── 🧪 TESTING (temporary objects, experiments)
```

---

## 🛠️ **Dragon Toolkit Tools Available**

Access via `Tools > Dragon Toolkit`:

### **Terrain Tools**
- **Snap Objects to Terrain** - Full featured window with options
- **Quick Snap to Terrain** (`Ctrl+T`) - Instant snapping

### **Organization Tools**
- **Create Combat Structure** - Auto-create military folders
- **Create Architecture Structure** - Auto-create civilian folders  
- **Refresh Project Colors** - Update folder icon colors

---

## 💡 **Pro Tips**

1. **Keep it Simple**: Only import assets you'll actually use
2. **Use Terrain Snapping**: Always snap placed objects with `Ctrl+T`
3. **Create Prefabs Early**: Make prefabs as soon as objects are positioned correctly
4. **Visual Organization**: The color-coded folders make navigation much faster
5. **Scene Hierarchy**: Keep scene organized with clear parent groups

---

**🎯 Goal**: Clean, organized project structure that's easy to navigate and ready for AI combat system integration!