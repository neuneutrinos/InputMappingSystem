#if TOOLS
using Godot;
using neuneuPlugin.addons.InputMappingSystem.Source.Editor;

namespace NeuneuPlugin.addons.InputMappingSystem;

[Tool]
public partial class InputMappingSystemPlugin : EditorPlugin
{
	private InputActionMappingContextInspector _iamcInspector;
	public override void _EnterTree()
	{
		GD.Print("Plugin Enter tree InputMappingSystemPlugin");
		_iamcInspector = new InputActionMappingContextInspector();
		AddInspectorPlugin(_iamcInspector);
	}

	public override void _ExitTree()
	{
		GD.Print("Plugin Exit tree InputMappingSystemPlugin");
		if (_iamcInspector != null)
		{
			RemoveInspectorPlugin(_iamcInspector);
			_iamcInspector = null;
		}
	}
}
#endif
