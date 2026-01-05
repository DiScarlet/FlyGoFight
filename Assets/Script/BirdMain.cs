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

    [SerializeField] private float _launchPower = 500;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
        _anchorPosition = transform.position;
        _collider = GetComponent<PolygonCollider2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetComponent<LineRenderer>().SetPosition(0, transform.position);
        GetComponent<LineRenderer>().SetPosition(1, _anchorPosition);
        

        //idle position
        if (_birdLaunched && 
            GetComponent<Rigidbody2D>().linearVelocity.magnitude <= 0.1)
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
        GetComponent<LineRenderer>().enabled = true;

        _collider.enabled = false;
        _rigidBody.gravityScale = 0;
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _collider.enabled = true;
        _rigidBody.gravityScale = 1;

        spriteRenderer.color = Color.white;

        Vector2 directionToInitialPosition = _anchorPosition - transform.position;
    
        GetComponent<Rigidbody2D>().AddForce(directionToInitialPosition * _launchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;

        _birdLaunched = true;

        GetComponent<LineRenderer>().enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        worldPos.z = transform.position.z;
        transform.position = worldPos;
    }
}
