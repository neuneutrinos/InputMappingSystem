using System;
using System.Linq;
using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;

/// <summary>
/// A Resource that represents a named action with multiple input events.
/// Can be converted to multiple NamedInputAction instances for compatibility.
/// </summary>
[GlobalClass]
public partial class NamedMultipleInputAction : NamedInputResource
{
    /// <summary> The name of this action. </summary>
    private StringName _actionName;
    /// <summary> Collection of InputEvents associated with this action. </summary>
    private Array<InputEvent> _iea;
    
    [Export]
    public StringName ActionName
    {
        get; set;
    }

    [Export]
    public Array<InputEvent> MappingArray
    {
        get => _iea;
        private set => _iea = value;
    }

    /// <summary>
    /// Converts this multiple input action into an array of NamedInputAction instances.
    /// Each InputEvent in MappingArray becomes a separate NamedInputAction with the same ActionName.
    /// </summary>
    /// <param name="Event"></param>
    /// <returns>Array of NamedInputAction instances</returns>
    public override Array<NamedInputAction> GetNamedActions()
    {
        Array<NamedInputAction> actions = new();
        foreach(var elem in MappingArray)
            actions.Add(new NamedInputAction(ActionName, elem));
        return actions;
    }
    
    
}