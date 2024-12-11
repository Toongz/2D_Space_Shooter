using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    public Vector3 startPosition; 
    public float repeatHeight;     
    public float moveSpeed = 2f;   

    private void Start()
    {
        startPosition = transform.position;


        repeatHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    private void Update()
    {

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

       
        if (transform.position.y > startPosition.y + repeatHeight)
        {
            transform.position = startPosition;
        }
    }
}
