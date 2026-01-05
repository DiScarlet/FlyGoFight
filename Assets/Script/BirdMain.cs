//Main Bird's script
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


//TODO: Add some ind of a slingshot, 'cos it's really hard to push for player off the ground
// if you disabled the ability of the player to launch bird below the ground level.
public class BirdMain : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    //Unity elements
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Rigidbody2D rigidBody;
    private LineRenderer lr;
    private PolygonCollider2D collider;
    public LayerMask groundLayer;

    //Local fields
    private Vector3 _anchorPosition;
    private bool _birdLaunched;
    private float _timeSittingAround;

    //Serialized fields
    [SerializeField] private float launchPower = 500;
    [SerializeField] private float maxPullDistance = 2.5f;
    [SerializeField] private float skinWidth = 0.05f;

    private void Awake()
    {
        //Get all the elements on te startup
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        lr = GetComponent<LineRenderer>();
        collider = GetComponent<PolygonCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();

        _anchorPosition = transform.position;
    }

    private void Update()
    {
        //Create a line of arrows pointing the trjectory
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, _anchorPosition);
        

        //Idle position
        if (_birdLaunched &&
            rigidBody.linearVelocity.magnitude <= 0.1)
        {
            _timeSittingAround += Time.deltaTime;
        }

        //Check for chicken outside of current camera bounds - hardcoded for now
        if (transform.position.y > 10 || 
            transform.position.y < -10 ||
            transform.position.x > 10 ||
            transform.position.x < -10 ||
            _timeSittingAround > 3)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _anchorPosition = transform.position;

        spriteRenderer.color = Color.red;
        lr.enabled = true;

        rigidBody.gravityScale = 0;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.color = Color.white;

        Vector2 directionToInitialPosition = _anchorPosition - transform.position;

        rigidBody.AddForce(directionToInitialPosition * launchPower);
        rigidBody.gravityScale = 1;

        _birdLaunched = true;

        lr.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        if(hit.collider != null)
        {
            float minY = hit.point.y + colliderBottom + skinWidth;
            if (targetPos.y < minY)
                targetPos.y = minY;
        }

        //Finally, assign the target position
        transform.position = targetPos; 
    }
}
