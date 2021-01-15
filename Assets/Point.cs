using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    // Add point to score when player collides with this game object
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Game.Score += 1;
            Destroy(this.gameObject);
        }
    }
}
