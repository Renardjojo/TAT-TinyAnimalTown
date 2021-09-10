using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera mCam;
    public float RotationSpeed = 1f;
    private float t = 0f;
    private float yStart;
    private float yGoal;
    private bool isRotate = false;

    public Transform mTarget;
    private bool mIsInTargetMode = false;
    public float mTargetModeOrthoSize = 2f;
    private float mFreeModeOrthoSize = 5f;
    [Range(0f, 5f)]
    [SerializeField] protected float mFadeSpeed = 2f;
    public AnimationCurve mFadeAnimation;

    private Quaternion mBaseCameraAngle;

    public void SetInTargetMode()
    {
        if (!mIsInTargetMode)
            StartCoroutine(ZoomFreeToTarget());
        
        mIsInTargetMode = true;
        mFreeModeOrthoSize = mCam.orthographicSize;
    }

    public void SetInFreeMode()
    {
        if (mIsInTargetMode)
            StartCoroutine(ZoomTargetToFree());
        
        mIsInTargetMode = false;
        mCam.transform.localRotation = mBaseCameraAngle;
    }

    void Awake()
    {
        mBaseCameraAngle = mCam.transform.localRotation; 
    }

    // Start is called before the first frame update
    void Start()
    {
        mFreeModeOrthoSize = mCam.orthographicSize;
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
            transform.rotation = Quaternion.Euler(new Vector3(0f, Mathf.Lerp(yStart, yGoal, t), 0f));
            
        }
        
        if (mIsInTargetMode)
           mCam.transform.LookAt(mTarget.position, Vector3.up);
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
    
    protected IEnumerator ZoomFreeToTarget()
    {
        float t = 0f;
        do
        {
            t += Time.deltaTime / mFadeSpeed;
            mCam.orthographicSize = Mathf.Lerp(mFreeModeOrthoSize, mTargetModeOrthoSize, mFadeAnimation.Evaluate(t));
            yield return null;
        } while (t < 1f);
    }
    
    protected IEnumerator ZoomTargetToFree()
    {
        float t = 0f;
        do
        {
            t += Time.deltaTime / mFadeSpeed;
            mCam.orthographicSize = Mathf.Lerp(mTargetModeOrthoSize, mFreeModeOrthoSize, mFadeAnimation.Evaluate(t));
            yield return null;
        } while (t < 1f);
    }
}
