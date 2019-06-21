using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour {

    //格子的枚举
    public enum Items
    {
        Normal, Wind, Compass, Light, Box, Snow, Stone, Village, Forest, NPC, Start, End, Null
    }
    public Items item = Items.Normal;

    private GameObject player;

    //角色被风移动到的位置
    private Vector3 targetPos;

    //地图是否被占，是否可走，是否可视
    public bool isExist = false;
    public bool canWalk = false;
    public bool canView = false;

    //效果信息文本
    private Text infoText;

    //射线检测
    [HideInInspector]
    public RaycastHit[] hit;

    //休息
    private bool canClick = true;
    private float sleepTime = 2.0f;
    private float timer = 0;
    private Slider coldUI;

    //格子的图片
    public Sprite[] compass;
    private bool clickCompass;
    public Sprite[] stone;
    private int stoneIndex;
    public Sprite lights;
    public Sprite wind;
    public Sprite snow;
    public Sprite[] box;
    private bool clickBox = false;
    public Sprite end;

    public Sprite[] zhou;

    void Start() {

        //设置起点
        if (transform.position == new Vector3(20.0f, 0.0f, 20.25f))
        {
            item = Items.Start;
            isExist = true;
        }

        player = GameObject.FindGameObjectWithTag("Player");

        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        coldUI = GameObject.Find("ColdUI").GetComponent<Slider>();

        infoText.text = "游戏开始";

        //检测周围的格子，返回一个数组
        hit = Physics.SphereCastAll(transform.position, 4.0f, transform.forward);

        stoneIndex = Random.Range(0, stone.Length);
    }

    void Update()
    {

        //等一回合
        if (!canClick)
        {
            timer += Time.deltaTime;
            coldUI.value = 1 - timer / sleepTime;
            if (timer >= sleepTime)
            {
                canClick = true;
            }
        }

        if (gameObject.GetComponent<PlacementManager>().isExist && gameObject.GetComponent<PlacementManager>().item == Items.Normal)
        {
            infoText.GetComponentInParent<Image>().sprite = zhou[1];
            infoText.text = "呼噜。。呼噜。。";
        }
        else if (gameObject.GetComponent<PlacementManager>().isExist && gameObject.GetComponent<PlacementManager>().item != Items.Normal)
        {
            infoText.GetComponentInParent<Image>().sprite = zhou[0];
        }

        if (Input.GetMouseButtonDown(0) && !MapsManager.isPause && canClick && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //如果点击到了特殊的格子，触发相应的效果
                if (hit.collider.GetComponent<PlacementManager>().item != Items.Normal && hit.collider.GetComponent<PlacementManager>().canWalk)
                {
                    timer = 0;
                    canClick = false;
                    switch (hit.collider.GetComponent<PlacementManager>().item)
                    {
                        case Items.Wind:
                            //触发风的效果
                            DoWind(hit);
                            break;
                        case Items.Compass:
                            //触发司南的效果
                            DoCompass();
                            break;
                        case Items.Light:
                            //触发灯的效果
                            DoLight();
                            break;
                        case Items.Box:
                            //触发箱子的效果
                            DoBox();
                            break;
                        case Items.Snow:
                            //触发雪的效果
                            Invoke("DoSnow", sleepTime);
                            //DoSnow();
                            break;
                        case Items.Stone:
                            //触发石头的效果
                            DoStone();
                            //DoThunder();
                            break;
                        case Items.Village:
                            //触发村庄的效果
                            DoVillage();
                            break;
                        case Items.Forest:
                            //触发树林的效果
                            DoForest();
                            break;
                        case Items.NPC:
                            //触发NPC的效果
                            DoNPC();
                            break;
                        case Items.End:
                            //游戏结束
                            DoEnd();
                            break;
                        default:
                            break;
                    }


                    if (hit.collider.GetComponent<PlacementManager>().item == PlacementManager.Items.Compass)
                    {
                        clickCompass = true;
                    }
                    else
                    {
                        clickCompass = false;
                    }
                }
            }
        }

        //将可视的特殊格子显示出来
        if (canView)
        {
            switch (item)
            {
                case Items.Normal:
                    break;
                case Items.Wind:
                    //角色位于格子上才设为可视
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    if (isExist)
                    {
                        gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = wind;
                        gameObject.transform.Find("Sprite1").GetComponent<Transform>().eulerAngles = new Vector3(90f, 0.0f, 30.0f);
                    }
                    break;
                case Items.Compass:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = compass[0];
                    gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[7];
                    gameObject.transform.Find("Sprite1").position = new Vector3(gameObject.transform.position.x, 1.1f, gameObject.transform.position.z);
                    gameObject.transform.Find("Sprite1").localScale = new Vector3(0.5f, 0.5f, 1.0f);
                    gameObject.transform.Find("Sprite1").LookAt(Camera.main.transform);
                    if ((gameObject.transform.position - player.transform.position).magnitude <= 6)
                    {
                        if (clickCompass)
                        {
                            DoCompass();
                        }
                    }
                    break;
                case Items.Light:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = lights;
                    gameObject.transform.Find("Sprite1").position = new Vector3(gameObject.transform.position.x, 1.1f, gameObject.transform.position.z);
                    gameObject.transform.Find("Sprite1").LookAt(Camera.main.transform);
                    break;
                case Items.Box:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    if (clickBox)
                    {
                        gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = box[1];
                    }
                    else
                    {
                        gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = box[0];
                    }
                    gameObject.transform.Find("Sprite1").position = new Vector3(gameObject.transform.position.x, 0.0f, gameObject.transform.position.z);
                    gameObject.transform.Find("Sprite1").LookAt(Camera.main.transform);
                    break;
                case Items.Snow:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = snow;
                    gameObject.transform.Find("Sprite1").GetComponent<Transform>().eulerAngles = new Vector3(90f, 0.0f, 30.0f);
                    break;
                case Items.Stone:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = stone[stoneIndex];
                    gameObject.transform.Find("Sprite1").position = new Vector3(gameObject.transform.position.x, 1.1f, gameObject.transform.position.z);
                    gameObject.transform.Find("Sprite1").localScale = new Vector3(0.5f, 0.5f, 1.0f);
                    gameObject.transform.Find("Sprite1").LookAt(Camera.main.transform);
                    break;
                case Items.Village:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    break;
                case Items.Forest:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    break;
                case Items.NPC:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                    break;
                case Items.Start:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                    break;
                case Items.End:
                    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.transform.Find("Sprite1").GetComponentInChildren<SpriteRenderer>().sprite = end;
                    gameObject.transform.Find("Sprite1").position = new Vector3(gameObject.transform.position.x, 1.1f, gameObject.transform.position.z);
                    gameObject.transform.Find("Sprite1").LookAt(Camera.main.transform);
                    break;
                case Items.Null:
                    break;
                default:
                    break;
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    //风的效果：将角色吹到周围任意一格
    private void DoWind(RaycastHit hit)
    {
        if (item == Items.Wind)
        {
            if (player.activeSelf && !player.GetComponent<PlayerController>().beMoved)
            {
                int dir = Random.Range(1, 7);
                switch (dir)
                {
                    case 1:
                        targetPos = new Vector3(transform.position.x - 4.0f, transform.position.y + 2.2f, transform.position.z + 2.25f);
                        infoText.text = "吹了一阵西北风";
                        break;
                    case 2:
                        targetPos = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z + 4.5f);
                        infoText.text = "吹了一阵东北风";
                        break;
                    case 3:
                        targetPos = new Vector3(transform.position.x + 4.0f, transform.position.y + 2.2f, transform.position.z + 2.25f);
                        infoText.text = "吹了一阵东风";
                        break;
                    case 4:
                        targetPos = new Vector3(transform.position.x + 4.0f, transform.position.y + 2.2f, transform.position.z - 2.25f);
                        infoText.text = "吹了一阵东南风";
                        break;
                    case 5:
                        targetPos = new Vector3(transform.position.x, transform.position.y + 2.2f, transform.position.z - 4.5f);
                        infoText.text = "吹了一阵西南风";
                        break;
                    case 6:
                        targetPos = new Vector3(transform.position.x - 4.0f, transform.position.y + 2.2f, transform.position.z - 2.25f);
                        infoText.text = "吹了一阵西风";
                        break;
                    default:
                        break;
                }
                player.GetComponent<PlayerController>().BeMoved(targetPos);

                GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
                //遍历数组里的地图块，将地图状态设为未被占
                foreach (GameObject go in maps)
                {
                    go.GetComponent<PlacementManager>().isExist = false;
                }

                //将点击的地图状态设为被占
                hit.collider.GetComponent<PlacementManager>().isExist = true;
            }
        }
    }

    //司南的效果：告知终点的大概位置，六个方向
    private void DoCompass()
    {
        if (item == Items.Compass)
        {
            GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
            //遍历集合里的地图块，将地图状态设为未被占
            foreach (GameObject go in maps)
            {
                if (go.GetComponent<PlacementManager>().item == Items.End)
                {
                    Vector3 targetDir = go.transform.position - transform.position;
                    Vector3 up = transform.up;
                    float angle = Vector3.SignedAngle(targetDir, up, Vector3.up);
                    if ((angle - 30.0f) == 0)
                    {
                        infoText.text = "终点在北面";
                        gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[7];
                    }
                    if ((angle - 30.0f) > 0)
                    {
                        if ((angle - 30.0f) < 60)
                        {
                            infoText.text = "终点在西北面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[4];
                        }
                        if ((angle - 30.0f) > 60 && (angle - 30.0f) < 120)
                        {
                            infoText.text = "终点在西面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[5];
                        }
                        if ((angle - 30.0f) > 120)
                        {
                            infoText.text = "终点在西南面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[6];
                        }
                    }
                    else
                    {
                        if ((angle - 30.0f) > -60)
                        {
                            infoText.text = "终点在东北面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[1];
                        }
                        if ((angle - 30.0f) < -60 && (angle - 30.0f) > -120)
                        {
                            infoText.text = "终点在东面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[2];
                        }
                        if ((angle - 30.0f) < -120)
                        {
                            infoText.text = "终点在东南面";
                            gameObject.transform.Find("Sprite1").Find("Sprite2").GetComponentInChildren<SpriteRenderer>().sprite = compass[3];
                        }
                    }
                }
            }
        }
    }
    
    //灯的效果：将周围两格设为可视
    private void DoLight()
    {
        if (item == Items.Light)
        {
            infoText.text = "灯照亮了周围两格";
            GetComponentInChildren<Light>().enabled = true;
            if (isExist)
            {
                GameObject[] lights = GameObject.FindGameObjectsWithTag("Map");
                foreach (GameObject go in lights)
                {
                    if (go.transform.position == transform.position + new Vector3(0.0f, 0.0f, 9.0f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(4.0f, 0.0f, 6.75f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(8.0f, 0.0f, 4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(8.0f, 0.0f, 0.0f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(8.0f, 0.0f, -4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(4.0f, 0.0f, -6.75f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(0.0f, 0.0f, -9f ))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-4.0f, 0.0f, -6.75f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-8.0f, 0.0f, -4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-8.0f, 0.0f, 0.0f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-8.0f, 0.0f, 4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-4.0f, 0.0f, 6.75f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    
                    if (go.transform.position == transform.position + new Vector3(-4.0f, 0.0f, 2.25f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(0.0f, 0.0f, 4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(4.0f, 0.0f, 2.25f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(4.0f, 0.0f, -2.25f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(0.0f, 0.0f, -4.5f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                    if (go.transform.position == transform.position + new Vector3(-4.0f, 0.0f, -2.25f))
                    {
                        go.GetComponent<PlacementManager>().canView = true;
                    }
                }
            }
        }
    }

    //箱子的效果：获得随机物品
    private void DoBox()
    {
        //TODO
        if (item == Items.Box)
        {
            infoText.text = "打开箱子，获得了";
            clickBox = true;
        }
    }

    //雪的效果：暂停一回合
    private void DoSnow()
    {
        if (item == Items.Snow)
        {
            infoText.text = "由于天气寒冷，暂停一回合";
            MapsManager mm = new MapsManager();
            mm.BePaused();
        }
    }

    //石头的效果：不可通过
    private void DoStone()
    {
        if (item == Items.Stone)
        {
            infoText.text = "被石头挡住了去路";
        }
    }

    //村庄的效果：遭遇随机事件
    private void DoVillage()
    {
        if (item == Items.Village)
        {

        }
    }

    //树林的效果：遭遇随机事件
    private void DoForest()
    {
        //TODO
        if (item == Items.Forest)
        {

        }
    }

    //NPC的效果：伙伴系统
    private void DoNPC()
    {
        //TODO
        if (item == Items.NPC)
        {

        }
    }

    private void DoEnd()
    {
        if (item == Items.End)
        {
            infoText.text = "恭喜你，到达终点！";
            MapsManager mm = GameObject.Find("Map").GetComponent<MapsManager>();
            mm.GameOver();
        }
    }
}
