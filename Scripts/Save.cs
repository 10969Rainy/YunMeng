using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存的数据
[System.Serializable]
public class Save {

    //角色的位置，方向
    public Vector3 playerPos;
    public PlayerController.PlayerDirection playerDir = PlayerController.PlayerDirection.South;

    //相机的位置
    public Vector3 cameraPos;

    //格子的位置，种类，是否被占，是否可走，是否可视
    public List<Vector3> placementPos = new List<Vector3>();
    public List<PlacementManager.Items> item =new List< PlacementManager.Items>();
    public List<bool> isExist = new List<bool>();
    public List<bool> canWalk = new List<bool>();
    public List<bool> canView = new List<bool>();
    public List<Sprite> sprite = new List<Sprite>();

    //效果文字
    public string infoText;

    //回合数
    public int round;
}
