using UnityEngine;
using UnityEngine.UI;

namespace Jurd.Utilities.SafeAreaLayout
{
    public class IgnoreSafeArea : MonoBehaviour, ILayoutIgnorer
    {
        public bool ignoreLayout => true;
    }
}
