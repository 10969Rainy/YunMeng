using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour {

    public enum Storys { Fire, Water, };
    public Storys story = Storys.Fire;

    public Text npc;
    public Text player;
    public Button okBtn;
    public Button noBtn;

    private bool isOK;
    private bool fire = false;

    //定义NPC对话数据
    public string[] story1 = { "村庄发生火灾了，是否帮忙灭火？", "成功扑灭了火灾", "离开了村庄" };
    
    //当前对话索引
    private int index = 0;
	
	void Update ()
    {

        switch (story)
        {
            case Storys.Fire:
                DoFire();
                break;
            case Storys.Water:
                break;
            default:
                break;
        }
    }

    public void DoFire()
    {
        npc.text = "旁白1：" + story1[0];
        okBtn.GetComponentInChildren<Text>().text = "帮忙";
        noBtn.GetComponentInChildren<Text>().text = "拒绝";
        if (fire)
        {
            if (isOK)
            {
                npc.text = "旁白：" + story1[1];
            }
            else
            {
                npc.text = "旁白：" + story1[2];
            }
        }

        fire = false;
    }

    public void ClickOK()
    {
        fire = true;
        isOK = true;
    }

    public void ClickNO()
    {
        fire = true;
        isOK = false;
    }
}
