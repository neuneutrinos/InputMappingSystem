using Godot;

namespace neuneuPlugin.addons.InputMappingSystem.Source;


[GlobalClass]
public partial class NamedInputAction : Resource
{
    private InputEvent _evt;
    private StringName _actionName;
    //valid even if meta is invalid
    private bool _ignoreMeta;
    //if this input is selected, stop input serach.
    private bool _consumeInput = false;
    
    [Export]
    public InputEvent Event
    {
        get => _evt;
        private set => _evt = value;
    }

    [Export]
    public StringName ActionName
    {
        get => _actionName;
        private set => _actionName = value;
    }

    [Export]
    public bool IgnoreMeta
    {
        get => _ignoreMeta;
        set => _ignoreMeta = value;
    }

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

    
    public bool IsSameEvent(InputEvent evt)
    {
        return IsSameEvent(_evt,in evt,IgnoreMeta);
    }
    public bool IsSameEvent(NamedInputAction evt)
    {
        return IsSameEvent(_evt,in evt._evt,IgnoreMeta);
    }

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
                (InputEventMouseMotion ea, InputEventMouseMotion eb) => IsSameInputEventMouseMotion(ea, eb),
                _ => true
            };
        }
        return false;
    }

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

    private static bool IsSameInputEventJoypadButton(InputEventJoypadButton A, InputEventJoypadButton B)
    {
        return A.ButtonIndex == B.ButtonIndex &&
               A.Pressed == B.Pressed;
    }
    
    private static bool IsSameInputEventMouseMotion(InputEventMouseMotion A, InputEventMouseMotion B)
    {
        return A.ButtonMask == B.ButtonMask;
    }
    
}