﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Script para uno de los enmigos que hay dentro del juego, creando todo lo necesario para su
//funcionamiento y comportamiento dentro del juego y con relación al jugador y demas enemigos(herencia de Enemigos)
public class Mariposa : Enemigos
{
    SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        objetivo1 = Controlador.instance.objetivo1;
        objetivo2 = Controlador.instance.objetivo2;
        maxHealth = 3;
        currentHealth = maxHealth;
        shield = transform.GetChild(1).gameObject;

        agent = gameObject.GetComponent<NavMeshAgent>();
        animE = gameObject.GetComponentInChildren<Animator>();
        sr = (SpriteRenderer)gameObject.GetComponentInChildren(typeof(SpriteRenderer));
    }

    // Update is called once per frame
    void Update()
    {
        //base.MirarObjetivo(cam);
        #region Seguimiento del enemigo por el mapa

        Vector3 vectorMov1 = new Vector3(objetivo1.position.x - this.transform.position.x, objetivo1.position.y - this.transform.position.y, objetivo1.position.z - this.transform.position.z);
        Vector3 vectorMov2 = new Vector3(objetivo2.position.x - this.transform.position.x, objetivo2.position.y - this.transform.position.y, objetivo2.position.z - this.transform.position.z); ;

        float distancia1 = Vector3.Distance(objetivo1.position, transform.position);
        float distancia2 = Vector3.Distance(objetivo2.position, transform.position);

        if (distancia1 <= radioVision && distancia1 < distancia2)
        {
            mov = vectorMov1;
            agent.SetDestination(objetivo1.position);

        }

        if (distancia2 <= radioVision && distancia2 < distancia1)
        {
            mov = vectorMov2;
            agent.SetDestination(objetivo2.position);
        }
        #endregion    

        if (mov.x < 0) sr.flipX = true;
        else sr.flipX = false;

        #region Morirse
        if (currentHealth <= 0)
        {
            animE.SetTrigger("Die");
            this.GetComponent<Collider>().enabled = false;
            this.enabled = false;
        }
        #endregion
    }

    //Usamos una funcion que tarde mas en realizarse para dar tiempo a que pueda cambiar el estado del enemigo.
    private void LateUpdate()
    {
        if (damaged && currentHealth > 0)
        {
            //Animasao
            //animE.SetTrigger("TakeDmg");
            animE.SetBool("Damaged", damaged);
            agent.isStopped = true;
            Invoke("Damaged", 0.1f);
            Invoke("Stop", 1f);
        }
    }

    //cambio de estados durante el daño del enemigo
    private void Damaged()
    {
        
        damaged = false;
        animE.SetBool("Damaged", damaged);
    }

    private void Stop()
    {
        agent.isStopped = false;
    }

}