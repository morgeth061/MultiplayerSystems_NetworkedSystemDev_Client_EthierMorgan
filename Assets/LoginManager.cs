using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    int currentGameRoom = -1;
    bool isLogin = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject go in sceneObjects)
        {
            if (go.name == "UsernameField")
            {
                usernameField = go;
            }
            else if (go.name == "PasswordField")
            {
                passwordField = go;
            }
            else if(go.name == "GameRoomField")
            {
                gameRoomField = go;
            }
            else if(go.name == "Login/CreateAccount")
            {
                loginToggle = go;
            }
            else if(go.name == "Submit")
            {
                submitButton = go;
            }
            else if(go.name == "NetworkedClient")
            {
                networkedClient = go;
            }
        }

        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        loginToggle.GetComponent<Button>().onClick.AddListener(ToggleButtonPressed);

        UpdateGameRoom();

    }

    // Update is called once per frame
    void Update()
    {
        //Can't update GameRoom value via OnValueChanged, so changing it here.
        UpdateGameRoom();
    }

    public void SubmitButtonPressed()
    {
        Debug.Log("Submit button pressed");

        string user = usernameField.GetComponent<InputField>().text;
        string pass = passwordField.GetComponent<InputField>().text;

        string msg;

        //Login is current toggle
        if (isLogin)
        {
            msg = ClientToServerStateSignifiers.Account + "," + ClientToServerAccountSignifiers.Login + "," + user + "," + pass + "," + currentGameRoom;
        }
        //Login is not current toggle
        else
        {
            msg = ClientToServerStateSignifiers.Account + "," + ClientToServerAccountSignifiers.CreateAccount + "," + user + "," + pass + "," + currentGameRoom;
        }

        //Send message to server
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);

    }

    //Switch between Login and Account Creation
    public void ToggleButtonPressed()
    {
        if(isLogin)
        {
            loginToggle.GetComponent<Button>().GetComponentInChildren<Text>().text = "Create Account";
            gameRoomField.SetActive(false);
        }
        else if(!isLogin)
        {
            loginToggle.GetComponent<Button>().GetComponentInChildren<Text>().text = "Login";
            gameRoomField.SetActive(true);
        }

        UpdateGameRoom();
        isLogin = !isLogin;
    }

    //Changes to GameRoom currently selected.
    //If on Account Creation, GameRoom is -1, meaning account is being created instead.
    public void UpdateGameRoom()
    {
        if(isLogin)
        {
            if(gameRoomField.GetComponent<Dropdown>().captionText.text == "Game Room 1")
            {
                currentGameRoom = 1;
            }
            else if(gameRoomField.GetComponent<Dropdown>().captionText.text == "Game Room 2")
            {
                currentGameRoom = 2;
            }
            else if (gameRoomField.GetComponent<Dropdown>().captionText.text == "Game Room 3")
            {
                currentGameRoom = 3;
            }
        }
        else 
        {
            currentGameRoom = -1;
        }
    }
}


