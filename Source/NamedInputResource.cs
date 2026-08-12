using System.Linq;
using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;

public abstract partial class NamedInputResource : Resource
{
    /// <summary>
    /// Return all NamedInputAction associate with this resource
    /// </summary>
    /// <param name="Event"></param>
    /// <returns>Array of NamedInputAction or empty array if none</returns>
    public virtual Array<NamedInputAction> GetNamedActions()
    {
        return new Array<NamedInputAction>();
    }

    /// <summary>
    /// return all associated namedInput that match with the inputEvent 
    /// </summary>
    /// <param name="inputEvent"></param>
    /// <typeparam name="NamedInputAction"></typeparam>
    /// <returns></returns>
    public virtual NamedInputAction[] GetNamedActionsWithEvent(InputEvent inputEvent)
    {
        return GetNamedActions().Where(ie => ie.IsSameEvent(inputEvent)).ToArray();
    }

}