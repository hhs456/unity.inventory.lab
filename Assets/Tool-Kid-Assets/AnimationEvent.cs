using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToolKid {
    [Serializable]
    public class AnimatorStateArg {
        [Label("�W�� Name")]
        public string name;
        [Label("���� Tag")]
        public string tag;
        [Label("���h Layer")]
        public int layer;
        //[Label("��ʵe�}�l On Animation Begin ()")]
        public UnityEvent onAnimationBegin;
        //[Label("��ʵe���� On Animation Begin ()")]
        public UnityEvent onAnimationEnd;
    }

    public class AnimatorEventArgs : EventArgs {

        public AnimatorStateArg stateArg;

        public AnimatorEventArgs(AnimatorStateArg stateArg) {
            this.stateArg = stateArg;
        }
    }

    [RequireComponent(typeof(Animator))]
    public class AnimationEvent : MonoBehaviour {
        private Animator animator;
        private AnimatorStateInfo animatorStateInfo;
        public AnimatorStateArg animatorStateArg;
        private bool isPlaying = false;
        public event EventHandler<AnimatorEventArgs> AnimationBegin;
        public event EventHandler<AnimatorEventArgs> AnimationEnd;

        private void Reset() {
            animator = GetComponent<Animator>();
        }
        private void Awake() {
            animator = GetComponent<Animator>();
        }

        private void Start() {
            animatorStateInfo = animator.GetCurrentAnimatorStateInfo(animatorStateArg.layer);
        }

        private void Update() {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(animatorStateArg.layer);
            if (animatorStateInfo.shortNameHash != info.shortNameHash) {
                if (info.IsName(animatorStateArg.name)) {
                    isPlaying = true;
                    animatorStateArg.onAnimationBegin.Invoke();
                    AnimationBegin?.Invoke(this, new AnimatorEventArgs(animatorStateArg));
                    Debug.Log(animatorStateArg.name + " begin play.");
                }
                else {
                    isPlaying = false;
                    animatorStateArg.onAnimationEnd.Invoke();
                    AnimationEnd?.Invoke(this, new AnimatorEventArgs(animatorStateArg));
                    Debug.Log(animatorStateArg.name + " end play.");
                }
                animatorStateInfo = info;
            }
            if (isPlaying) {
                if (info.normalizedTime >= 1f) {
                    isPlaying = false;
                    animatorStateArg.onAnimationEnd.Invoke();
                    AnimationEnd?.Invoke(this, new AnimatorEventArgs(animatorStateArg));
                    Debug.Log(animatorStateArg.name + " end play.");
                }
            }
        }
    }
}
