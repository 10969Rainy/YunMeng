using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//开始界面的镜头控制
public class CameraController : MonoBehaviour {
    
    private Rigidbody2D rb2D;

    //之前的鼠标位置
    private Vector2 preMousePos;

    void Start () {

        rb2D = GetComponent<Rigidbody2D>();
	}
	
	void Update () {

        //限制镜头的移动范围
        if (transform.position.x < -6)
        {
            transform.position = new Vector3(-6, 0, -10);
        }
        else if (transform.position.x > 6)
        {
            transform.position = new Vector3(6, 0, -10);
        }
        else if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, -10);
        }

        //鼠标左键按下，移动摄像头
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider.tag != "Player")
            {
                if (preMousePos.x <= 0)
                {
                    preMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
                else
                {
                    //当前的鼠标位置
                    Vector2 curMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                    Vector2 offset = curMousePos - preMousePos;
                    offset = offset * -0.5f;
                    rb2D.MovePosition(rb2D.position + offset * Time.deltaTime);
                    preMousePos = curMousePos;
                }
            }
        }
        else
        {
            preMousePos = Vector3.zero;
        }
    }
}
