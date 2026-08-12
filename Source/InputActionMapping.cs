using System.Collections.Immutable;
using System.Linq;
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
    /// <summary>
    /// store NamedInputResource, not used directly, you need to 'build()' it
    /// </summary>
    private Array<NamedInputResource> _mapping;
    
    /// <summary>
    /// Set when Build the InputActionMapping
    /// </summary>
    private Array<NamedInputAction> _builtMappingActions;
    
    /// <summary>
    /// Name of this mapping (must be set)
    /// </summary>
    private StringName _mappingName;
    
    /// <summary> Collection of NamedInputActions in this mapping. </summary>
    [Export]
    public Array<NamedInputResource> Mapping
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
    
    public InputActionMapping(StringName name,Array<NamedInputResource> mapping):base()
    {
        _mapping = mapping;
        _mappingName = name;
    }
    

    /// <summary> Adds a new NamedInputAction to this mapping. </summary>
    public void AddNamedInputAction(NamedInputResource a)
    {
        Mapping.Add(a.Duplicate() as  NamedInputAction);
    }

    /// <summary> Removes a NamedInputAction from this mapping. </summary>
    public void RemoveNamedInputAction(NamedInputAction a)
    {
        Mapping.Remove(a.Duplicate() as NamedInputAction);
    }

    /// <summary> Clears all NamedInputActions from this mapping.
    /// Can free a little memory adter build the mapping</summary>
    public void RemoveAllNamedInputResource()
    {
        Mapping.Clear();
    }

    /// <summary>
    /// return all named input action or filter it with an event.
    /// </summary>
    /// <param name="inputEvent"></param>
    /// <returns></returns>
    public Array<NamedInputAction> GetBuiltMappingActions(InputEvent inputEvent)
    {
        return (inputEvent == null)
            ? _builtMappingActions
            : new Array<NamedInputAction>(_builtMappingActions
                .Where(nia => nia.IsSameEvent(inputEvent))
                .ToArray());
    }

    public void Build(bool freeResources = true)
    {
        _builtMappingActions = [];
        for (int i = 0; i < Mapping.Count; i++)
        {
            NamedInputResource nir = Mapping[i];
            if (nir == null)
            {
                GD.PushWarning($"Mapping[{i}] is null");
            }

            var actions = nir.GetNamedActions();
            if (actions.Count==0)
            {
                GD.PushWarning($"mapping[{i}] is empty (no named input associated)");
            }
            _builtMappingActions.AddRange(actions);
        }
        if(freeResources)RemoveAllNamedInputResource();
    }

}