using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int Score = 0;
    public static bool GameOver;
    public static bool GameStarted = false;

    public GameObject[] Walls;
    public Transform Spawn;
    public GameObject Player;

    public Sprite Fly;
    public Sprite Flap;
    public Sprite Glide;

    public float JumpPower;
    public ForceMode JumpType;

    public float SpawnInterval = 3;
    private float Timer = 0;

    public float SkyboxSpeed;

    private Vector2 StartPos;

    // Stop time and set bool
    public static void EndGame()
    {
        Time.timeScale = 0;
        GameOver = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartPos = Player.transform.position;
        Time.timeScale = 0;
        PositionPlayer();
        Player.GetComponent<SpriteRenderer>().sprite = Fly;
    }

    // Start time and set bools, score, spawn timer and position
    void StartGame()
    {
        if (GameOver) PositionPlayer();
        Time.timeScale = 1;
        GameStarted = true;
        GameOver = false;
        Score = 0;
        Timer = SpawnInterval;
    }

    // Activate player game object, set position and velocity to 0
    void PositionPlayer()
    {
        Player.transform.position = StartPos;
        Player.GetComponent<Rigidbody>().velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameStarted) return;

        if (GameOver) return;

        Timer += Time.deltaTime; // add time of last frame completion to timer

        if (Timer > SpawnInterval) // spawn wall and reset timer at spawn interval
        {
            SpawnWall();
            Timer -= SpawnInterval;
        }

        HandleInput();
        FlapWings();

        RenderSettings.skybox.SetFloat("_Rotation", Time.time * SkyboxSpeed); // move skybox
    }

    // Randomize and spawn wall
    void SpawnWall()
    {
        int WallIndex = Random.Range(0, Walls.Length);
        GameObject.Instantiate(Walls[WallIndex], Spawn.position, Quaternion.identity);
    }
    
    // Scale and draw GUI depending on bools
    void OnGUI()
    {
        float ScaleValue = Mathf.Max(Screen.dpi * 0.02f, 1);
        GUI.matrix = Matrix4x4.Scale(Vector3.one * ScaleValue);
        GUILayout.BeginArea(new Rect(0, 0, Screen.width / ScaleValue, Screen.height / ScaleValue));

        if (GameStarted && !GameOver)
        {
            GUILayout.Label("Score: " + Score);
        }

        GUILayout.BeginVertical(); // begin section of vertical stack
        GUILayout.FlexibleSpace(); // center area
        {
            if (GameOver)
            {
                GUILayout.BeginHorizontal(); // begin horizontal section
                GUILayout.FlexibleSpace(); // center area
                {
                    GUILayout.Label("Game over!");
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                {
                    GUILayout.Label("Score: " + Score);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.Space(Screen.height * 0.01f); // insert spacing

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                {
                    if (GUILayout.Button("Try again"))
                    {
                        StartGame();
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            if (!GameStarted)
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                {
                    if (GUILayout.Button("Play"))
                    {
                        StartGame();
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        GUILayout.EndArea();
    }

    // Listen for mouse clicks and move player
    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Player.GetComponent<Rigidbody>().velocity.y < 0)
            {
                Player.GetComponent<Rigidbody>().AddForce(Vector3.up * JumpPower, JumpType);
            }
        }
    }

    // Change sprite and hitbox based on vertical velocity
    void FlapWings()
    {
        if (Player.GetComponent<Rigidbody>().velocity.y <= -0.6)
        {
            Player.GetComponent<SpriteRenderer>().sprite = Fly;
            Player.GetComponent<SphereCollider>().center = new Vector3(0, -0.04f);
        }
        else if (Player.GetComponent<Rigidbody>().velocity.y > -0.6 && Player.GetComponent<Rigidbody>().velocity.y < 0.6)
        {
            Player.GetComponent<SpriteRenderer>().sprite = Glide;
            Player.GetComponent<SphereCollider>().center = new Vector3(0, -0.04f);
        }
        else
        {
            Player.GetComponent<SpriteRenderer>().sprite = Flap;
            Player.GetComponent<SphereCollider>().center = Vector3.zero;
        }
    }
}
