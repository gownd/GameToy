using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isOn = false;
    public int index = 0;
    public delegate void HandleSwitchChange();
    public HandleSwitchChange HandleSwitchOn;
    public HandleSwitchChange HandleSwitchOff;
}