using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPress : MonoBehaviour
{
    public KeyCode key;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("Pressing Button");
            button.onClick.Invoke();
        }

    }
}