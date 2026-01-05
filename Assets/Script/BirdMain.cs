//Main Bird's script
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BirdMain : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector3 _anchorPosition;
    private bool _birdLaunched;
    private float _timeSittingAround;
    private PolygonCollider2D _collider;
    private Rigidbody2D _rigidBody;
    private LineRenderer lr;

    public LayerMask groundLayer;

    [SerializeField] private float _launchPower = 500;
    [SerializeField] private float maxPullDistance = 2.5f;
    [SerializeField] private float skinWidth = 0.05f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        lr = GetComponent<LineRenderer>();
        _anchorPosition = transform.position;
        _collider = GetComponent<PolygonCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, _anchorPosition);
        

        //idle position
        if (_birdLaunched &&
            _rigidBody.linearVelocity.magnitude <= 0.1)
        {
            _timeSittingAround += Time.deltaTime;
        }
        //check for chicken outside of current camera bounds - hardcoded for now
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

        _rigidBody.gravityScale = 0;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        spriteRenderer.color = Color.white;

        Vector2 directionToInitialPosition = _anchorPosition - transform.position;

        _rigidBody.AddForce(directionToInitialPosition * _launchPower);
        _rigidBody.gravityScale = 1;

        _birdLaunched = true;

        lr.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(eventData.position);// i need some help probabaly
        worldPos.z = transform.position.z;

        Vector3 pullVector = worldPos - _anchorPosition;
        pullVector = Vector3.ClampMagnitude(pullVector, maxPullDistance);

        Vector3 targetPos = _anchorPosition + pullVector;

        float colliderBottom = _collider.bounds.extents.y;

        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(targetPos.x, _anchorPosition.y),
            Vector2.down,
            Mathf.Infinity,
            groundLayer
        );

        if(hit.collider != null)
        {
            float minY = hit.point.y + colliderBottom + skinWidth;
            if (targetPos.y < minY)
                targetPos.y = minY;
        }

        transform.position = targetPos;
    }
}
