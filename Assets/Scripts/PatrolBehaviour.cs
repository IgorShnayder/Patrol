using UnityEngine;

public class PatrolBehaviour : MonoBehaviour
{
    [SerializeField] 
    private float _unitSpeed;

    [SerializeField] 
    private float _delayNextMove;

    [SerializeField]
    private Transform[] _patrolDots;
    
    private float _currentTime;
    private int _patrolDotIndex;
    private float _waitTime;
    private Animator _animator;
    private float _animationSpeed;

    private void Awake()
    {
        _waitTime = _delayNextMove;
        _animator = GetComponent<Animator>();
        _animationSpeed = _animator.speed;
    }

    private void Update()
    {
        if (_waitTime < _delayNextMove)
        {
            _animator.speed = 0;
            _waitTime += Time.deltaTime;
            return;
        }

        if (IsLastDot())
        {
            MovingFromDotToDot(_patrolDotIndex, 0);
            return;
        }
        
        MovingFromDotToDot(_patrolDotIndex,_patrolDotIndex + 1);
    }

    private void MovingFromDotToDot(int startDotIndex, int nextDotIndex)
    {
        _currentTime += Time.deltaTime;
        
        var distanceBetweenTwoDots = Vector3.Distance(_patrolDots[startDotIndex].position, _patrolDots[nextDotIndex].position);
        var travelTimeBetweenTwoDots = distanceBetweenTwoDots / _unitSpeed;
        var progress = _currentTime / travelTimeBetweenTwoDots;
        _animator.speed = _animationSpeed;
        var newPosition = Vector3.Lerp(_patrolDots[startDotIndex].position, _patrolDots[nextDotIndex].position, progress);
        
        transform.LookAt(_patrolDots[nextDotIndex]);
        transform.position = newPosition;
       
        
        if (IsLastDot() && progress >= 1)
        {
            _patrolDotIndex = 0;
           MakeTimeZeroValue();
        }
        
        else if (progress >= 1)
        {
            _patrolDotIndex++;
            MakeTimeZeroValue();
        }
    }

    private bool IsLastDot()
    {
        return _patrolDotIndex == _patrolDots.Length - 1;
    }

    private void MakeTimeZeroValue()
    {
        _currentTime = 0;
        _waitTime = 0;
    }
}
