using System;
using DG.Tweening;
using UnityEngine;

public class SceneTransitionController : MonoBehaviour
{
    [Header("Cloud References")] [SerializeField]
    private RectTransform leftCloud;

    [SerializeField] private RectTransform rightCloud;

    [Header("Transition Settings")] [SerializeField]
    private float transitionDuration = 1.5f;

    [SerializeField] private float waitInMiddle = 0.5f;

    [Header("Offsets")] [SerializeField] private float cloudOffset = 2100f;

    private static SceneTransitionController _instance;

    public static SceneTransitionController Instance => _instance;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);

        leftCloud.gameObject.SetActive(false);
        rightCloud.gameObject.SetActive(false);

        /*leftStartPos = leftCloud.position;
        rightStartPos = rightCloud.position;*/

        leftCloud.localPosition = new Vector3(-cloudOffset, 0, 0);
        rightCloud.localPosition = new Vector3(cloudOffset, 0, 0);
    }

    public void PlayTransition(Action onWaitInMiddle = null, Action onComplete = null)
    {
        leftCloud.gameObject.SetActive(true);
        rightCloud.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();

        sequence
            .Append(leftCloud.DOLocalMoveX(0, transitionDuration).SetEase(Ease.OutQuad))
            .Join(rightCloud.DOLocalMoveX(0, transitionDuration).SetEase(Ease.OutQuad))
            .AppendCallback(() => onWaitInMiddle?.Invoke())
            .AppendInterval(waitInMiddle)
            .Append(leftCloud.DOLocalMoveX(-cloudOffset, transitionDuration))
            .Join(rightCloud.DOLocalMoveX(cloudOffset, transitionDuration))
            .OnComplete(() => onComplete?.Invoke()).SetAutoKill();
    }
}