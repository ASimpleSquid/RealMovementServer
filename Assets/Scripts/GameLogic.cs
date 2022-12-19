using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    void Start()
    {
        NetworkedServerProcessing.SetGameLogic(this);
    }

    void Update()
    {

    }

}

public struct Player
{
    Vector2 Position, Velocity;
    int id;

    public Player(Vector2 Position, Vector2 Velocity, int id)
    {
        this.Position = Position;
        this.Velocity = Velocity;
        this.id = id;
    }
}
