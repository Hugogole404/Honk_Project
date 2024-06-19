using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class ShowBabyPosition : MonoBehaviour
{
    [SerializeField] private GameObject _baby;
    [SerializeField] private GameObject _player;
    [SerializeField] private RectTransform _arrowImage;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private bool _canBabyMustBeDetected;

    [SerializeField] private Camera _uiCamera;

    //private void Update()
    //{
    //    Vector3 toPosition = _baby.transform.position;
    //    Vector3 fromPosition = _mainCamera.transform.position;
    //    fromPosition.y = 0;
    //    Vector3 dir = (toPosition - fromPosition).normalized;
    //    float angle = UtilsClass.GetAngleFromVectorFloat(dir);
    //    _arrowImage.localEulerAngles = new Vector3(0, 0, angle);

    //    Vector3 targetPosScreenpoint = _mainCamera.WorldToScreenPoint(_baby.transform.position);
    //    bool isOffScreen = targetPosScreenpoint.x <= 0 || targetPosScreenpoint.y >= Screen.width || targetPosScreenpoint.y <= 0 || targetPosScreenpoint.y >= Screen.height;

    //    if (isOffScreen)
    //    {
    //        Vector3 cappedTargetPosScreen = targetPosScreenpoint;
    //        if (cappedTargetPosScreen.x <= 0) cappedTargetPosScreen.x = 0f;
    //        if (cappedTargetPosScreen.x >= Screen.width) cappedTargetPosScreen.x = Screen.width;
    //        if (cappedTargetPosScreen.y <= 0) cappedTargetPosScreen.y = 0f;
    //        if (cappedTargetPosScreen.y >= Screen.height) cappedTargetPosScreen.y = Screen.height;

    //        Vector3 pointerWorldPos = _uiCamera.ScreenToWorldPoint(cappedTargetPosScreen);
    //        _arrowImage.position = pointerWorldPos;
    //        _arrowImage.localPosition = new Vector3(_arrowImage.localPosition.x, _arrowImage.localPosition.y, 0f);
    //    }
    //}

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