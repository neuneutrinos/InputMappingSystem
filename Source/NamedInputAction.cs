using Godot;
using Godot.Collections;

namespace neuneuPlugin.addons.InputMappingSystem.Source;

/// <summary>
/// Represents a named input action with an associated event and matching logic.
/// Used to define custom input mappings that can be compared against input events.
/// </summary>
[GlobalClass]
public partial class NamedInputAction : NamedInputResource
{
    
    private InputEvent _evt;
    private StringName _actionName;
    private bool _ignoreMeta;
    private bool _consumeInput = false;
    
    /// <summary> The input event this action represents. </summary>
    [Export]
    public InputEvent Event
    {
        get => _evt;
        private set => _evt = value;
    }
    
    /// <summary> The name of this action. </summary>
    [Export]
    public StringName ActionName
    {
        get => _actionName;
        private set => _actionName = value;
    }
    
    /// <summary> If true, ignores modifier keys (Ctrl, Shift, Alt, Meta) when matching events. </summary>
    [Export]
    public bool IgnoreMeta
    {
        get => _ignoreMeta;
        set => _ignoreMeta = value;
    }
    /// <summary> If true, stops further input processing when this action is matched. </summary>
    [Export]
    public bool ConsumeInput
    {
        get => _consumeInput;
        set => _consumeInput = value;
    }
    
    public NamedInputAction():base()
    {
        _evt = null;
        _actionName="";
    }

    public NamedInputAction(StringName actionName, InputEvent evt,bool deep=false):base()
    {
        _evt = evt.Duplicate(deep) as InputEvent;
        _actionName = actionName;
    }

    public NamedInputAction(NamedInputAction other,bool deep=false):base()
    {
        other = other.Duplicate(deep) as NamedInputAction;
        _evt = other!._evt;
        _actionName = other!._actionName;
    }

    public override Array<NamedInputAction> GetNamedActions()
    {
        return [this];
    }

    /// <summary> Checks if this action's event matches the given input event. </summary>
    public bool IsSameEvent(InputEvent evt)
    {
        return IsSameEvent(_evt,in evt,IgnoreMeta);
    }
    /// <summary> Checks if this action's event matches another NamedInputAction's event. </summary>
    public bool IsSameEvent(NamedInputAction evt)
    {
        return IsSameEvent(_evt,in evt._evt,IgnoreMeta);
    }

    /// <summary> Compares two input events for equality, optionally ignoring modifier keys. </summary>
    public static bool IsSameEvent(in InputEvent evt1,in InputEvent evt2,bool ignoreMeta=false)
    {
        bool b = true;//evt1.IsMatch(evt2);
        if (evt1.GetType() != evt2.GetType()) return false;
        if(b)
        {
            return (evt1, evt2) switch
            {
                (InputEventKey ea, InputEventKey eb) => IsSameInputEventKey(ea, eb,ignoreMeta),
                (InputEventMouseButton ea, InputEventMouseButton eb) => IsSameInputEventMouseButton(ea, eb,ignoreMeta),
                (InputEventJoypadButton ea, InputEventJoypadButton eb) => IsSameInputEventJoypadButton(ea, eb),
                (InputEventMouseMotion ea, InputEventMouseMotion eb) => IsSameInputEventMouseMotion(ea, eb,ignoreMeta),
                _ => true
            };
        }
        return false;
    }

    /// <summary> Compares two keyboard input events. </summary>
    private static bool IsSameInputEventKey(InputEventKey A, InputEventKey B,bool ignoreMeta)
    {
        bool b = (A.PhysicalKeycode == 0 && (A.Keycode == B.Keycode) ||
                  (A.Keycode == 0 && A.PhysicalKeycode == B.PhysicalKeycode)) &&
                 //A.Unicode == B.Unicode &&
                 A.IsPressed() == B.IsPressed();
        if (!ignoreMeta)
        {
               b &= A.MetaPressed == B.MetaPressed &&
               A.AltPressed == B.AltPressed &&
               A.ShiftPressed == B.ShiftPressed && 
               A.CtrlPressed == B.CtrlPressed;
        }
        return b;
    }
    
    /// <summary> Compares two mouse button input events. </summary>
    private static bool IsSameInputEventMouseButton(InputEventMouseButton A, InputEventMouseButton B,bool ignoreMeta)
    {
        bool b = A.ButtonIndex == B.ButtonIndex &&
                 A.Pressed == B.Pressed;
            //A.IsDoubleClick() == B.IsDoubleClick() &&
            if (!ignoreMeta)
            {
                b&=A.MetaPressed == B.MetaPressed &&
                A.AltPressed == B.AltPressed &&
                A.ShiftPressed == B.ShiftPressed &&
                A.CtrlPressed == B.CtrlPressed;
            }

            return b;
    }

    /// <summary> Compares two joypad button input events. </summary>
    private static bool IsSameInputEventJoypadButton(InputEventJoypadButton A, InputEventJoypadButton B)
    {
        return A.ButtonIndex == B.ButtonIndex &&
               A.Pressed == B.Pressed;
    }
    
    /// <summary> Compares two mouse motion input events. </summary>
    private static bool IsSameInputEventMouseMotion(InputEventMouseMotion A, InputEventMouseMotion B,bool ignoreMeta)
    {
        return ignoreMeta || A.ButtonMask == B.ButtonMask;
    }
    
}