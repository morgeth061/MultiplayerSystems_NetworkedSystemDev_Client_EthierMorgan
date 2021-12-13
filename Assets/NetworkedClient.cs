using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkedClient : MonoBehaviour
{
    int connectionID;
    int maxConnections = 1000;
    int reliableChannelID;
    int unreliableChannelID;
    int hostID;
    int socketPort = 5491;
    byte error;
    bool isConnected = false;
    int ourClientID;

    //UI
    GameObject LoginCanvas;
    GameObject GameCanvas;

    //Game
    bool gameInitialized = false;
    bool isSpectator = false;
    GameObject gameSystemManager;
    string username;

    void Start()
    {
        GameObject[] sceneObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in sceneObjects)
        {
            if (go.name == "LoginCanvas")
            {
                LoginCanvas = go;
            }
            else if (go.name == "GameCanvas")
            {
                GameCanvas = go;
            }
            else if (go.name == "GameSystemManager")
            {
                gameSystemManager = go;
            }
        }

        GameCanvas.SetActive(false);

        Connect();
    }

    void Update()
    {
        UpdateNetworkConnection();
    }

    private void UpdateNetworkConnection()
    {
        if (isConnected)
        {
            int recHostID;
            int recConnectionID;
            int recChannelID;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            NetworkEventType recNetworkEvent = NetworkTransport.Receive(out recHostID, out recConnectionID, out recChannelID, recBuffer, bufferSize, out dataSize, out error);

            switch (recNetworkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    Debug.Log("connected.  " + recConnectionID);
                    ourClientID = recConnectionID;
                    break;
                case NetworkEventType.DataEvent:
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    ProcessRecievedMsg(msg, recConnectionID);
                    //Debug.Log("got msg = " + msg);
                    break;
                case NetworkEventType.DisconnectEvent:
                    isConnected = false;
                    Debug.Log("disconnected.  " + recConnectionID);
                    break;
            }
        }
    }

    private void Connect()
    {

        if (!isConnected)
        {
            Debug.Log("Attempting to create connection");

            NetworkTransport.Init();

            ConnectionConfig config = new ConnectionConfig();
            reliableChannelID = config.AddChannel(QosType.Reliable);
            unreliableChannelID = config.AddChannel(QosType.Unreliable);
            HostTopology topology = new HostTopology(config, maxConnections);
            hostID = NetworkTransport.AddHost(topology, 0);
            Debug.Log("Socket open.  Host ID = " + hostID);

            connectionID = NetworkTransport.Connect(hostID, "192.168.2.20", socketPort, 0, out error); // server is local on network

            if (error == 0)
            {
                isConnected = true;

                Debug.Log("Connected, id = " + connectionID);

            }
        }
    }

    public void Disconnect()
    {
        NetworkTransport.Disconnect(hostID, connectionID, out error);
    }

    public void SendMessageToHost(string msg)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(msg);
        NetworkTransport.Send(hostID, connectionID, reliableChannelID, buffer, msg.Length * sizeof(char), out error);
    }

    private void ProcessRecievedMsg(string msg, int id)
    {
        Debug.Log("msg recieved = " + msg + ".  connection id = " + id);

        string[] csv = msg.Split(',');

        int stateSignifier = int.Parse(csv[0]);

        if(stateSignifier == ServerToClientStateSignifiers.Account)
        {
            int accountSignifier = int.Parse(csv[1]);
            //Switch from Login to Game view
            if (accountSignifier == ServerToClientAccountSignifiers.LoginComplete)
            {
                GameCanvas.SetActive(true);
                LoginCanvas.SetActive(false);
                gameSystemManager.GetComponent<GameSystemManager>().PlayerJoined();
            }
        }
        //Game-Related Functions
        else if(stateSignifier == ServerToClientStateSignifiers.Game)
        {
            int gameSignifier = int.Parse(csv[1]);

            //Initialize game
            if (gameSignifier == ServerToClientGameSignifiers.GameInitialize)
            {
                Debug.Log("Game initializing.");
                gameSystemManager.GetComponent<GameSystemManager>().InitializeGame(csv[2]);
                gameInitialized = true;
            }
            //Currently your turn
            else if (gameSignifier == ServerToClientGameSignifiers.CurrentTurn)
            {
                Debug.Log("Your turn.");
                gameSystemManager.GetComponent<GameSystemManager>().UpdateTextBox("It is now your turn.");
                gameSystemManager.GetComponent<GameSystemManager>().RefreshUI(int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]), int.Parse(csv[7]), int.Parse(csv[8]), int.Parse(csv[9]), int.Parse(csv[10]));
            }
            //Refresh game board with new Xs or Os placed
            else if (gameSignifier == ServerToClientGameSignifiers.RefreshUI)
            {
                Debug.Log("Refreshing UI.");
                gameSystemManager.GetComponent<GameSystemManager>().RefreshUI(int.Parse(csv[2]), int.Parse(csv[3]), int.Parse(csv[4]), int.Parse(csv[5]), int.Parse(csv[6]), int.Parse(csv[7]), int.Parse(csv[8]), int.Parse(csv[9]), int.Parse(csv[10]));
            }
            //You won
            else if (gameSignifier == ServerToClientGameSignifiers.YouWon)
            {
                Debug.Log("You won!");
                gameSystemManager.GetComponent<GameSystemManager>().UpdateTextBox("You won!");
                gameSystemManager.GetComponent<GameSystemManager>().GameWon(true);
            }
            //You lost
            else if (gameSignifier == ServerToClientGameSignifiers.OpponentWon)
            {
                Debug.Log("Opponent won!");
                gameSystemManager.GetComponent<GameSystemManager>().UpdateTextBox("You lost.");
                gameSystemManager.GetComponent<GameSystemManager>().GameWon(false);
            }
            //Process message received from another player
            else if (gameSignifier == ServerToClientGameSignifiers.Message)
            {
                gameSystemManager.GetComponent<GameSystemManager>().UpdateTextBox(csv[2]);
            }
            //Game ends in tie
            else if (gameSignifier == ServerToClientGameSignifiers.Stalemate)
            {
                Debug.Log("Tie!");
                gameSystemManager.GetComponent<GameSystemManager>().UpdateTextBox("Tie!");
                gameSystemManager.GetComponent<GameSystemManager>().GameWon(true); //Allows both players to click replay/restart button
            }
        }
        //You are a spectator
        else if(stateSignifier == ServerToClientStateSignifiers.Spectate)
        {
            gameSystemManager.GetComponent<GameSystemManager>().SetSpectator();
        }
    }

    public void SetUsername(string name)
    {
        username = name;
    }

    public string GetUsername()
    {
        return username;
    }

    public bool IsConnected()
    {
        return isConnected;
    }
}

public static class ClientToServerStateSignifiers
{
    public const int Account = 1;

    public const int Game = 2;

    public const int Spectate = 3;

    public const int Other = 9;
}

public static class ClientToServerAccountSignifiers
{
    public const int CreateAccount = 1;

    public const int Login = 2;
}
public static class ClientToServerGameSignifiers
{
    public const int ChoiceMade = 1;

    public const int ResetGame = 2;

    public const int Message = 3;

    public const int Replay = 4;
}

public static class ServerToClientStateSignifiers
{
    public const int Account = 1;

    public const int Game = 2;

    public const int Spectate = 3;
}

public static class ServerToClientAccountSignifiers
{
    public const int LoginComplete = 1;

    public const int LoginFailed = 2;

    public const int AccountCreationComplete = 3;

    public const int AccountCreationFailed = 4;
}

public static class ServerToClientGameSignifiers
{
    public const int CurrentTurn = 1;

    public const int YouWon = 2;

    public const int OpponentWon = 3;

    public const int RefreshUI = 4;

    public const int Message = 5;

    public const int Stalemate = 6;

    public const int GameInitialize = 9;

}
