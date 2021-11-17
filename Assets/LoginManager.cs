using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoginManager : MonoBehaviour
{
    //UI Elements
    GameObject loginToggle;
    GameObject usernameField;
    GameObject passwordField;
    GameObject gameRoomField;
    GameObject submitButton;

    //Member Variables
    GameObject networkedClient;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject go in sceneObjects)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
