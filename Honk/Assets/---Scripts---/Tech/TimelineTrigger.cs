using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineTrigger : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public bool canSeePlayer;
    [SerializeField] private PlayerMovements _playerMovements;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private GameObject spawnPointTransform;
    [SerializeField] private GameObject PlayerMesh;
    private bool _canBePlay;
    private float _oldSpeed;

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
                    _canBePlay = false;
                    _oldSpeed = _playerMovements.BaseSpeed;

                    _playerMovements.BaseSpeed = 0;
                    _playerMovements.Direction = Vector3.zero;
                    _playerMovements.AnimatorHonk.SetBool("IsMoving", false);
                    if (canSeePlayer == false)
                    {
                        PlayerMesh.SetActive(false);
                    }

                    _playerMovements.CanPlayerUseInputs = false;
                    playableDirector.Play();
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
            PlayerMesh.SetActive(true);
            _spawnPoint.transform.position = spawnPointTransform.transform.position;
            _playerMovements.TakeBaby();
            _playerMovements.SpawnPoint = _spawnPoint;
            _playerMovements.TeleportToSpawnPoint();

            _playerMovements.BaseSpeed = _oldSpeed;

            
            _playerMovements.CanPlayerUseInputs = true;
        }
    }
}