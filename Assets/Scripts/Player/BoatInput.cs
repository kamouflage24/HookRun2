using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class BoatInput : MonoBehaviour
{
   public Vector2 Move
   {
    get{
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return Vector2.zero;
        Vector2 input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) input.x -= 1;
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) input.x += 1;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) input.y -= 1;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) input.y += 1;
        return input.normalized;
#else
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
#endif
        } 
    }
    public Vector2 Look{
        get
        {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current == null) return Vector2.zero;
        return Mouse.current.delta.ReadValue() * 0.05f;
#else
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
#endif
        }
    }


    public bool Sprint
     {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
#else
            return Input.GetKey(KeyCode.leftshiftKey);
#endif
        }
    }
    public bool Cast
    {
        get
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.fKey.isPressed;
#else
            return Input.GetKey(KeyCode.fKey);
#endif            
        }
    }

}
