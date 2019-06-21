using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LinkController : MonoBehaviour
{

    private bool isMoving = false;
    private enum PlayerDirection { North, East, South, West, NorthEast, NorthWest, SouthEast, SouthWest };
    private PlayerDirection playerDirection;

    public float speed = 5.0f;
    private Vector2 moveDir;

    private Rigidbody2D rb2D;
    private Animator anim;

    private int point = 10;

    void Start()
    {

        rb2D = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();

        playerDirection = PlayerDirection.South;
    }

    void FixedUpdate()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rb2D.MovePosition(rb2D.position + moveDir * (speed * Time.fixedDeltaTime));
    }

    void Update()
    {

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 1) { playerDirection = PlayerDirection.North; isMoving = true; }
        if (h == 0 && v == -1) { playerDirection = PlayerDirection.South; isMoving = true; }
        if (h == 1 && v == 0) { playerDirection = PlayerDirection.East; isMoving = true; }
        if (h == -1 && v == 0) { playerDirection = PlayerDirection.West; isMoving = true; }
        if (h == 0 && v == 0) { isMoving = false; }

        switch (playerDirection)
        {
            case PlayerDirection.North:
                if (isMoving) { anim.Play("Link_N_Walk"); }
                else { anim.Play("Link_N_Idle"); }
                break;
            case PlayerDirection.East:
                if (isMoving) { anim.Play("Link_E_Walk"); }
                else { anim.Play("Link_E_Idle"); }
                break;
            case PlayerDirection.South:
                if (isMoving) { anim.Play("Link_S_Walk"); }
                else { anim.Play("Link_S_Idle"); }
                break;
            case PlayerDirection.West:
                if (isMoving) { anim.Play("Link_W_Walk"); }
                else { anim.Play("Link_W_Idle"); }
                break;
            default:
                break;
        }
    }

    //保存，读取游戏

    private Save SaveGame()
    {
        Save save = new Save();

        if (gameObject != null)
        {
            save.playerPos = rb2D.position;
        }

        //save.round = point;

        return save;
    }

    private void LoadGame(Save save)
    {
        if (gameObject != null)
        {
            rb2D.position = save.playerPos;
        }
    }

    public void SaveAsJSON()
    {
        Save save = SaveGame();
        //存档路径
        string filePath = Application.dataPath + "/SaveFiles" + "/byJSON.json";
        //将存档转为JSON
        string json = JsonUtility.ToJson(save);

        StreamWriter sw = new StreamWriter(filePath);
        //写入文件
        sw.Write(json);
        sw.Close();
    }

    public void LoadAsJSON()
    {
        string filePath = Application.dataPath + "/SaveFiles" + "/byJSON.json";

        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            //读取文件
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //将JSON转为存档
            Save save = JsonUtility.FromJson<Save>(jsonStr);
            //调用读取游戏方法
            LoadGame(save);
        }
        else
        {
            Debug.Log("Not Save");
        }
    }
}