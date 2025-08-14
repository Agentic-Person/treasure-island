# 🎯 Custom Vegetation Types - Dynamic Object Management

## ✨ NEW FEATURE: Add Your Own Object Types!

### 🚀 Quick Start
1. Open `Tools → Dragon Toolkit`
2. Scroll to **Vegetation Optimizer** section
3. Find **Custom Types** area
4. Click **➕ Add Custom Object Type**
5. Fill in:
   - **Name**: Display name (e.g., "Barrels")
   - **Icon**: Emoji or symbol (e.g., "🛢️")
   - **Search Term**: Text to find in object names (e.g., "barrel")
6. Click **Add Type**
7. Your custom type appears with its own slider!

## 🎚️ How It Works

### Adding Custom Types
When you add a custom type:
1. It's immediately available in the window with a slider
2. Settings are saved to EditorPrefs (persists between sessions)
3. A script is auto-generated with menu items
4. After Unity recompiles, menu items appear under `Tools/Dragon Toolkit/Vegetation/Custom/`

### Using Custom Types
- **Slider Control**: Adjust percentage and click "Remove"
- **X Button**: Delete the custom type
- **Quick Actions**: "Remove X% ALL" includes custom types
- **Menu Access**: After recompile, use menu items

## 📝 Examples

### Example 1: Adding Barrels
```
Name: Barrels
Icon: 🛢️
Search Term: barrel
```
This will find and manage all objects with "barrel" in their name.

### Example 2: Adding Crates
```
Name: Crates
Icon: 📦
Search Term: crate
```
This will find all crate objects.

### Example 3: Adding Debris
```
Name: Debris
Icon: 🗑️
Search Term: debris
```
This will manage all debris objects.

### Example 4: Adding Fences
```
Name: Fences
Icon: 🚧
Search Term: fence
```
This will handle all fence objects.

## 🔧 Advanced Features

### Auto-Generated Script
When you add custom types, a script is created at:
`Assets/Scripts/Editor/DragonToolkitCustomTypes.cs`

This script contains menu items like:
```csharp
[MenuItem("Tools/Dragon Toolkit/Vegetation/Custom/Barrels/Remove 10%")]
[MenuItem("Tools/Dragon Toolkit/Vegetation/Custom/Barrels/Remove 30%")]
[MenuItem("Tools/Dragon Toolkit/Vegetation/Custom/Barrels/Remove 50%")]
```

### Persistence
Custom types are saved to EditorPrefs with key:
`DragonToolkit_CustomVegetationTypes`

They persist between:
- Unity sessions
- Project reloads
- Script recompilations

### Integration
Custom types work with:
- **Remove X% ALL** buttons
- **Mobile Optimization** (if applicable)
- **Reset Sliders** function
- **Vegetation removal statistics**

## 🎨 Icon Suggestions

### Common Objects
- 🛢️ Barrels
- 📦 Crates/Boxes
- 🗑️ Debris
- 🚧 Fences
- 💎 Crystals
- 🏺 Vases/Pots
- 🪑 Furniture
- 🚪 Doors
- 🪟 Windows
- 🏮 Lanterns
- ⚓ Anchors
- 🎯 Targets
- 🛡️ Shields
- ⚙️ Gears
- 🧱 Bricks

### Nature Objects
- 🍄 Mushrooms
- 🌾 Wheat/Grain
- 🎋 Bamboo
- 🌵 Cacti
- 🪵 Logs
- 🍂 Leaves
- 🌸 Blossoms
- 🌻 Sunflowers
- 🌹 Roses
- 🦴 Bones

## 🔍 Search Term Tips

### Exact Matching
- Use exact prefixes: `TAI_` for specific assets
- Use full names: `Barrel_01` for specific objects

### Broad Matching
- Use partial terms: `bar` matches barrel, bar, rebar
- Use common parts: `rock` matches rocks, rocky, rockface

### Case Insensitive
All searches are case-insensitive:
- `barrel` matches Barrel, BARREL, BaRReL

## ⚡ Performance Tips

### Optimal Setup
- Add 3-5 custom types maximum
- Use specific search terms
- Remove unused custom types

### Batch Operations
- Use "Remove X% ALL" for multiple types
- Process similar objects together
- Save scene after bulk removals

## 🚨 Troubleshooting

### Custom Type Not Working?
1. Check search term matches object names
2. Verify objects are active in hierarchy
3. Look at console for removal statistics

### Menu Items Not Appearing?
1. Wait for Unity to recompile
2. Check `DragonToolkitCustomTypes.cs` exists
3. Look for compilation errors

### Slider Not Updating?
1. Click away and back to refresh
2. Check if objects exist in scene
3. Verify search term is correct

## 💡 Pro Tips

### Workflow Optimization
1. Add all needed custom types at once
2. Let Unity recompile once
3. Use menu items for quick access
4. Save custom type presets for different projects

### Common Use Cases
- **Props**: Barrels, crates, furniture
- **Clutter**: Debris, trash, small objects
- **Architecture**: Fences, walls, pillars
- **Effects**: Particles, decals, lights
- **Gameplay**: Collectibles, triggers, spawners

## 🎉 Benefits

### Flexibility
- Add ANY object type
- No code modification needed
- Works immediately

### Persistence
- Settings saved automatically
- Survives Unity restarts
- Project-independent storage

### Integration
- Works with all existing tools
- Included in batch operations
- Menu items auto-generated

### Efficiency
- No manual script editing
- Visual slider control
- Quick removal options

## 📋 Summary

The Custom Vegetation Types feature allows you to:
1. **Dynamically add** new object types to manage
2. **Use sliders** for precise control
3. **Generate menu items** automatically
4. **Save settings** between sessions
5. **Integrate seamlessly** with existing tools

Perfect for managing any type of scene object beyond the default vegetation and rocks!