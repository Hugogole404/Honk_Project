using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    [SerializeField] private PlayerMovements _playerMovements;
    [SerializeField] private GameObject _spawnPoint;
    private bool _canBePlay;

    private void Start()
    {
        if (playableDirector != null)
        {
            _canBePlay = true;
            playableDirector.stopped += OnPlayableDirectorStopped;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Vérifie si l'objet entrant est le joueur, ou ajoute une condition spécifique ici
        if (_canBePlay)
        {
            if (other.GetComponent<PlayerMovements>() != null)
            {
                if (playableDirector != null)
                {
                    _playerMovements.CanPlayerUseInputs = false;
                    playableDirector.Play();
                    _canBePlay = false;
                }
                else
                {
                    Debug.LogError("PlayableDirector is not assigned!");
                }
            }
        }
    }
    private void OnDestroy()
    {
        if (playableDirector != null)
        {
            playableDirector.stopped -= OnPlayableDirectorStopped;
        }
    }
    void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (director == playableDirector)
        {
            _playerMovements.GetComponent<CharacterController>().enabled = false;
            _playerMovements.transform.position = _spawnPoint.transform.position;
            _playerMovements.GetComponent<CharacterController>().enabled = true;

            _playerMovements.CanPlayerUseInputs = true;
        }
    }
}