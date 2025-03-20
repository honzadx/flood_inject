using FloodInject.Runtime;
using UnityEngine;

namespace SourceGenerators.Sample.Tests;

[ContextListener]
public partial class Unit : MonoBehaviour
{
    [Inject(typeof(GameplayContext))] private UnitManager _unitManager;
}