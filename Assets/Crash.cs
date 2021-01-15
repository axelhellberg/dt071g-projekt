using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    public Sprite Crashed;

    // End game and change sprite when player collides with wall
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            this.GetComponent<SpriteRenderer>().sprite = Crashed;
            Game.EndGame();
        }
    }
}
