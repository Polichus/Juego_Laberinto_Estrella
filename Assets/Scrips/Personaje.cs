using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour
{
    public float speed = 3f;
    public Animation animator;
    public Sprite[] sprites; 
    public AnimationClip[] clip;
    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        foreach (AnimationClip clip in clip)
        {
            animator.AddClip(clip, clip.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.Translate(0, v,0 );
        transform.Translate(h, 0, 0);
        if (h > 0)
        {          
            spriteRenderer.sprite = sprites[0];
        }
        else if (h < 0)
        {
            // spriteRenderer.sprite = sprites[1];

            animator.Play(clip[1].name);
        }
        else if (v > 0)
        {
            spriteRenderer.sprite = sprites[2];
        }
        else if(v < 0)
        {
            spriteRenderer.sprite = sprites[3];
        }


    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "A")
        {
            Debug.Log("colisioma");
        }
    }
    
}
