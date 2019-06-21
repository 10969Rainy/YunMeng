using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //角色的移动状态
    private bool isMoving = false;
    public enum PlayerDirection { North, East, South, West, };
    public PlayerDirection playerDirection = PlayerDirection.South;

    private Animator anim;
    
    //点击的地图块的位置
    private Vector3 targetPos1;
    private bool canMove = false;
    //被迫移动的位置
    private Vector3 targetPos2;
    [HideInInspector]
    public bool beMoved = false;

    //用于望向相机
    public Transform cameraTransform;

    void Start() {
        
        anim = GetComponent<Animator>();
    }

    void Update() {

        //角色始终望向镜头
        transform.LookAt(cameraTransform);

        if (canMove)
        {
            //移动到targetPos1
            transform.position = Vector3.Lerp(transform.position, targetPos1, 5 * Time.deltaTime);
            //到达之后停止
            //Debug.Log(transform.position == Vector3.Lerp(transform.position, targetPos1, 5 * Time.deltaTime));
            if (transform.position == Vector3.Lerp(transform.position, targetPos1, 5 * Time.deltaTime) /*transform.position == targetPos1*/)
            {
                canMove = false;
            }
        }
        //被迫移动而且不是玩家操作移动
        if (beMoved && !canMove)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos2, 8 * Time.deltaTime);
            if (transform.position == Vector3.Lerp(transform.position, targetPos2, 8 * Time.deltaTime))
            {
                beMoved = false;
            }
        }

        //改变角色的方向
        switch (playerDirection)
        {
            case PlayerDirection.North:
                if (isMoving) { anim.Play("Player_Back"); }
                else { anim.Play("Player_Back"); }
                break;
            case PlayerDirection.East:
                if (isMoving) { anim.Play("Player_Right"); }
                else { anim.Play("Player_Right_Wink"); }
                break;
            case PlayerDirection.South:
                if (isMoving) { anim.Play("Player_Front"); }
                else { anim.Play("Player_Front_Wink"); }
                break;
            case PlayerDirection.West:
                if (isMoving) { anim.Play("Player_Left"); }
                else { anim.Play("Player_Left_Wink"); }
                break;
            default:
                break;
        }
    }

    //在角色的可移动范围里，将地图块设为可移动
    private void OnTriggerStay(Collider other)
    {
        other.gameObject.GetComponent<PlacementManager>().canWalk = true;
        other.gameObject.GetComponent<PlacementManager>().canView = true;
    }

    //在角色的可移动范围外，将地图块设为不可移动
    private void OnTriggerExit(Collider other)
    {
        other.gameObject.GetComponent<PlacementManager>().canWalk = false;
    }

    //玩家操作移动的方法
    public void MovePostion(Vector3 pos)
    {
        targetPos1 = pos;
        canMove = true;
    }

    //地形使角色移动的方法
    public void BeMoved(Vector3 pos)
    {
        targetPos2 = pos;
        beMoved = true;
    }
}
