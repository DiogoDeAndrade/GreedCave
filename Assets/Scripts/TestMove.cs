using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    Animator        anim;
    SpriteRenderer  spriteRenderer;
    Vector2         lastMoveDir = new Vector2(0.0f, -1.0f);
    Material        spriteMaterial;
    Coroutine       flashCR;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteMaterial = spriteRenderer.material;
    }

    void Update()
    {
        Vector2 mv = Vector2.zero;
        mv.x = Input.GetAxis("Horizontal");
        mv.y = Input.GetAxis("Vertical");

        if (mv.magnitude > 0.1f)
        {
            lastMoveDir = mv.normalized;
        }

        anim.SetFloat("DirX", lastMoveDir.x);
        anim.SetFloat("DirY", lastMoveDir.y);

        anim.SetFloat("Speed", mv.magnitude);

        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("Slash");
        }
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Thrust");
        }

        anim.SetBool("Shoot", Input.GetButton("Fire3"));

        if (Input.GetButtonDown("Jump"))
        {
            if (flashCR != null)
            {
                StopCoroutine(flashCR);
            }
            flashCR = StartCoroutine(FlashCR());
        }
    }

    IEnumerator FlashCR()
    {
        Color c = Color.red;

        while (c.a > 0.0f)
        {
            spriteMaterial.SetColor("_Color", c);

            c.a -= Time.deltaTime * 2.0f;

            yield return null;
        }

        c.a = 0;
        spriteMaterial.SetColor("_Color", c);

        flashCR = null;
    }
}
