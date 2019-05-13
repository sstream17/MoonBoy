using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    public  Transform[] backgrounds;
    private float[] parallaxScales;
    public float smoothing = 1f;
    public float smoothingY = 2f;

    private Transform gameCamera;
    private Vector3 previousCameraPosition;


    void Awake() {
        gameCamera = Camera.main.transform;
    }
    // Start is called before the first frame update
    void Start() {
        previousCameraPosition = gameCamera.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++) {
            parallaxScales[i] = backgrounds[i].position.z;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        for (int i = 0; i < backgrounds.Length; i++) {
            float parallaxX = (previousCameraPosition.x - gameCamera.position.x) * parallaxScales[i];
            float parallaxY = (previousCameraPosition.y - gameCamera.position.y) * parallaxScales[i] * smoothingY;
            float targetPositionX = backgrounds[i].position.x + parallaxX;
            float targetPositionY = backgrounds[i].position.y + parallaxY;
            Vector3 backgroundTargetPositon = new Vector3(targetPositionX, targetPositionY, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPositon, smoothing * Time.fixedDeltaTime);
        }
        previousCameraPosition = gameCamera.position;
    }
}
