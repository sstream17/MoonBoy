using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Pathfinding;

public class RandomizeLevel : MonoBehaviour
{

    public SpriteShapeController spriteShape;

    public GameObject[] enemies;
    public Weapon[] enemyWeapons;
    public int numberOfEnemies = 3;
    public float deltaX = 3f;
    public float deltaY = 3f;

    // Start is called before the first frame update
    void Start() {
        GameControl.control.enemyWeapons = enemyWeapons;
        Spline spline = spriteShape.spline;
        float xMinimum = spline.GetPosition(0).x + 20f;
        float xMaximum = spline.GetPosition(1).x;
        float yMinimum = spline.GetPosition(2).y;
        int splineIndex = 1;
        for (float currentX = xMinimum; currentX < xMaximum; currentX = currentX + 1 + (Random.value * deltaX)) {
          float lastY = spline.GetPosition(splineIndex - 1).y;
          float newY = lastY + Random.Range(-deltaY, deltaY);
          if (Mathf.Abs(lastY - yMinimum) < 8) {
            newY = lastY + deltaY;
          }
          spline.InsertPointAt(splineIndex, new Vector3(currentX, newY, 0f));
          spline.SetTangentMode(splineIndex, (ShapeTangentMode) Mathf.Floor(Random.Range(0f, 3f)));
          splineIndex = splineIndex + 1;
        }
        spriteShape.BakeCollider();
        AstarPath.active.Scan();
        spline = spriteShape.spline;
        int numberOfPoints = spline.GetPointCount() - 15;
        for (int i = 0; i < numberOfEnemies; i++) {
            Vector3 randomPosition = spline.GetPosition((int) Mathf.Floor(Random.value * numberOfPoints) + 4);
            randomPosition.y = randomPosition.y + 8f;
            randomPosition.z = 0;
            int randomEnemy = (int) Mathf.Floor(Random.value * enemies.Length);
            Instantiate(enemies[randomEnemy], randomPosition, Quaternion.identity);
        }
    }
}
