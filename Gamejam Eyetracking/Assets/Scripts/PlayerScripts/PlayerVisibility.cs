using UnityEngine;

public class PlayerVisibility : MonoBehaviour
{
    public static PlayerVisibility Instance { get; private set; }
    public float CurrentVisibility { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public void SetVisibility(float v)
    {
        CurrentVisibility = v;
    }
}