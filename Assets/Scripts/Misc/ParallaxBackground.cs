using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;

    [SerializeField]
    private float delta;
    
    private float _length, _startX;

    private void Start()
    {
        _startX = transform.position.x;
        _length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        var temp = cam.transform.position.x * (1 - delta);
        var dist = cam.transform.position.x * delta;

        transform.position = new Vector3(_startX + dist, transform.position.y, transform.position.z);

        if (temp > _startX + _length)
        {
            _startX += _length;
        }
        else if (temp < _startX - _length)
        {
            _startX -= _length;
        }
    }
}
