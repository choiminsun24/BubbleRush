using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRange : MonoBehaviour
{
    public SpriteRenderer sprite;                   // 사거리 이미지
    public bool hit {get;set;} = true;              // 타워 설치 가능 구역 여부
    [SerializeField] private string category;       // 타워 종류


    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Tower")
        {
            hit = false;
            Debug.Log("hit = " + hit);
            // Red
            sprite.color = new Color(1f, 0f, 0f, sprite.color.a);
        }
        if (other.tag == category)
        {
            hit = false;
            Debug.Log("hit = " + hit);
            // Red
            sprite.color = new Color(1f, 0f, 0f, sprite.color.a);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {

        if(other.tag == category)
        {
            hit = true;
            // Gray
            sprite.color = new Color(0.5f, 0.5f, 0.5f, sprite.color.a);
        }

        if (other.tag == "Tower")
        {
            hit = true;
            // Gray
            sprite.color = new Color(0.5f, 0.5f, 0.5f, sprite.color.a);
        }

    }


    // 사거리 색표시
    public void DisplayRange()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0.5f);
    }
    // 사거리 투명화
    public void ClearRange()
    {
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0f);
    }
}
