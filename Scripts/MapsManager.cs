using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapsManager : MonoBehaviour {

    //地图的大小
    public int x1 = 5;
    public int x2 = 3;
    public int y1 = 5;
    public int y2 = 3;
    public GameObject map;

    //存放地图的集合
    List<GameObject> mapList = new List<GameObject>();

    //用于移动角色
    public Transform playerTransform;

    private PlayerController playerController;

    //用于更换地图的Sprite
    private SpriteRenderer sr;
    public Sprite fillMap;

    //休息
    private static bool canClick = true;
    private float sleepTime = 2.0f;
    private float timer = 0;
    private Slider coldUI;

    //记录步数
    private static Text roundText;
    [HideInInspector]
    public static int round = 0;

    //暂停状态
    [HideInInspector]
    public static bool isPause = false;

    public GameObject quitButton;

    void Start() {

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        coldUI = GameObject.Find("ColdUI").GetComponent<Slider>();

        roundText = GameObject.Find("RoundText").GetComponent<Text>();

        InitMap();
    }

    //初始化地图
    void InitMap() {

        //生成地图的一部分
        for (int x = 0; x < x1; x++)
        {
            for (int y = 0; y < x2; y++)
            {
                Instantiate(map, new Vector3(8.0f * x, 0, 4.5f * y), Quaternion.Euler(90, 0, 0), transform);
            }
        }
        //生成地图的另一部分
        for (int x = 0; x < y1; x++)
        {
            for (int y = 0; y < y2; y++)
            {
                Instantiate(map, new Vector3(4.0f + (8.0f * x), 0, -2.25f + (4.5f * y)), Quaternion.Euler(90, 0, 0), transform);
            }
        }

        Invoke("InitPlacement", 0.5f);
    }

    //初始化地图块
    private void InitPlacement()
    {
        //将所有地图块加入到集合里
        List<GameObject> placement = new List<GameObject>();
        GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
        foreach (GameObject go in maps)
        {
            placement.Add(go);
        }

        //生成终点
        List<GameObject> end = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //远离角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start && (go.transform.position - playerTransform.position).magnitude >= 16)
            {
                end.Add(go);
            }
        }
        int e = Random.Range(0, end.Count);
        end[e].GetComponent<PlacementManager>().item = PlacementManager.Items.End;
        //end[e].GetComponent<SpriteRenderer>().color = Color.red;
        placement.Remove(end[e]);

        //随机生成一个司南
        List<GameObject> compass = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //靠近角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start && (go.transform.position - playerTransform.position).magnitude <= 9)
            {
                compass.Add(go);
            }
        }
        int c = Random.Range(0, compass.Count);
        compass[c].GetComponent<PlacementManager>().item = PlacementManager.Items.Compass;
        placement.Remove(compass[c]);

        //随机生成一个灯
        List<GameObject> light = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //靠近角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start && (go.transform.position - playerTransform.position).magnitude <= 12)
            {
                light.Add(go);
            }
        }
        int l = Random.Range(0, light.Count);
        light[l].GetComponent<PlacementManager>().item = PlacementManager.Items.Light;
        placement.Remove(light[l]);

        //随机生成一个风
        List<GameObject> winds = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //不在起点和地图边界生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start && go.GetComponent<PlacementManager>().hit.Length == 7)
            {
                winds.Add(go);
            }
        }
        int w = Random.Range(0, winds.Count);
        winds[w].GetComponent<PlacementManager>().item = PlacementManager.Items.Wind;
        placement.Remove(winds[w]);

        //随机生成一个箱子
        List<GameObject> box = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //靠近角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start)
            {
                box.Add(go);
            }
        }
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            int b = Random.Range(0, box.Count);
            box[b].GetComponent<PlacementManager>().item = PlacementManager.Items.Box;
            placement.Remove(box[b]);
        }

        //随机生成一个雪
        List<GameObject> snow = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //靠近角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start)
            {
                snow.Add(go);
            }
        }
        int s = Random.Range(0, snow.Count);
        snow[s].GetComponent<PlacementManager>().item = PlacementManager.Items.Snow;
        placement.Remove(snow[s]);

        //随机生成一到三个石头
        List<GameObject> Stone = new List<GameObject>();
        foreach (GameObject go in placement)
        {
            //靠近角色及不在起点生成
            if (go.GetComponent<PlacementManager>().item != PlacementManager.Items.Start)
            {
                Stone.Add(go);
            }
        }
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            int t = Random.Range(0, Stone.Count);
            Stone[t].GetComponent<PlacementManager>().item = PlacementManager.Items.Stone;
            placement.Remove(Stone[t]);
        }
    }

    void Update() {

        GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");

        //将可视的地图设为可视
        foreach (GameObject go in maps)
        {
            //设为可视
            if (go.GetComponent<PlacementManager>().canView && go.GetComponent<PlacementManager>().item == PlacementManager.Items.Normal)
            {
                go.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        //等一回合
        if (!canClick)
        {
            timer += Time.deltaTime;
            coldUI.value = 1 - timer / sleepTime;
            coldUI.transform.Find("CS").GetComponent<RectTransform>().eulerAngles = new Vector3(0, 180, (1 - coldUI.value / 1) * 360);
            if (timer >= sleepTime)
            {
                canClick = true;
            }
        }

        //鼠标左键按下，移动角色
        if (Input.GetMouseButtonDown(0) && !isPause && canClick && EventSystem.current.IsPointerOverGameObject() == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //点击了地图并且在移动范围内
                if ((hit.collider.tag == "Map") && !playerController.beMoved && hit.collider.GetComponent<PlacementManager>().canWalk && hit.collider.GetComponent<PlacementManager>().item != PlacementManager.Items.Null && !hit.collider.GetComponent<PlacementManager>().isExist)
                {
                    timer = 0;
                    canClick = false;

                    round++;
                    roundText.text = round + "";

                    if (hit.collider.GetComponent<PlacementManager>().item != PlacementManager.Items.Compass && 
                        hit.collider.GetComponent<PlacementManager>().item != PlacementManager.Items.Stone &&
                        hit.collider.GetComponent<PlacementManager>().item != PlacementManager.Items.Light)
                    {
                        //调用角色的MovePosition移动位置
                        playerTransform.GetComponent<PlayerController>().MovePostion(hit.transform.position + new Vector3(0.0f, 2.2f, 0.0f));
                    }

                    //遍历所有的地图块，加入到地图集合里
                    //GameObject[] maps = GameObject.FindGameObjectsWithTag("Map");
                    foreach (GameObject go in maps)
                    {
                        mapList.Add(go);
                        //得到角色位于的地图块的位置，与点击的位置做差，得到角色的方位，改变角色的方向
                        if (go.GetComponent<PlacementManager>().isExist)
                        {
                            Vector3 existPos = go.GetComponent<Transform>().position;

                            float xOffset = hit.transform.position.x - existPos.x;
                            float zOffset = hit.transform.position.z - existPos.z;

                            if (xOffset <= 0 && zOffset > 0)
                            {
                                playerController.playerDirection = PlayerController.PlayerDirection.North;
                            }
                            if (xOffset >= 0 && zOffset < 0)
                            {
                                playerController.playerDirection = PlayerController.PlayerDirection.South;
                            }
                            if (xOffset < 0 && zOffset < 0)
                            {
                                playerController.playerDirection = PlayerController.PlayerDirection.East;
                            }
                            if (xOffset > 0 && zOffset > 0)
                            {
                                playerController.playerDirection = PlayerController.PlayerDirection.West;
                            }
                        }
                    }

                    //将被点击的地图块移出集合
                    mapList.Remove(hit.collider.gameObject);

                    //遍历集合里的地图块，将地图状态设为未被占
                    foreach (var go in mapList)
                    {
                        go.GetComponent<PlacementManager>().isExist = false;
                    }

                    //将点击的地图状态设为被占
                    hit.collider.GetComponent<PlacementManager>().isExist = true;

                    //将点击的地图sprite设为fillMap
                    sr = hit.collider.GetComponent<SpriteRenderer>();
                    sr.sprite = fillMap;
                }
            }
        }
    }

    public void BePaused()
    {
        timer = 0;
        canClick = false;

        round++;
        roundText.text = round + "";
    }

    public void GameOver()
    {
        quitButton.SetActive(true);
    }
}