using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    public void UpdateHearts(int _hearts)
    {
        for (int i=0; i<_hearts; i++)
        {
            hearts[i].gameObject.SetActive(true);
        }
        for (int i=_hearts; i<hearts.Length; i++)
        {
            hearts[i].gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
