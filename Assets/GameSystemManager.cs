using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSystemManager : MonoBehaviour
{

    GameObject TopLeft;
    GameObject TopMiddle;
    GameObject TopRight;
    GameObject MiddleLeft;
    GameObject Middle;
    GameObject MiddleRight;
    GameObject BottomLeft;
    GameObject BottomMiddle;
    GameObject BottomRight;

    bool gameRunning = false;
    GameObject networkedClient;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeGame()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in sceneObjects)
        {
            if (go.name == "TopLeft")
            {
                TopLeft = go;
            }
            else if (go.name == "TopMiddle")
            {
                TopMiddle = go;
            }
            else if (go.name == "TopRight")
            {
                TopRight = go;
            }
            else if (go.name == "MiddleLeft")
            {
                MiddleLeft = go;
            }
            else if (go.name == "Middle")
            {
                Middle = go;
            }
            else if (go.name == "MiddleRight")
            {
                MiddleRight = go;
            }
            else if (go.name == "BottomLeft")
            {
                BottomLeft = go;
            }
            else if (go.name == "BottomMiddle")
            {
                BottomMiddle = go;
            }
            else if (go.name == "BottomRight")
            {
                BottomRight = go;
            }
            else if (go.name == "NetworkedClient")
            {
                networkedClient = go;
            }
        }

        gameRunning = true;

        TopLeft.GetComponent<Button>().onClick.AddListener(TopLeftClicked);
        TopMiddle.GetComponent<Button>().onClick.AddListener(TopMiddleClicked);
        TopRight.GetComponent<Button>().onClick.AddListener(TopRightClicked);
        MiddleLeft.GetComponent<Button>().onClick.AddListener(MiddleLeftClicked);
        Middle.GetComponent<Button>().onClick.AddListener(MiddleClicked);
        MiddleRight.GetComponent<Button>().onClick.AddListener(MiddleRightClicked);
        BottomLeft.GetComponent<Button>().onClick.AddListener(BottomLeftClicked);
        BottomMiddle.GetComponent<Button>().onClick.AddListener(BottomMiddleClicked);
        BottomRight.GetComponent<Button>().onClick.AddListener(BottomRightClicked);
    }

    public void RefreshUI(int tl, int tm, int tr, int ml, int m, int mr, int bl, int bm, int br)
    {
        TopLeft.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(tl);
        TopMiddle.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(tm);
        TopRight.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(tr);
        MiddleLeft.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(ml);
        Middle.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(m);
        MiddleRight.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(mr);
        BottomLeft.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(bl);
        BottomMiddle.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(bm);
        BottomRight.GetComponent<Button>().GetComponentInChildren<Text>().text = ConvertButtonString(br);
    }

    private string ConvertButtonString(int val)
    {
        if(val == 0)
        {
            return "";
        }
        else if(val == 1)
        {
            return "X";
        }
        else if(val == 2)
        {
            return "O";
        }

        return "";
    }

    //***************************
    //TIC-TAC-TOE BOARD OnClick()
    //***************************
    private void TopLeftClicked()
    {
        if(TopLeft.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 1);
        }
    }

    private void TopMiddleClicked()
    {
        if (TopMiddle.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 2);
        }
    }
    private void TopRightClicked()
    {
        if (TopRight.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 3);
        }
    }
    private void MiddleLeftClicked()
    {
        if (MiddleLeft.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 4);
        }
    }
    private void MiddleClicked()
    {
        if (Middle.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 5);
        }
    }
    private void MiddleRightClicked()
    {
        if (MiddleRight.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 6);
        }
    }
    private void BottomLeftClicked()
    {
        if (BottomLeft.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 7);
        }
    }
    private void BottomMiddleClicked()
    {
        if (BottomMiddle.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 8);
        }
    }
    private void BottomRightClicked()
    {
        if (BottomRight.GetComponent<Button>().GetComponentFromChild<Text>().text == 0)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerStateSignifiers.Game + "," + ClientToServerGameSignifiers.ChoiceMade + "," + 9);
        }
    }
}
