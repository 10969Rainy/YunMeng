using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//相机跟随
public class CameraFollow : MonoBehaviour {

    private Vector3 offset;
    private Transform playerTransform;

    void Start() {

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        offset = transform.position - playerTransform.position;
    }

    void Update () {

		Vector3 cameraPos = playerTransform.position + offset;
		transform.position = Vector3.Lerp (transform.position, cameraPos, 5 * Time.deltaTime);
    }
}
