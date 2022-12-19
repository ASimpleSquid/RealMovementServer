using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    public List<Player> players;

    void Start()
    {
        players = new List<Player>();
        NetworkedServerProcessing.SetGameLogic(this);
    }

    void Update()
    {
        players.ForEach(player => player.Position += (player.Velocity * Time.deltaTime));
        //characterPositionInPercent += (characterVelocityInPercent * Time.deltaTime);
    }

    public void UpdateAll(Player player)
    {
        players.ForEach(reciever => player.SendUpdate(reciever.id));
    }
}

public struct Player
{
    public Vector2 Position, Velocity;
    public int id;

    public Player(int id)
    {
        this.Position = new Vector2(0.5f,0.5f);
        this.Velocity = Vector2.zero;
        this.id = id;
    }

    public override string ToString() => $"{Position.x:F5}_{Position.y:F5}_{Velocity.x:F5}_{Velocity.y:F5}_{id}";
    
    public void SendUpdate(int id)
    {
        NetworkedServerProcessing.SendMessageToClient($"{ServerToClientSignifiers.updatePlayer:D},{this}",id);
    }

}
