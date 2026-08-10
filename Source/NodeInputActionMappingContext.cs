using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source.Editor;

[GlobalClass]
public partial class NodeInputActionMappingContext : Node
{
    private Dictionary<StringName, InputActionMapping> _mapping;
    private Array<StringName> _activeMapping;

    

    [Export]
    public Dictionary<StringName, InputActionMapping> Mapping
    {
        get => _mapping;
        private set => _mapping = value ?? new();
    }

   // [Export]
    public Array<StringName> ActiveMapping
    {
        get => _activeMapping;
        private set => _activeMapping = value ?? new();
    }

    void Rebuild()
    {
        var mv = Mapping.Values;
        Mapping.Clear();
        foreach (var elem in mv)
        {
            Mapping.Add(elem.MappingName, elem);
        }
        
    }

    public NodeInputActionMappingContext() : base()
    {
        Mapping = null;
        ActiveMapping = null;
    }

    public void AddMappingContext(InputActionMapping mappingContext)
    {
        Mapping.Add(mappingContext.MappingName, mappingContext);
    }
    
    public void RemoveMappingContext(StringName mappingName)
    {
        Mapping.Remove(mappingName);
        ActiveMapping.Remove(mappingName);
    }

    public bool ActivateMapping(StringName mappingName)
    {
        bool dbgA = !ActiveMapping.Contains(mappingName);
        bool dbgB = Mapping.ContainsKey(mappingName);
        Mapping.ContainsKey(mappingName);
        if(!ActiveMapping.Contains(mappingName) && Mapping.ContainsKey(mappingName))
        {
            ActiveMapping.Add(mappingName);
            return true;
        }
        return false;
    }
    public void DeactivateMapping(StringName mappingName)
    {
            ActiveMapping.Remove(mappingName);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Rebuild();
    }

    

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event.IsEcho()) return;
        foreach (var am in ActiveMapping)
        {
            foreach (var ni in Mapping[am].GetNamedInputActionsByEvent(@event))
            {
                EmitSignalOnActionTriggered(Mapping[am],ni,Time.GetTicksMsec()/1000f);
            }
        }
    }
    
    //signal
    [Signal]
    public delegate void OnActionTriggeredEventHandler(InputActionMapping iam,NamedInputAction namedInput,float triggerTime);
}