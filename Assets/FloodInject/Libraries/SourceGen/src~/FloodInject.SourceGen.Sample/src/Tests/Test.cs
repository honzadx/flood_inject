using FloodInject.Runtime;
using UnityEngine;

#pragma warning disable CS0649
#pragma warning disable CS8618

namespace SourceGenerators.Sample.Tests;

public class Setting<T>
{
    public T value { get; set; }
}
public class DifficultySetting : Setting<int>;
public class VolumeSetting : Setting<float>;

[FloodStreamRequirement(typeof(DifficultySetting))]
[FloodStreamRequirement(typeof(VolumeSetting))]
public partial class SettingsStreamSO : AManagedStreamSO;

public class HealthSO : ScriptableObject
{
    [SerializeField] AnimationCurve _difficultyHealthCurve;
    public AnimationCurve difficultyHealthCurve => _difficultyHealthCurve;
}

[Flood]
public partial class HealthComponent : MonoBehaviour
{
    [FloodResolve(typeof(SettingsStreamSO))] DifficultySetting _difficultySetting;
    [SerializeField] HealthSO _healthSO;

    private float _maxHealth;
    private float _currentHealth;

    public void Init()
    {
        Construct();
        var health = _healthSO.difficultyHealthCurve.Evaluate(_difficultySetting.value);
        _maxHealth = health;
        _currentHealth = health;
    }
}

[Flood]
public partial class SettingsViewController : MonoBehaviour
{
    [FloodResolve(typeof(SettingsStreamSO))] DifficultySetting _difficultySetting;
    [FloodResolve(typeof(SettingsStreamSO))] VolumeSetting _volumeSetting;
}

#pragma warning restore CS0649
#pragma warning restore CS8618
