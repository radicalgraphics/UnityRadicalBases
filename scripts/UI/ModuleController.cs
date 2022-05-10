using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace RadicalGraphics.Core
{
    public delegate void OnModuleClosed();

    public interface IModuleController
    {
        void OnClick(Button pButton);
        OnModuleClosed OnModuleClosedCallback { get; set; }
    }
    public class ModuleController : MonoBehaviour , IModuleController
    {
        private Showeable m_showeable = null;
        private OnModuleClosed m_onModuleClosed = null;
        private bool m_isClosing = false;

        protected Dictionary<string, Action> m_actions = new Dictionary<string, Action>();
        protected Action OnCloseAction;
        protected string m_sceneName = null;

        protected virtual void Awake()
        {
            m_showeable = GetComponent<Showeable>();
            RegisterActions();
        }

        public virtual void OnClick(Button button)
        {
            if (m_actions.ContainsKey(button.name))
                m_actions[button.name].Invoke();
        }

        protected virtual void RegisterActions()
        {

        }

        public virtual void Unload()
        {
            if (m_isClosing)
                return;

            m_isClosing = true;
            m_showeable.Show(false);

            Sequence _sequence = DOTween.Sequence();
            _sequence.AppendInterval(Showeable.ANIM_DURATION);

            _sequence.AppendCallback(() =>
            {
                if (_sequence.IsComplete())
                {
                    UnloadInternal();
                }

                OnCloseAction?.Invoke();
                OnCloseAction = null;
            });
        }


        private void UnloadInternal()
        {
            SceneManager.UnloadSceneAsync(m_sceneName).completed += (op) =>
            {
                m_isClosing = false;

                if (m_onModuleClosed != null)
                {
                    m_onModuleClosed();
                    m_onModuleClosed = null;
                }
            };
        }

        protected virtual void SetData()
        { }

        public OnModuleClosed OnModuleClosedCallback
        {
            get { return m_onModuleClosed; }
            set { m_onModuleClosed = value; }
        }
    }
}