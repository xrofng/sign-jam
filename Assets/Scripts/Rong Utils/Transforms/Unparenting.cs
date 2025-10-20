using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GolfBTW
{
    public class Unparenting : MonoBehaviour
    {
        public enum StarFlow
        {
            Start,
            Awake,
            Enable
        }
        public StarFlow UnparentTime = StarFlow.Start;
        public bool UnparentCompletely = true;
        public Transform UnparentFrom;

        private void OnEnable()
        {
            if (UnparentTime == StarFlow.Enable)
            {
                DoUnparent();
            }
        }

        private void Start()
        {
            if (UnparentTime == StarFlow.Start)
            {
                DoUnparent();
            }
        }
        private void Awake()
        {
            if (UnparentTime == StarFlow.Awake)
            {
                DoUnparent();
            }
        }

        public void DoUnparent()
        {
            transform.parent = UnparentFrom != null ? UnparentFrom.parent : null;
        }
    }

}
