using DG.Tweening;
using System;
using UnityEngine;

namespace RadicalGraphics.Core
{

    public interface IShoweable
    {
        void Show(bool value);
    }

    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    public class Showeable : MonoBehaviour , IShoweable
    {

        protected RectTransform m_rectTrans = null;
        protected CanvasGroup m_canvasGroup = null;

        private Vector3 m_originalPosition;

        public const float ANIM_DURATION = 0.3f;
        private const Ease ANIM_EASING_IN = Ease.OutCubic;
        private const Ease ANIM_EASING_OUT = Ease.InCubic;

        private Action m_showFinishedAction;

        public Action ShowFinishedAction
        {
            get { return m_showFinishedAction; }
            set { m_showFinishedAction = value; }
        }

        protected virtual void Awake()
        {
            m_rectTrans = (RectTransform)this.transform;
            m_originalPosition = m_rectTrans.position;
            m_canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(bool pActive)
        {
            m_canvasGroup.blocksRaycasts = pActive;

            if (pActive)
            {
                m_rectTrans.localScale = Vector3.zero;
                m_canvasGroup.alpha = 0f;

                m_rectTrans.DOScale(1f, ANIM_DURATION).SetEase(ANIM_EASING_IN).OnComplete(() => {
                    ShowFinishedAction?.Invoke();
                });

                m_canvasGroup.DOFade(1f, ANIM_DURATION);

            }
            else
            {
                m_rectTrans.DOScale(0f, ANIM_DURATION).SetEase(ANIM_EASING_OUT);
                m_canvasGroup.DOFade(0f, ANIM_DURATION).SetEase(ANIM_EASING_OUT);

            }
        }

        protected void SetTransform(RectTransform pTrans)
        {
            if (pTrans != null)
                m_rectTrans = pTrans;
        }
    }
}