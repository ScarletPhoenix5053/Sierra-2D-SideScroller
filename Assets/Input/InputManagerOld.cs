using UnityEngine;

public class InputManagerOld : MonoBehaviour {

    public static InputManagerOld Instance; 
    
    // Properties for all usable inputs.
    public bool Attack1
    {
        get
        {
            return
                Input.GetKeyDown(KeyCode.JoystickButton2) ||
                Input.GetKeyDown(KeyCode.Mouse0) ||
                Input.GetKeyDown(KeyCode.U);
        }
    }
    public bool Attack2
    {
        get
        {
            return
                Input.GetKeyDown(KeyCode.JoystickButton3) ||
                Input.GetKeyDown(KeyCode.Mouse1) ||
                Input.GetKeyDown(KeyCode.I);
        }
    }

    private void Awake()
    {
        if (Instance != this && Instance != null) Destroy(this);
        else Instance = this;
    }


}