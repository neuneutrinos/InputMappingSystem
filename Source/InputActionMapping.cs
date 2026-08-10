using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;


[GlobalClass]
public partial class InputActionMapping : Resource
{
    private Array<NamedInputAction> _mapping;
    private StringName _mappingName;

    [Export]
    public Array<NamedInputAction> Mapping
    {
        get => _mapping;
        private set => _mapping = value;
    }

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
    

    public void AddNamedInputAction(NamedInputAction a)
    {
        Mapping.Add(a.Duplicate() as  NamedInputAction);
    }

    public void RemoveNamedInputAction(NamedInputAction a)
    {
        Mapping.Remove(a.Duplicate() as NamedInputAction);
    }

    public void RemoveAllNamedInputActions()
    {
        Mapping.Clear();
    }

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