using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : MonoBehaviour
{
    public SpriteRenderer sprite;
    [SerializeField] private TowerController tc;
    public bool hit = false;                    // 타워 설치 가능 구역 여부
    [SerializeField] private string category;   // 타워 종류
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == category)
        {
            hit = true;
            // Gray
            sprite.color = new Color(0.5f, 0.5f, 0.5f, sprite.color.a);
        }
        else if(other.tag == "Player")
        {
            tc.DetectEnemies(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == category)
        {
            hit = false;
            // Red
            sprite.color = new Color(1f, 0f, 0f, sprite.color.a);
            print("hit=false");
        }
        else if(other.gameObject.tag == "Player")
        {
            tc.RemoveEnemies(other.gameObject);
        }
    }
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // 사거리 색표시
    public void DisplayRange()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }
    // 사거리 투명화
    public void ClearRange()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
    }
}
