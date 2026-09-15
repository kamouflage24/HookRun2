using UnityEngine;

public class Fish : MonoBehaviour
{
    public GameObject Fish1Prefab;
    public GameObject Fish2Prefab;
    public GameObject Fish3Prefab;
    public GameObject Trout;
    public GameObject Tang;
    public GameObject Nemo;
    public GameObject FishBone;
    public GameObject MilkBone;
    public GameObject Shark;
    public int fishDiff = 0;
    public int sharkDiff = 0;
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;
    [SerializeField] private float _amount;
    [SerializeField] private float _minPrice;
    [SerializeField] private float _maxPrice;

    public float minWeight => this._minWeight;
    public float maxWeight => this._maxWeight;
    public float amount => this._amount;
    private float minPrice => this._minPrice;
    public float maxPrice => this._maxPrice;

}
