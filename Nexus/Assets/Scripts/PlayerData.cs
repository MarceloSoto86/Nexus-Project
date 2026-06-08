using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class PlayerData
{
    public int memoryCurrentSlotsUnlocked; // Variable para almacenar la cantidad de slots de memoria desbloqueados por el jugador, lo que permite guardar y cargar el progreso del jugador en términos de su capacidad de memoria a lo largo del juego
    public float currentHealth; // Variable para almacenar la salud actual del jugador, lo que permite guardar y cargar el progreso del jugador en términos de su estado de salud a lo largo del juego
    public float xPosition; // Variable para almacenar la posición X del jugador, lo que permite guardar y cargar la ubicación del jugador en el mundo del juego a lo largo del progreso del jugador
    public float yPosition; // Variable para almacenar la posición Y del jugador, lo que permite guardar y cargar la ubicación del jugador en el mundo del juego a lo largo del progreso del jugador
    public float zPosition; // Variable para almacenar la posición Z del jugador, lo que permite guardar y cargar la ubicación del jugador en el mundo del juego a lo largo del progreso del jugador
    public List<string> collectedItemIDs = new List<string>(); // Variable para almacenar una lista de IDs de objetos recolectados por el jugador, lo que permite guardar y cargar el progreso del jugador en términos de los objetos que ha recolectado a lo largo del juego, lo que puede afectar la jugabilidad y la experiencia del jugador al interactuar con el mundo del juego y sus mecánicas relacionadas con la recolección de objetos
}
