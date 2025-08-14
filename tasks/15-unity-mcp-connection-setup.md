# Task 15: Unity MCP Connection Setup

## Task Overview
Successfully establish MCP (Model Context Protocol) connection between Claude Code and Unity Editor to enable AI-powered Unity development.

## Status: COMPLETED ✅

## Prerequisites
- Unity 6000.0.53f1 (Unity 6 LTS)
- Python 3.12+ installed
- Unity project open in Unity Editor
- Claude Code installed

## Implementation Steps Completed

### 1. Repository Setup
- Cloned Unity MCP server from GitHub: https://github.com/CoplayDev/unity-mcp
- Location: `/home/jimihacks/UnityProjects/TreasureDragon/unity-mcp-server`

### 2. Dependencies Installation
- Verified Python 3.13.5 installation
- Installed `uv` package manager via pip
- Server dependencies auto-installed on first run

### 3. Unity MCP Bridge
- Confirmed Unity MCP Bridge package already installed in Unity project
- Package: `com.coplaydev.unity-mcp` from GitHub

### 4. MCP Configuration
- Removed old/incorrect MCP configuration
- Added new Unity MCP server configuration to Claude Code
- Command: `claude mcp add unity -- uv --directory "/home/jimihacks/UnityProjects/TreasureDragon/unity-mcp-server/UnityMcpBridge/UnityMcpServer~/src" run server.py`

### 5. Connection Verification
- Tested server runs successfully
- Verified Unity Editor is running with project loaded
- Confirmed MCP connection status: ✓ Connected

## File Structure
```
TreasureDragon/
├── unity-mcp-server/
│   ├── UnityMcpBridge/
│   │   ├── UnityMcpServer~/
│   │   │   └── src/
│   │   │       ├── server.py (Main MCP server)
│   │   │       └── tools/ (Unity control tools)
│   │   └── Editor/ (Unity Bridge components)
│   └── README.md
├── TreasureIsland_Unity/ (Unity project)
│   └── Packages/
│       └── manifest.json (Contains Unity MCP Bridge reference)
└── .mcp.json (Local MCP configuration)
```

## Configuration Details
- **Server Path**: `unity-mcp-server/UnityMcpBridge/UnityMcpServer~/src/server.py`
- **Unity Port**: Auto-discovered via port file (typically 6400)
- **Connection Type**: stdio (standard input/output)
- **Unity Version**: 6000.0.53f1

## Testing Procedures
1. Run `claude mcp list` to verify connection status
2. Unity Editor must be open with project loaded
3. Server auto-connects to Unity on startup

## Available MCP Tools
Once connected, the following Unity control tools are available:
- `read_console`: Read Unity console messages
- `manage_script`: Create/modify C# scripts
- `manage_editor`: Control editor state
- `manage_scene`: Manage Unity scenes
- `manage_asset`: Handle Unity assets
- `manage_shader`: Create/modify shaders
- `manage_gameobject`: Manage GameObjects
- `execute_menu_item`: Execute Unity menu commands

## Troubleshooting Notes
- If connection fails, ensure Unity Editor is running first
- Port file is created by Unity Bridge when editor starts
- Server must be restarted if Unity Editor is restarted
- Check `unity-mcp-port.json` for port configuration

## Lessons Learned
1. Unity MCP Bridge must be installed in Unity before server can connect
2. Server path in MCP configuration must be absolute
3. The `UnityMcpServer~` folder contains the actual Python server (tilde indicates hidden from Unity)
4. Connection is established automatically when both Unity and MCP server are running

## Next Steps
- Use MCP tools to interact with Unity Editor
- Create game objects and scripts via AI commands
- Test automated Unity workflows

## Completion Checklist
- [x] Clone Unity MCP repository
- [x] Install Python dependencies
- [x] Verify Unity MCP Bridge in Unity
- [x] Configure Claude Code MCP settings
- [x] Test connection successfully
- [x] Document setup process