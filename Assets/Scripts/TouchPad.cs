using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public class VAxis
    {
        public string Name { get; private set; }
        public float Value { get; set; }

        public VAxis(string name)
        {
            Name = name;
            Value = 0f;
        }
    }

    [Header("Axis Names")]
    public const string HorizontalAxisName = "Horizontal";
    public const string VerticalAxisName = "Vertical";

    [Header("Gun Rotation Settings")]
    public float MaingunMinTurnX = -20f;
    public float MaingunMaxTurnX = 45f;

    [Header("References")]
    public GameObject gun;

    [Header("Motion Settings")]
    public float Speed = 20f;
    public float Friction = 3f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    public VAxis _horizontalAxis;
    public VAxis _verticalAxis;

    private bool _isTweaking = false;
    private int _lastDragFrame;
    //private Quaternion _initialRotation;

   // public PlayerHandler _playermove;

    private void Start()
    {

    }
    private void OnEnable()
    {
        _horizontalAxis = new VAxis(HorizontalAxisName);
        _verticalAxis = new VAxis(VerticalAxisName);

        // _initialRotation = Gun.transform.localRotation;
    }
    public bool IsTouching()
    {
        return _isTweaking;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _horizontalAxis.Value = eventData.delta.x;
        _verticalAxis.Value = eventData.delta.y;

        _lastDragFrame = Time.renderedFrameCount;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isTweaking = true;
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isTweaking = false;
        _horizontalAxis.Value = 0f;
        _verticalAxis.Value = 0f;
    }

    private void Aim(Vector2 aimVector)
    {

        rotationX += aimVector.y * Speed * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, MaingunMinTurnX, MaingunMaxTurnX);

        rotationY += aimVector.x * Speed * Time.deltaTime;

        //Gun = gun.transform.GetChild(0);
        gun.transform.localEulerAngles = new Vector3(
            0,// Vertical control
            rotationY, // Horizontal control
            0
        );
        gun.transform.GetChild(0).localEulerAngles = new Vector3(-rotationX, 0, 0);

    }

    private void Update()
    {
        if (_lastDragFrame < Time.renderedFrameCount - 2)
        {
            _horizontalAxis.Value = 0f;
            _verticalAxis.Value = 0f;
        }

        if (_isTweaking)
        {
            _horizontalAxis.Value = Mathf.Lerp(_horizontalAxis.Value, 0f, Friction * Time.deltaTime);
            _verticalAxis.Value = Mathf.Lerp(_verticalAxis.Value, 0f, Friction * Time.deltaTime);

            Vector2 aimVector = new Vector2(_horizontalAxis.Value, _verticalAxis.Value);
            Aim(aimVector);
            // print(_verticalAxis.Value);
            // _playermove.MovePlayer(gun);
        }
    }
}