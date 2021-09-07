using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float RotationSpeed = 1f;
    private float t = 0f;
    private float yStart;
    private float yGoal;
    private bool isRotate = false;
    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            t += Time.deltaTime * RotationSpeed;
            if (t > 1f)
            {
                t = 1f;
                isRotate = false;
            }
            Debug.Log(t + " " + Mathf.Lerp(yStart, yGoal, t));
            transform.rotation = Quaternion.Euler(new Vector3(0f, Mathf.Lerp(yStart, yGoal, t), 0f));
            
        }
    }

    public void RotateLeft90()
    {
        if (!isRotate)
        {
            yStart = transform.rotation.eulerAngles.y;
            yGoal = yStart + 90;
            isRotate = true;
            t = 0f;
        }
    }

    public void RotateRight90()
    {
        if (!isRotate)
        {
            yStart = transform.rotation.eulerAngles.y;
            yGoal = yStart - 90;
            isRotate = true;
            t = 0f;
        }
    }
}
