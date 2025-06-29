using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmCanvasUI : MonoBehaviour
{
    [SerializeField] private GameObject m_farmUIObjet;
    [SerializeField] private GameObject m_interactionUIObjectd;

    private void Start()
    {
        Tile.FarmUIRef = m_farmUIObjet;
        Tile.InteractUiTextRef = m_interactionUIObjectd;
    }
}