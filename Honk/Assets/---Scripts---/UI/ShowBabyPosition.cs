using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class ShowBabyPosition : MonoBehaviour
{
    //[SerializeField] private GameObject _baby;
    //[SerializeField] private Vector3 _targetPosition;
    //[SerializeField] private GameObject _player;
    //[SerializeField] private RectTransform _arrowImage;
    //[SerializeField] private Camera _mainCamera;
    //[SerializeField] private bool _canBabyMustBeDetected;

    //[SerializeField] private Camera _uiCamera;

    public Transform _target; // L'objet que la flèche doit pointer
    public RectTransform _arrow; // L'UI Image de la flèche
    public Camera _mainCamera; // La caméra principale de la scène
    public float _distanceThreshold = 2f; // Distance à partir de laquelle l'arrow disparaît
    public Transform _start; // Point de départ

    void Update()
    {
        if (_target == null || _start == null)
            return;

        // Calculer la direction entre le point de départ et la cible
        Vector3 directionToTarget = _target.position - _start.position;

        // Si la cible est suffisamment proche, cacher la flèche
        if (directionToTarget.magnitude < _distanceThreshold)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }

        // Calculer la position sur l'écran de la cible
        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(_target.position);

        // Vérifier si l'objectif est sur l'écran
        if (viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1 && viewportPosition.z >= 0)
        {
            _arrow.gameObject.SetActive(false);
            return;
        }

        _arrow.gameObject.SetActive(true);

        // Calculer la position sur l'écran de la cible
        Vector3 screenPosition = _mainCamera.WorldToScreenPoint(_target.position);

        // Inverser la direction si la cible est derrière
        if (screenPosition.z < 0)
        {
            screenPosition *= -1;
            screenPosition.z = Mathf.Abs(screenPosition.z);
        }

        // Calculer l'angle de la flèche
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector2 direction = (Vector2)(screenPosition - screenCenter);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        _arrow.rotation = Quaternion.Euler(0, 0, angle);

        // Limiter la position de la flèche à l'intérieur de l'écran
        Vector3 cappedScreenPosition = screenPosition;
        cappedScreenPosition.x = Mathf.Clamp(cappedScreenPosition.x, _arrow.rect.width / 2, Screen.width - _arrow.rect.width / 2);
        cappedScreenPosition.y = Mathf.Clamp(cappedScreenPosition.y, _arrow.rect.height / 2, Screen.height - _arrow.rect.height / 2);

        _arrow.position = cappedScreenPosition;
    }

    //void Update()
    //{
    //    if (_canBabyMustBeDetected)
    //    {
    //        if (_baby == null || _player == null) return;

    //        Vector3 screenPoint = _mainCamera.WorldToViewportPoint(_baby.transform.position);
    //        bool isOutOfScreen = screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;

    //        if (isOutOfScreen)
    //        {
    //            _arrowImage.enabled = true;

    //            Vector3 direction = new Vector3(_baby.transform.position.x - _player.transform.position.x, 0, _baby.transform.position.z - _player.transform.position.z);

    //            float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

    //            _arrowImage.transform.rotation = Quaternion.Euler(0, 0, angle);
    //        }
    //        else
    //        {
    //            _arrowImage.enabled = false;
    //        }
    //    }
    //    else
    //    {
    //        _arrowImage.enabled = false;
    //    }
    //}
}