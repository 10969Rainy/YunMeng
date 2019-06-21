using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManagers : MonoBehaviour {

    private GameObject player;
    private Transform _camera;
    private GameObject[] placements;
    private Text roundText;
    private Text infoText;

    //角色的位置，方向
    public Vector3 playerPos;
    public PlayerController.PlayerDirection playerDir = PlayerController.PlayerDirection.South;

    public Vector3 cameraPos;

    //效果文字
    public string info;

    //回合数
    public int round;
    
    void Start () {

        player = GameObject.Find("Player");
        _camera = GameObject.Find("Main Camera").transform;
        Invoke("InitGame", 0.5f);
        infoText = GameObject.Find("InfoText").GetComponent<Text>();
        roundText = GameObject.Find("RoundText").GetComponent<Text>();
    }
	
	void Update () {

        playerPos = player.transform.position;
        playerDir = player.GetComponent<PlayerController>().playerDirection;

        cameraPos = _camera.position;

        info = infoText.text;

        //round = roundText.text;
        round = MapsManager.round;
    }

    void InitGame()
    {
        placements = GameObject.FindGameObjectsWithTag("Map");
    }


    //保存，读取游戏
    private Save SaveGame()
    {
        Save save = new Save();

        //保存角色位置，方向
        if (player != null)
        {
            save.playerPos = playerPos;
            save.playerDir = playerDir;
        }

        //保存相机位置
        if (_camera != null)
        {
            save.cameraPos = cameraPos;
        }

        //保存地图块状态
        foreach (GameObject go in placements)
        {
            save.placementPos.Add(go.transform.position);
            save.item.Add(go.GetComponent<PlacementManager>().item);
            save.isExist.Add(go.GetComponent<PlacementManager>().isExist);
            save.canWalk.Add(go.GetComponent<PlacementManager>().canWalk);
            save.canView.Add(go.GetComponent<PlacementManager>().canView);
            save.sprite.Add(go.GetComponent<SpriteRenderer>().sprite);
        }

        //保存信息文字
        if (infoText != null)
        {
            save.infoText = info;
        }

        //保存回合数
        if (roundText != null)
        {
            save.round = round;
        }

        return save;
    }

    private void LoadGame(Save save)
    {
        if (player != null)
        {
            player.transform.position = save.playerPos;
            player.GetComponent<PlayerController>().playerDirection = save.playerDir;
        }

        if (_camera != null)
        {
            _camera.position = save.cameraPos;
        }

        for (int x = 0; x < save.placementPos.Count; x++)
        {
            placements[x].transform.position = save.placementPos[x];
            placements[x].GetComponent<PlacementManager>().item = save.item[x];
            placements[x].GetComponent<PlacementManager>().isExist = save.isExist[x];
            placements[x].GetComponent<PlacementManager>().canWalk = save.canWalk[x];
            placements[x].GetComponent<PlacementManager>().canView = save.canView[x];
            placements[x].GetComponent<SpriteRenderer>().sprite = save.sprite[x];
        }

        if (infoText != null)
        {
            infoText.text = save.infoText;
        }

        if (roundText != null)
        {
            roundText.text = save.round + "";
            MapsManager.round = save.round;
        }
    }

    public void SaveAsJSON()
    {
        Save save = SaveGame();
        //存档路径
        string filePath = Application.persistentDataPath + "/byJSON.json";
        //将存档转为JSON
        string json = JsonUtility.ToJson(save);

        StreamWriter sw = new StreamWriter(filePath);
        //写入文件
        sw.Write(json);
        sw.Close();
    }

    public void LoadAsJSON()
    {
        string filePath = Application.persistentDataPath + "/byJSON.json";

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
