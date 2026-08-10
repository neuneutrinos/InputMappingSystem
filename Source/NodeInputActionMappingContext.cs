using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;

/// <summary>
/// A Node that manages multiple InputActionMappings and handles input signal.
/// Check all actives mappings and emits signal when matched actions are triggered.
/// </summary>
[GlobalClass]
public partial class NodeInputActionMappingContext : Node
{
    private Dictionary<StringName, InputActionMapping> _mapping;
    private Array<StringName> _activeMapping;

    
    /// <summary> All registered InputActionMappings by their names. </summary>
    [Export]
    public Dictionary<StringName, InputActionMapping> Mapping
    {
        get => _mapping;
        private set => _mapping = value ?? new();
    }

    /// <summary> Names of currently active mappings. </summary>
    public Array<StringName> ActiveMapping
    {
        get => _activeMapping;
        private set => _activeMapping = value ?? new();
    }

    /// <summary> Rebuilds the mapping dictionary to ensure proper key assignment. </summary>
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

    /// <summary> Adds a new InputActionMapping to the context. </summary>
    public void AddMappingContext(InputActionMapping mappingContext)
    {
        Mapping.Add(mappingContext.MappingName, mappingContext);
    }
    
    /// <summary> Removes a mapping from both the registry and active list. </summary>
    public void RemoveMappingContext(StringName mappingName)
    {
        Mapping.Remove(mappingName);
        ActiveMapping.Remove(mappingName);
    }

    /// <summary>
    /// Activates a mapping if it exists and isn't already active.
    /// </summary>
    /// <returns>True if activation succeeded, false otherwise.</returns>
    public bool ActivateMapping(StringName mappingName)
    {
        if(!ActiveMapping.Contains(mappingName) && Mapping.ContainsKey(mappingName))
        {
            ActiveMapping.Add(mappingName);
            return true;
        }
        return false;
    }
    /// <summary> Deactivates a mapping by removing it from the active list. </summary>
    /// <returns>True if removing succeeded, false otherwise.</returns>
    public bool DeactivateMapping(StringName mappingName)
    {
           return ActiveMapping.Remove(mappingName);
    }


    public override void _EnterTree()
    {
        base._EnterTree();
        Rebuild();
    }
    
    /// <summary>
    /// Handles input events. For each active mapping, checks if the event triggers any named input actions
    /// and emits OnActionTriggered signal accordingly.
    /// </summary>
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
    /// <summary>
    /// Emitted when an action in an active mapping is triggered.
    /// </summary>
    /// <param name="iam">The InputActionMapping that was triggered</param>
    /// <param name="namedInput">The specific NamedInputAction that was triggered</param>
    /// <param name="triggerTime">The timestamp when the action was triggered (in seconds)</param>
    [Signal]
    public delegate void OnActionTriggeredEventHandler(InputActionMapping iam,NamedInputAction namedInput,float triggerTime);
}