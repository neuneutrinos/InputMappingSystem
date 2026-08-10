#if TOOLS
using Godot;

namespace neuneuPlugin.addons.InputMappingSystem.Source.Editor;

/// XXX button is not added in the Inspector
[Tool]
public partial class InputActionMappingContextInspector : EditorInspectorPlugin
{
    public override bool _CanHandle(GodotObject @object)
    {
        return @object is InputActionMapping;
    }
    public override void _ParseBegin(GodotObject @object)
    {
        GD.Print("_ParseBegin "+@object);
        var button = new Button
        {
            Text = "ActionBaseTest"
        };

        button.Pressed += () =>
        {
            var iamc = @object as InputActionMapping;
        };

        AddCustomControl(button);
    }
}

#endif