using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatSystem.CombatStates
{
    public class TimingCircle : MonoBehaviour
    {
        [SerializeField] private Image outCircle, miniCirle;
        [SerializeField] private TMP_Text inputText;

        private Vector3 initialScale;

        private void Awake()
        {
            initialScale = outCircle.rectTransform.localScale;
            gameObject.SetActive(false);
        }

        public void StartTiming(char targetKey, float maxTime)
        {
            gameObject.SetActive(true);
            inputText.text = targetKey.ToString().ToUpper();

            outCircle.rectTransform.DOScale(Vector3.one * .5f, maxTime)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    FloatingTextSpawner.Instance.ShowMessage("Timing Failed!", Color.red);
                }).SetAutoKill(true);
        }

        public void DoPunch()
        {
            transform.DOPunchScale(Vector3.one * 1.05f, 0.15f).SetAutoKill(true);
        }

        public void ResetCircle()
        {
            DOTween.Kill(outCircle.rectTransform);
            DOTween.Kill(outCircle);
            DOTween.Kill(miniCirle);
            outCircle.rectTransform.localScale = initialScale;
            inputText.text = string.Empty;
            outCircle.color = Color.white;
            miniCirle.color = Color.white;
            gameObject.SetActive(false);
        }

        public void SetCircleColor(Color color, bool isFade = true)
        {
            if (isFade)
            {
                outCircle.DOColor(color, 0.1f).SetEase(Ease.Linear);
                miniCirle.DOColor(color, 0.1f).SetEase(Ease.Linear);
            }
            else
            {
                outCircle.color = color;
                miniCirle.color = color;
            }
        }
    }
}