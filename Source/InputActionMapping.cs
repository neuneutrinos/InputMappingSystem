using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;

/// <summary>
/// A Resource that groups multiple NamedInputActions under a single mapping name.
/// Used to organize related input actions into logical contexts.
/// </summary>
[GlobalClass]
public partial class InputActionMapping : Resource
{
    private Array<NamedInputAction> _mapping;
    private StringName _mappingName;
    
    /// <summary> Collection of NamedInputActions in this mapping. </summary>
    [Export]
    public Array<NamedInputAction> Mapping
    {
        get => _mapping;
        private set => _mapping = value;
    }
    /// <summary> Unique identifier for this action mapping. </summary>
    [Export]
    public StringName MappingName
    {
        get => _mappingName;
        private set => _mappingName = value;
    }

    public InputActionMapping():base()
    {
        _mapping = new();
        _mappingName = new();
    }
    
    public InputActionMapping(StringName name):this()
    {
        _mappingName = name;
    }
    
    public InputActionMapping(StringName name,Array<NamedInputAction> mapping):base()
    {
        _mapping = mapping;
        _mappingName = name;
    }
    

    /// <summary> Adds a new NamedInputAction to this mapping. </summary>
    public void AddNamedInputAction(NamedInputAction a)
    {
        Mapping.Add(a.Duplicate() as  NamedInputAction);
    }

    /// <summary> Removes a NamedInputAction from this mapping. </summary>
    public void RemoveNamedInputAction(NamedInputAction a)
    {
        Mapping.Remove(a.Duplicate() as NamedInputAction);
    }

    /// <summary> Clears all NamedInputActions from this mapping. </summary>
    public void RemoveAllNamedInputActions()
    {
        Mapping.Clear();
    }

    /// <summary>
    /// Finds all NamedInputActions in this mapping that match the given input event.
    /// Stops searching if a matching action has ConsumeInput set to true.
    /// </summary>
    public Array<NamedInputAction> GetNamedInputActionsByEvent(InputEvent @event)
    {
        Array<NamedInputAction> ret = new();
        foreach (var namedInput in _mapping)
        {
            if(namedInput.IsSameEvent(@event))
            {
                ret.Add(namedInput);
                if (namedInput.ConsumeInput) break;
            }
        }
        return ret;
    }
}