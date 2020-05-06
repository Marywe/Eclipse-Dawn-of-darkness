﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Al igua que ColliderArma pero para el otro pj
public class ColliderArmaArrow : MonoBehaviour
{
    public LayerMask enemyLayer;
    Collider[] enemiesHit;
    [SerializeField]
    Animator anim;
    private bool puedeAtacar;

    private Vector3 posicion;
    public Vector3 cubeSz;
    float lastButTime;
    public float maxComboDelay = 0.9f;
    private int nBut;
    public float radius;
    private Azul a;


    public Image basic, skill;
    // Start is called before the first frame update
    void Start()
    {
        posicion.x =0.7f;
        posicion.y = 0;
        posicion.z = 0;
        transform.position = posicion;
        puedeAtacar = true;
        a = gameObject.GetComponentInParent<Azul>();
    }

    // Update is called once per frame
    void Update()
    {
        #region Skill Girar

        if(!a.skilling) skill.color = new Color(255, 255, 255);
        if (Input.GetButtonDown("HAB2") && a.playerState == Jugador.PlayerState.idle && !a.skilling)
        {
            skill.color = new Color(0, 0, 0);
            a.playerState = Jugador.PlayerState.skill;
            a.StartCoroutine(a.corrSkill());
            a.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Skill");
        }
        a.HabilidadGirar(enemiesHit, enemyLayer, radius);
        #endregion

        #region Dirección ataque
        float attackDirection = a.anim.GetFloat("Direction");
        if (attackDirection >= 0)
        {
            posicion.x = 0.7f;
            transform.localPosition = posicion;
        }
        else if (attackDirection < 0)
        {
            posicion.x = -0.7f;
            transform.localPosition = posicion;
        }
        #endregion

        if (puedeAtacar)
        {
            basic.color = new Color(255, 255, 255);
            BasicAttack();
        }
        else basic.color = new Color(0, 0, 0);
    }

    //Para poder ver los colliders
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(base.transform.position, cubeSz);
        Gizmos.DrawWireSphere(base.transform.position, radius);
    }

    #region Tipos de Ataque
    private void BasicAttack()
    {
        if (Time.time - lastButTime > maxComboDelay)
        {
            nBut = 0;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            lastButTime = Time.time;
            nBut++;
            anim.SetBool("Attack", true);

            if (nBut == 1)
            {
                a.speed -= 3;

                SetBasicAttack(0);

                enemiesHit = Physics.OverlapBox(transform.position, cubeSz / 2, Quaternion.identity, enemyLayer);
                //enemiesHit = Physics.OverlapSphere(this.transform.position, cubeSz, enemyLayer);
                foreach (Collider enemy in enemiesHit)
                {
                    enemy.GetComponent<Enemigos>().TakeDamage(a.dano);
                }

            }

            nBut = Mathf.Clamp(nBut, 0, 3);
        }

    }       

    public void FtAt()
    {
        if (nBut >= 2)
        {
            SetBasicAttack(0.5f);
            enemiesHit = Physics.OverlapBox(transform.position, cubeSz / 2, Quaternion.identity, enemyLayer);
            //enemiesHit = Physics.OverlapSphere(this.transform.position, cubeSz, enemyLayer);
            foreach (Collider enemy in enemiesHit)
            {
                enemy.GetComponent<Enemigos>().TakeDamage(a.dano);
            }

        }
        else
        {
            a.speed += 3;
            StartCoroutine(corBasicAtt());
            anim.SetBool("Attack", false);
            nBut = 0;
        }

    }
    public void SecAt()
    {
        if (nBut >= 3)
        {
            SetBasicAttack(1f);
            enemiesHit = Physics.OverlapBox(transform.position, cubeSz / 2, Quaternion.identity, enemyLayer);
            //enemiesHit = Physics.OverlapSphere(this.transform.position, cubeSz, enemyLayer);
            foreach (Collider enemy in enemiesHit)
            {
                enemy.GetComponent<Enemigos>().TakeDamage(a.dano);
            }

        }
        else
        {
            a.speed += 3;
            nBut = 0;
            anim.SetBool("Attack", false);
            StartCoroutine(corBasicAtt());
        }
    }
    public void ThAt()
    {
        Debug.Log("htt");
        anim.SetBool("Attack", false);
        StartCoroutine(corBasicAtt());
        nBut = 0;
        a.speed += 3;
    }
#endregion

    public void SetBasicAttack(float f)
    {
        anim.SetFloat("AttackN", f);
    }
    private IEnumerator corBasicAtt()
    {
        puedeAtacar = false;
        yield return new WaitForSeconds(a.cdbasicAttack);
        puedeAtacar = true;
    }
}
