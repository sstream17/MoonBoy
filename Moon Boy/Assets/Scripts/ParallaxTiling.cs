using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;

public class ParallaxTiling : MonoBehaviour
{

    enum Direction {Right = 1, Left = -1};

    public int offsetX = 0;

    public bool hasLeftConnection = false;
    public bool hasRightConnection = false;

    private float spriteWidth = 60f;
    private Camera gameCamera;
    private Transform thisTransform;


    void Awake() {
        gameCamera = Camera.main;
        thisTransform = transform;
    }
    // Start is called before the first frame update
    void Start() {
        SpriteShapeRenderer spriteRenderer = GetComponent<SpriteShapeRenderer>();
    }


    void MakeConnection(Direction direction) {
        Vector3 newPosition = new Vector3(thisTransform.position.x + (spriteWidth * (int) direction), thisTransform.position.y, thisTransform.position.z);
        Transform newConnection = Instantiate(thisTransform, newPosition, thisTransform.rotation);
        newConnection.parent = thisTransform;
        if (direction == Direction.Right) {
            newConnection.GetComponent<ParallaxTiling>().hasLeftConnection = true;
        }
        else {
            newConnection.GetComponent<ParallaxTiling>().hasRightConnection = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!hasLeftConnection || !hasRightConnection) {
            float cameraHorizontalExtend = gameCamera.orthographicSize * Screen.width / Screen.height;
            float edgeVisiblePositionRight = (thisTransform.position.x + spriteWidth / 2) - cameraHorizontalExtend;
            float edgeVisiblePositionLeft = (thisTransform.position.x - spriteWidth / 2) + cameraHorizontalExtend;

            if ((gameCamera.transform.position.x >= edgeVisiblePositionRight - offsetX) && !hasRightConnection) {
                MakeConnection(Direction.Right);
                hasRightConnection = true;
            }
            else if ((gameCamera.transform.position.x <= edgeVisiblePositionLeft + offsetX) && !hasLeftConnection) {
                MakeConnection(Direction.Left);
                hasLeftConnection = true;
            }
        }
    }
}
