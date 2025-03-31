using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if (IsServer) //Only the server can move the Player in a circle. 
        {
            float theta = Time.time;
            transform.position = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));
        }
    }
}