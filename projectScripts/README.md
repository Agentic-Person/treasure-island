# Project Scripts Library

Reusable Unity scripts that can be copied to any future project.

## 📁 Structure

```
projectScripts/
├── Editor/              # Unity Editor scripts
│   ├── DragonToolkit.cs # Vegetation optimizer + debugging tools
│   └── PolygonCounter.cs # Scene View polygon counter overlay ⭐ NEW!
└── Runtime/             # Future runtime scripts
```

## 🛠️ Available Scripts

### Editor Scripts

#### DragonToolkit.cs
**Purpose**: Comprehensive Unity Editor toolkit for project optimization and debugging

**Features**:
- 🌿 **Vegetation Optimizer** - Interactive sliders for reducing vegetation for prefab optimization
  - Grass, Plants, Trees, Palm Trees, Vines, Small Rocks
  - Percentage-based removal (0-50%)
  - Real-time object counts
- 🚨 **Emergency Fixes** - One-click fixes for common Unity issues
- 🔍 **Diagnostics** - Complete project health analysis
- 🔧 **Individual Fixes** - Targeted solutions for specific problems

**How to Use**:
1. Copy `DragonToolkit.cs` to `Assets/Scripts/Editor/` in your Unity project
2. Access via `Tools → Dragon Toolkit` in Unity Editor
3. Use Vegetation Optimizer sliders to reduce polygon count for prefabs

**Dependencies**: 
- UnityEngine
- UnityEditor  
- System.Collections.Generic
- System.Linq

**Tested With**: Unity 2022.3+ LTS

#### PolygonCounter.cs ⭐ NEW!
**Purpose**: Real-time polygon/triangle counter overlay for Scene View

**Features**:
- 🔺 **Scene View Overlay** - Always visible triangle count in Scene View
- 📊 **Real-time Updates** - Automatic count updates every 0.5 seconds
- 🎯 **Performance Indicators** - Color-coded performance warnings
- 📍 **Vertex Counting** - Optional vertex count display
- 🔄 **Manual Refresh** - Force update button for immediate counts
- 🟢🟡🔴 **Visual Feedback** - Green (good) to Red (high) performance indicators

**How to Use**:
1. Copy `PolygonCounter.cs` to `Assets/Scripts/Editor/` in your Unity project
2. **Scene View Overlay**: Automatically appears in Scene View (toggle in Overlays menu)
3. **Menu Access**: `Tools → Dragon Toolkit → Polygon Counter`
4. **Current Scene Count**: `Tools → Dragon Toolkit → Polygon Counter → Count Current Scene`

**Performance Thresholds**:
- 🟢 **< 25K triangles**: Excellent (all platforms)
- 🟡 **25K-50K**: Good (most platforms)  
- 🟠 **50K-100K**: Moderate (optimization recommended)
- 🔴 **100K-200K**: High (optimization needed)
- 🟣 **> 200K**: Very High (immediate optimization required)

**Dependencies**: 
- UnityEngine
- UnityEditor
- UnityEditor.Overlays (Unity 2022.2+)
- System.Linq

**Tested With**: Unity 2022.3+ LTS

## 🚀 Usage Instructions

### Adding to New Project
```bash
# Copy Editor scripts
cp projectScripts/Editor/*.cs YourProject/Assets/Scripts/Editor/

# Copy Runtime scripts (when available)
cp projectScripts/Runtime/*.cs YourProject/Assets/Scripts/
```

### Requirements
- Unity 2022.3 LTS or newer
- Scripts must be placed in appropriate folders:
  - Editor scripts → `Assets/Scripts/Editor/`
  - Runtime scripts → `Assets/Scripts/`

## 🎯 Perfect for File Size Control

The **PolygonCounter** is ideal for:
- 📏 **Monitoring scene complexity** during development
- 🌿 **Testing vegetation optimization** with DragonToolkit sliders
- 🎮 **WebGL optimization** (keep under 100K triangles)
- 📱 **Mobile performance** (keep under 50K triangles)
- 🔍 **Before/after optimization** comparison

## 📝 Notes

- All scripts are tested and working in PlasmaDragon project
- Update this README when adding new scripts
- Maintain Unity naming conventions
- Include dependency information for each script
- PolygonCounter overlay requires Unity 2022.2+ for Overlay system 