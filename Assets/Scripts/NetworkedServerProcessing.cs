using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedServerProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromClient(string msg, int clientConnectionID)
    {
        Debug.Log("msg received = " + msg + ".  connection id = " + clientConnectionID);

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

        switch ((ClientToServerSignifiers)signifier)
        {
            case ClientToServerSignifiers.movement:
                Player player = gameLogic.players.Find(p => p.id == clientConnectionID);
                switch ((Directions)Int32.Parse(csv[1]))
                {
                    case Directions.Stop:
                        player.Velocity = Vector2.zero;
                        break;
                    case Directions.N:
                        player.Velocity.x = 0;
                        player.Velocity.y = GameLogic.CharacterSpeed;
                        break;
                    case Directions.NE:
                        player.Velocity.x = GameLogic.DiagonalCharacterSpeed;
                        player.Velocity.y = GameLogic.DiagonalCharacterSpeed;
                        break;
                    case Directions.E:
                        player.Velocity.x = GameLogic.CharacterSpeed;
                        player.Velocity.y = 0;
                        break;
                    case Directions.SE:
                        player.Velocity.x = GameLogic.DiagonalCharacterSpeed;
                        player.Velocity.y = -GameLogic.DiagonalCharacterSpeed;
                        break;
                    case Directions.S:
                        player.Velocity.x = 0;
                        player.Velocity.y = -GameLogic.CharacterSpeed;
                        break;
                    case Directions.SW:
                        player.Velocity.x = -GameLogic.DiagonalCharacterSpeed;
                        player.Velocity.y = -GameLogic.DiagonalCharacterSpeed;
                        break;
                    case Directions.W:
                        player.Velocity.x = -GameLogic.CharacterSpeed;
                        player.Velocity.y = 0;
                        break;
                    case Directions.NW:
                        player.Velocity.x = -GameLogic.DiagonalCharacterSpeed;
                        player.Velocity.y = GameLogic.DiagonalCharacterSpeed;
                        break;

                    default:
                        break;
                }
                break;

            default:
                break;
        }
    }
    static public void SendMessageToClient(string msg, int clientConnectionID)
    {
        networkedServer.SendMessageToClient(msg, clientConnectionID);
    }

    static public void SendMessageToClientWithSimulatedLatency(string msg, int clientConnectionID)
    {
        networkedServer.SendMessageToClientWithSimulatedLatency(msg, clientConnectionID);
    }

    
    #endregion

    #region Connection Events

    static public void ConnectionEvent(int clientConnectionID)
    {
        gameLogic.players.ForEach(player => SendMessageToClient($"{ServerToClientSignifiers.connect:D},{clientConnectionID}", player.id));
        gameLogic.players.Add(new Player(clientConnectionID));
        Debug.Log("New Connection, ID == " + clientConnectionID);
    }
    static public void DisconnectionEvent(int clientConnectionID)
    {
        gameLogic.players.RemoveAll(player => player.id == clientConnectionID);
        gameLogic.players.ForEach(player => SendMessageToClient($"{ServerToClientSignifiers.disconnect:D},{clientConnectionID}", player.id));
        Debug.Log("New Disconnection, ID == " + clientConnectionID);
    }

    #endregion

    #region Setup
    static NetworkedServer networkedServer;
    static GameLogic gameLogic;

    static public void SetNetworkedServer(NetworkedServer NetworkedServer)
    {
        networkedServer = NetworkedServer;
    }
    static public NetworkedServer GetNetworkedServer()
    {
        return networkedServer;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion
}

#region Protocol Signifiers
public enum ClientToServerSignifiers
{
    asd,
    movement
}

public enum ServerToClientSignifiers
{
    asd,
    connect,
    disconnect,
    updatePlayer
}

public enum Directions
{ 
    Stop,
    N,
    NE,
    E,
    SE,
    S,
    SW,
    W,
    NW
}


#endregion