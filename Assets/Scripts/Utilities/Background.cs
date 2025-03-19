using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCamera;
    public Transform[] backgrounds; 
    public float scrollSpeed = 2f;  

    private float length; 

    private void Start()
    {
        length = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;
    }

    private void Update()
    {
        // Di chuyển background xuống
        foreach (Transform bg in backgrounds)
        {
            bg.position += Vector3.down * scrollSpeed * Time.deltaTime;
        }

        
        foreach (Transform bg in backgrounds)
        {
            float camBottom = mainCamera.position.y - Camera.main.orthographicSize;
            if (bg.position.y + (length / 2) < camBottom) 
            {
                MoveToTop(bg);
            }
        }
    }

    void MoveToTop(Transform bg)
    {
      
        Transform highestBg = backgrounds[0];
        foreach (Transform b in backgrounds)
        {
            if (b.position.y > highestBg.position.y)
                highestBg = b;
        }

        bg.position = highestBg.position + Vector3.up * length;
    }
}
