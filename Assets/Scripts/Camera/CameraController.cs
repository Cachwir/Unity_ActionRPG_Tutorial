using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour, IPersistent
{
    public MonoBehaviour target;
    public float moveSpeed;

    public bool IsManuallyControlled { get; set; }
    private Vector3 targetPosition;
    private BoxCollider2D mapBounds;
    private Vector2 minBounds;
    private Vector2 maxBounds;

    private Camera gameCamera;
    public float CameraHalfWidth { get; set; }
    public float CameraHalfHeight { get; set; }

    static private bool cameraExists = false;

    // Use this for initialization
    void Start ()
    {
        if (!cameraExists) {
            cameraExists = true;
            DontDestroyOnLoad(transform.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        IsManuallyControlled = false;
        gameCamera = GetComponent<Camera>();
        CameraHalfHeight = gameCamera.orthographicSize;
        CameraHalfWidth = CameraHalfHeight * Screen.width / Screen.height;
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void FixedUpdate()
    {
        HandleMovementsOnUpdate();
    }

    protected void HandleMovementsOnUpdate()
    {
        if (!IsManuallyControlled)
        {
            this.targetPosition = new Vector3(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosition, this.moveSpeed * Time.fixedDeltaTime);
        }

        this.transform.position = GetClampedPosition(transform.position);
    }

    public void SetMapBounds(BoxCollider2D mapBounds)
    {
        mapBounds = FindObjectOfType<MapBounds>().GetComponent<BoxCollider2D>();
        minBounds = mapBounds.bounds.min;
        maxBounds = mapBounds.bounds.max;
    }

    public float GetClampedXPosition(float xPosition)
    {
        return Mathf.Clamp(xPosition, minBounds.x + CameraHalfWidth, maxBounds.x - CameraHalfWidth);
    }

    public float GetClampedYPosition(float yPosition)
    {
        return Mathf.Clamp(yPosition, minBounds.y + CameraHalfHeight, maxBounds.y - CameraHalfHeight);
    }

    public Vector3 GetClampedPosition(Vector3 position)
    {
        return new Vector3(GetClampedXPosition(position.x), GetClampedYPosition(position.y), position.z);
    }

    public void SetTarget(MonoBehaviour newTarget)
    {
        target = newTarget;
    }

    public void SwitchToManualControl()
    {
        IsManuallyControlled = true;
    }

    public void SwitchToAutomaticControl()
    {
        IsManuallyControlled = false;
    }
}
