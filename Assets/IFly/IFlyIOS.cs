using UnityEngine;
using System.Collections;

namespace Wangz.IFly
{
    public class IFlyIOS : IFlyBase
    {
        public override void Init()
        {
        }

        public override void StartSpeech(int lengthSec)
        {
        }

        public override bool isListening()
        {
            return false;
        }

        public override void StopSpeech()
        {
        }

        public override void CancelSpeech()
        {
        }
    } 
}
