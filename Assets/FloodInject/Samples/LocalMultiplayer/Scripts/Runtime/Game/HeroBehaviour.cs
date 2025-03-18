using FloodInject.Runtime;
using UnityEngine;

namespace LocalMultiplayer.Runtime
{
    [ContextListener(autoInjectType: AutoInjectType.Unity)]
    public partial class HeroBehaviour : MonoBehaviour
    {
        [Inject(typeof(Player1Context))] public HeroTemplateSO template;
        [Inject(typeof(Player1Context))] public InputControllerSO inputController;
    }
}