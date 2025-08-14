# 09a: Unity MCP Connection Setup for TreasureDragon

## Status: ✅ CORRECTED SETUP - Ready for Testing

## Priority: CRITICAL - Required for AI-assisted Unity development

## Description
Configure Unity MCP server connection for the new TreasureDragon project using the **correct CoderGamester/mcp-unity package** to enable AI-assisted Unity development through natural language commands.

## What Was Completed

### 1. ✅ Correct Unity MCP Package Installed
- **Removed**: Wrong `com.justinpbarnett.unity-mcp` package from manifest.json
- **Installed**: [CoderGamester/mcp-unity](https://github.com/CoderGamester/mcp-unity) at `/home/benton/bin/mcp-unity`
- **Unity Editor Scripts**: Copied to `Assets/Scripts/MCP-Unity/`
- **Node.js Server**: Installed at `/home/benton/bin/mcp-unity/Server~/`

### 2. ✅ MCP Server Dependencies Installed  
- Node.js dependencies installed in `/home/benton/bin/mcp-unity/Server~/`
- Server ready to run with `npm start`
- WebSocket-based architecture (not package-based)

### 3. ✅ Architecture Overview
- **Unity Editor**: WebSocket server runs inside Unity Editor
- **Node.js Bridge**: Acts as MCP client, connects to Unity via WebSocket
- **Cursor IDE**: Connects to Node.js server via MCP protocol
- **Flow**: Cursor → MCP → Node.js → WebSocket → Unity Editor

## How to Start the MCP Connection

### Step 1: Start Unity MCP Server
1. **Open Unity Editor** with TreasureDragon project
2. **Menu**: `Tools → MCP Unity → Server Window`
3. **Click**: "Start Server" button
4. **Verify**: Unity Console shows "WebSocket server started on port XXXX"

### Step 2: Start Node.js Bridge
```bash
cd /home/benton/bin/mcp-unity/Server~
npm start
```

### Step 3: Configure Cursor IDE
Add to your Cursor `settings.json`:
```json
{
  "mcp.servers": {
    "unity": {
      "command": "node",
      "args": ["/home/benton/bin/mcp-unity/Server~/src/index.js"],
      "cwd": "/home/benton/bin/mcp-unity/Server~"
    }
  }
}
```

## Connection Test Protocol

Once both servers are running, test these commands in Cursor:

### Basic Connection Test
```
"Can you see my Unity project?"
Expected: Project details and scene information
```

### Object Creation Test
```
"Create a cube named TestCube at position 0, 1, 0"
Expected: Cube appears in scene at specified position
```

### Scene Information Test  
```
"List the GameObjects in my current scene"
Expected: List of objects in current scene
```

### Camera Configuration Test
```
"Set main camera field of view to 60 and position at 0, 10, -10"
Expected: Camera repositioned with new FOV
```

## Project-Specific Configuration

### Project Details
- **Unity Version**: 6000.0.53f1 (Unity6000 installed)
- **Project Path**: `/home/benton/projects/Unity3d-Projects/TreasureDragon/TreasureIsland_Unity`
- **MCP Server Location**: `/home/benton/bin/mcp-unity/`
- **Connection**: WebSocket (Unity) ↔ Node.js ↔ MCP (Cursor)

### Required Components
- **Unity Scripts**: `Assets/Scripts/MCP-Unity/` (✅ Installed)
- **Node.js Server**: `/home/benton/bin/mcp-unity/Server~/` (✅ Installed) 
- **Dependencies**: npm packages (✅ Installed)

## Troubleshooting

### If Connection Fails
1. **Check Unity Console**: Look for "WebSocket server started" message
2. **Check Unity Menu**: Verify `Tools → MCP Unity → Server Window` exists
3. **Check Node.js Server**: Should show "MCP server listening on port..."
4. **Check Cursor**: MCP Unity server should appear in connected servers

### Common Issues
- **No Tools menu**: MCP-Unity scripts not properly imported
- **WebSocket error**: Unity server not started via Tools menu
- **Connection timeout**: Node.js server not running
- **No response**: Domain reload disabled (see FAQ below)

### Important: Domain Reload Setting
For Play Mode tests to work, disable domain reload:
- **Edit → Project Settings → Editor**
- **Enter Play Mode Settings** → Uncheck "Reload Domain"

## Commands to Test Immediately

```bash
# 1. Basic object creation
"Create a cube named DragonTestCube at position 0, 2, 0 with red material"

# 2. Environment setup  
"Create a terrain 100x100 units with grass texture"

# 3. Lighting setup
"Add directional light named SunLight with intensity 1.2 and warm color"

# 4. Camera setup
"Position main camera at 0, 5, -10 looking toward the origin with FOV 65"

# 5. Run tests
"Run all Play Mode tests in the project"
```

## Success Criteria
- ✅ Unity MCP Editor scripts loaded without errors
- ✅ Unity WebSocket server starts via Tools menu  
- ✅ Node.js MCP bridge connects to Unity
- ✅ Cursor recognizes Unity MCP server
- ✅ Basic object creation working
- ✅ Scene information retrieval working
- ✅ No Unity Console errors related to MCP

## Junior Developer Notes
- **Start Unity first**, then Node.js server, then test in Cursor
- **Use Tools menu** in Unity to start WebSocket server
- **Check both consoles** - Unity Console and terminal running Node.js
- **Domain reload must be disabled** for Play Mode test commands
- **WebSocket connection** - different from package-based approach

---

**Setup Corrected**: CoderGamester/mcp-unity properly installed and configured  
**Next Task**: Test MCP connection and proceed with TreasureDragon development tasks 