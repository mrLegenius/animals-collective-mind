using Lions.Animals;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private Animal _animal;
    [SerializeField] private Image _energy;
    [SerializeField] private Image _thirst;
    [SerializeField] private Image _hunger;
    
    private void Update()
    {
        _energy.fillAmount = _animal.State.Energy.NormalizedValue;
        _thirst.fillAmount = 1 - _animal.State.Thirst.NormalizedValue;
        _hunger.fillAmount = 1 - _animal.State.Hunger.NormalizedValue;

        transform.rotation = Quaternion.LookRotation(Camera.main.transform.position - transform.position);
    }
}
