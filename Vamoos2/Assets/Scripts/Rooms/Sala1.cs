﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase herencia
/// Control de la primera sala, donde apareceran los jugadores, controlando si han completado
/// los objetivos o no.
/// </summary>
public class Sala1 : Salas

{
    public RoomController roomController;
    [SerializeField]
    GameObject luces;

    public GameObject[] doors;
    // Start is called before the first frame update
    void Start()
    {
        salaClean = true;
        salaCleanFirstTime = true;
        numPuertas = 2;
    }

    //Cuando los jugadores completen derrotar a los enemigos, podrán activar el evento de pasar de sala.
    void Update()
    {
        if (Controlador.instance.currentNumEnems == 0) salaClean = true;
        else salaClean = false;

        if (salaClean) sePuedePasar = true;
        else sePuedePasar = false;

        base.PuertasAbiertas(Controlador.instance.currentNumEnems);
        if (sePuedePasar && salaClean)
        {
            for (int i = 0; i < numPuertas; i++)
            {
                doors[i].GetComponent<Animator>().SetBool("SePuedePasar", true);
            }
        }
        else
        {
            for (int i = 0; i < numPuertas; i++)
            {
                doors[i].GetComponent<Animator>().SetBool("SePuedePasar", false);
            }
        }

    }
    //Al entrar a la sala, activar luces y mover la camara
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            luces.SetActive(true);

            Controlador.instance.cam = Controlador.instance.ptoscamara[0];
            Controlador.instance.dondeEstas = Controlador.DondeEstas.s1;
        }
    }

    //Al salir de la sala, apagar luces
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            luces.SetActive(false);
    }

    //Instanciar enemigos en sus posiciones



}
