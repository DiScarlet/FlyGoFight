//Main Bird's script
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//TODO: Don't forget to create the readme this time.

public class BirdMain : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    //Unity elements
    private Camera mainCamera;
    private Rigidbody2D rigidBody;
    private LineRenderer lr;
    private PolygonCollider2D collider;
    public LayerMask groundLayer;

    //Local fields
    private Vector3 _anchorPosition;
    private bool _birdLaunched;
    private float _timeSittingAround;
    private float minX = -10f;
    private float maxTimeIdle = 1.5f;
    //manage lifes
    private int birdsLeft = 3;

    //Serialized fields
    [SerializeField] private float launchPower = 500;
    [SerializeField] private float maxPullDistance = 2.5f;
    [SerializeField] private float skinWidth = 0.05f;
    [SerializeField] private Transform slingshotAnchor;
    [SerializeField] private SlingshotBands slingshotBands;
    //Sprites change depending on user on the slingshot or flying
    [SerializeField] private GameObject normalSpriteObject;
    [SerializeField] private GameObject slingSpriteObject;
    //X Limits on bird's position
    [SerializeField] private float maxX = 20f;
    [SerializeField] private GameObject awaitngBird1;
    [SerializeField] private GameObject awaitngBird2;
    [SerializeField] private LevelController levelController;

    [SerializeField] private TrailRenderer trail;

    private void Awake()
    {
        //Get all the elements on the startup
        mainCamera = Camera.main;
        lr = GetComponent<LineRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        //assign positions
        _anchorPosition = slingshotAnchor.position;
        transform.position = _anchorPosition;

        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.gravityScale = 0f;
        rigidBody.linearVelocity = Vector2.zero;

        //set aiming sprite, turn off flying sprite
        normalSpriteObject.SetActive(false);
        slingSpriteObject.SetActive(true);

        trail.enabled = false;
        trail.emitting = false;
    }

    private void Update()
    {
        //Idle position
        if (_birdLaunched &&
            rigidBody.linearVelocity.magnitude <= 0.1f)
        {
            _timeSittingAround += Time.deltaTime;
        }

        //Check for chicken outside of current camera bounds - hardcoded for now
        Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
        if (transform.position.x < minX ||
            transform.position.x > maxX ||
            viewPos.y < -0.1f ||
            viewPos.y > 1.1f ||
            _timeSittingAround > maxTimeIdle)
        {
            RespawnTheBird();
        }

        //Create a line of arrows pointing the trjectory
        if (lr.enabled)
        {
            Vector3 vector = transform.position - _anchorPosition;
            Vector3 flippedEnd = _anchorPosition - vector;

            lr.SetPosition(0, _anchorPosition);
            lr.SetPosition(1, flippedEnd);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _anchorPosition = transform.position;

        //set aiming sprite, turn off flying sprite
        normalSpriteObject.SetActive(false);
        slingSpriteObject.SetActive(true);

        lr.enabled = true;

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.gravityScale = 0f;

        slingshotBands.SetBird(transform);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //set flying sprite, turn off aiming sprite
        normalSpriteObject.SetActive(true);
        slingSpriteObject.SetActive(false);

        lr.enabled = false;

        Vector2 launchDirection = _anchorPosition - transform.position;

        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.gravityScale = 1f;

        rigidBody.AddForce(launchDirection * launchPower);


        _birdLaunched = true;

        slingshotBands.ClearBird();

        trail.Clear();
        trail.enabled = true;
        trail.emitting = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        trail.emitting = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_birdLaunched)
            return;

        //Get current world position
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        worldPos.z = transform.position.z;

        //Get pull vector (offset)
        Vector3 pullVector = worldPos - _anchorPosition;
        pullVector = Vector3.ClampMagnitude(pullVector, maxPullDistance);

        //Calculate target position
        Vector3 targetPos = _anchorPosition + pullVector;

        float colliderBottom = collider.bounds.extents.y;

        //Simulate hit to the ground using raycasts to check if there are objects which do not allow the bird to be pulled in that position
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(targetPos.x, _anchorPosition.y),
            Vector2.down,
            Mathf.Infinity,
            groundLayer
        );

        //If raycast returned collision, recalculate allowed position
        if (hit.collider != null)
        {
            float minY = hit.point.y + colliderBottom + skinWidth;
            if (targetPos.y < minY)
                targetPos.y = minY;
        }

        //Finally, assign the target position
        transform.position = targetPos;
    }

    public void RespawnTheBird()
    {
        birdsLeft--;
        GameManager.Instance.BirdsLeft = birdsLeft;

        //if no more lifes left navigate to lose menu
        if (birdsLeft <= 0)
        {
            GameManager.Instance.LastLevelName =
    SceneManager.GetActiveScene().name;

            string loseMenu = "LoseMenu";
            StartCoroutine(levelController.LoadLevel(loseMenu));
            return;
        }

        //if lifes left, remove one bird from waiting and put it on the slingshot
        if (birdsLeft == 2)
            awaitngBird2.SetActive(false);
        else if (birdsLeft == 1)
            awaitngBird1.SetActive(false);

        _birdLaunched = false;
        _timeSittingAround = 0f;

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.angularVelocity = 0;
        rigidBody.bodyType = RigidbodyType2D.Kinematic;
        rigidBody.gravityScale = 0f;

        transform.position = _anchorPosition;
        transform.rotation = Quaternion.identity;

        trail.Clear();
        trail.enabled = false;

        normalSpriteObject.SetActive(false);
        slingSpriteObject.SetActive(true);

        mainCamera.transform.position = new Vector3(-0.05114731f, -0.3377109f, -10f);
    }
}