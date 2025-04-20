# Unity MCP (Server + Plugin)

[![openupm](https://img.shields.io/npm/v/com.ivanmurzak.unity.mcp?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.ivanmurzak.unity.mcp/) ![License](https://img.shields.io/github/license/IvanMurzak/Unity-MCP) [![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/badges/StandWithUkraine.svg)](https://stand-with-ukraine.pp.ua)

![image](https://github.com/user-attachments/assets/8f595879-a578-421a-a06d-8c194af874f7)


| Unity Version | Editmode | Playmode | Standalone |
|---------------|----------|----------|------------|
| 2022.3.61f1   | ![2022.3.61f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2022.3.61f1_editmode.yml?label=2022.3.61f1-editmode) | ![2022.3.61f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2022.3.61f1_playmode.yml?label=2022.3.61f1-playmode) | ![2022.3.61f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2022.3.61f1_standalone.yml?label=2022.3.61f1-standalone) |
| 2023.2.20f1   | ![2023.2.20f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2023.2.20f1_editmode.yml?label=2023.2.20f1-editmode) | ![2023.2.20f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2023.2.20f1_playmode.yml?label=2023.2.20f1-playmode) | ![2023.2.20f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/2023.2.20f1_standalone.yml?label=2023.2.20f1-standalone) |
| 6000.0.46f1   | ![6000.0.46f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/6000.0.46f1_editmode.yml?label=6000.0.46f1-editmode) | ![6000.0.46f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/6000.0.46f1_playmode.yml?label=6000.0.46f1-playmode) | ![6000.0.46f1](https://img.shields.io/github/actions/workflow/status/IvanMurzak/Unity-MCP/6000.0.46f1_standalone.yml?label=6000.0.46f1-standalone) |

**[Unity-MCP](https://github.com/IvanMurzak/Unity-MCP)** is a bridge between LLM and Unity. It exposes and explains to LLM Unity's tools. LLM understands the interface and utilizes the tools in the way a user asks.

Connect **[Unity-MCP](https://github.com/IvanMurzak/Unity-MCP)** to LLM client such as [Claude](https://claude.ai/download) or [Cursor](https://www.cursor.com/) using integrated `AI Connector` window. Custom clients are supported as well.

The project is designed to let developers to add custom tools soon. After that the next goal is to enable the same features in player's build. For not it works only in Unity Editor.

The system is extensible: you can define custom `tool`s directly in your Unity project codebase, exposing new capabilities to the AI or automation clients. This makes Unity-MCP a flexible foundation for building advanced workflows, rapid prototyping, or integrating AI-driven features into your development process.

## AI Tools

<table>
<tr>
<td valign="top">

### GameObject

- ✅ Create
- ✅ Destroy
- ✅ Find
- 🔲 Modify (tag, layer, name, static)

##### GameObject.Components
- ✅ Add Component
- ✅ Destroy Component
- ✅ Modify Component
- - ✅ `Field` set value
- - ✅ `Property` set value
- - ✅ `Reference` link set
- 🔲 Remove missing components

### Editor

- ✅ State (Playmode)
  - ✅ Get
  - ✅ Set
- 🔲 Get Windows
- 🔲 Selection
  - 🔲 Get Selected
  - 🔲 Copy
  - 🔲 Paste
- 🔲 Layer
  - 🔲 Get All
  - 🔲 Add
  - 🔲 Remove
- 🔲 Tag
  - 🔲 Get All
  - 🔲 Add
  - 🔲 Remove
- 🔲 Execute `MenuItem`
- 🔲 Run Tests

### Scriptable Object

- 🔲 Create
- 🔲 Read
- 🔲 Modify
- 🔲 Remove

</td>
<td valign="top">

### Assets

- ✅ Search
- ✅ Refresh
- 🔲 Import
- 🔲 Read
- 🔲 Modify
- 🔲 Rename
- 🔲 Remove
- 🔲 Create folder

### Scene

- ✅ Get hierarchy
- 🔲 Create scene
- 🔲 Save scene
- 🔲 Open scene
- 🔲 Search (Editor)
- 🔲 Raycast (understand volume)

### Materials

- 🔲 Create
- 🔲 Update
- ✅ Assign to a Component on a GameObject

### Scripts

- ✅ Read
- ✅ Update or Create
- ✅ Delete

### Component

- ✅ Get All

### Prefabs

- ✅ Instantiate
- 🔲 Create from scene

</td>
</tr>
</table>

> **Legend:**
> ✅ = Implemented & available
> 🔲 = Planned / Not yet implemented

# Installation

1. [Install .NET 9.0](https://dotnet.microsoft.com/en-us/download)
2. [Install OpenUPM-CLI](https://github.com/openupm/openupm-cli#installation)

- Open command line in Unity project folder
- Run the command

```bash
openupm add com.ivanmurzak.unity.mcp
```

# Usage

1. Go 👉 `Window/AI Connector (Unity-MCP)`.
2. Click configure on your MCP client.
3. Restart your MCP client.
4. Make sure `AI Connector` is "Connected" after restart.
5. Test AI connection in your Client (Cursor, Claude Desktop). Type any question or task into the chat. Something like:
```
Explain my scene hierarchy
```

# Add custom `tool`

> ⚠️ Not yet supported. There is a blocker issue in `csharp-sdk` for MCP server. Waiting for solution.
> Please vote up for [this issue](https://github.com/modelcontextprotocol/csharp-sdk/discussions/301) and [this](https://github.com/modelcontextprotocol/csharp-sdk/issues/335) to bring more attention to it. The custom tool is dependent on it.

Unity-MCP is designed to support custom `tool` development by project owner. MCP server takes data from Unity plugin and exposes it to a Client. So anyone in the MCP communication chain would receive the information about a new `tool`. Which LLM may decide to call at some point.

To add a custom `tool` you need:

1. To have a class with attribute `McpPluginToolType`.
2. To have a method in the class with attribute `McpPluginTool`.
3. [optional] Add `Description` attribute to each method argument to let LLM to understand it.
4. [optional] Use `string? optional = null` properties with `?` and default value to mark them as `optional` for LLM.

> Take a look that the line `=> MainThread.Run(() =>` it allows to run the code in Main thread which is needed to interact with Unity API. If you don't need it and running the tool in background thread is fine for the tool, don't use Main thread for efficience purpose.

```csharp
[McpPluginToolType]
public class Tool_GameObject
{
    [McpPluginTool
    (
        "GameObject_Create",
        Title = "Create a new GameObject",
        Description = "Create a new GameObject."
    )]
    public string Create
    (
        [Description("Path to the GameObject (excluding the name of the GameObject).")]
        string path,
        [Description("Name of the GameObject.")]
        string name
    )
    => MainThread.Run(() =>
    {
        var targetParent = string.IsNullOrEmpty(path) ? null : GameObject.Find(path);
        if (targetParent == null && !string.IsNullOrEmpty(path))
            return $"[Error] Parent GameObject '{path}' not found.";

        var go = new GameObject(name);
        go.transform.position = new Vector3(0, 0, 0);
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = new Vector3(1, 1, 1);
        if (targetParent != null)
            go.transform.SetParent(targetParent.transform, false);

        EditorUtility.SetDirty(go);
        EditorApplication.RepaintHierarchyWindow();

        return $"[Success] Created GameObject '{name}' at path '{path}'.";
    });
}
```

# Add custom in-game `tool`

> ⚠️ Not yet supported. There is a blocker issue in `csharp-sdk` for MCP server. Waiting for solution.
> Please vote up for [this issue](https://github.com/modelcontextprotocol/csharp-sdk/discussions/301) and [this](https://github.com/modelcontextprotocol/csharp-sdk/issues/335) to bring more attention to it. The custom tool is dependent on it.


# Contribution

Feel free to add new `tool` into the project.

1. Fork the project.
2. Implement new `tool` in your forked repository.
3. Create Pull Request into original [Unity-MCP](https://github.com/IvanMurzak/Unity-MCP) repository.
