using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float Speed = 2.5f;
    public float OutsideScreen;
    private bool ShouldRemove;

    // Start is called before the first frame update
    void Start()
    {
        ShouldRemove = false;
    }

    // Update is called once per frame
    void Update()
    {
        // move wall to left
        transform.position += Vector3.left * Time.deltaTime * Speed;

        if (Game.GameOver)
        {
            ShouldRemove = true;
            return;
        }

        // remove wall if outside screen or game over
        if (transform.position.x < OutsideScreen || ShouldRemove)
        {
            Destroy(this.gameObject);
        }
    }
}
