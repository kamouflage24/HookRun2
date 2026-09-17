using UnityEngine;

public class Fish : MonoBehaviour
{
    private GameObject Fish1Prefab;
    private GameObject Fish2Prefab;
    private GameObject Fish3Prefab;
    private GameObject Trout;
    private GameObject Tang;
    private GameObject Nemo;
    private GameObject FishBone;
    private GameObject MilkBone;
    
    public int fishDiff = 10;
    
    [SerializeField] private float _minWeight;
    [SerializeField] private float _maxWeight;
    
    [SerializeField] private float _minPrice;
    [SerializeField] private float _maxPrice;

    public float minWeight => this._minWeight;
    public float maxWeight => this._maxWeight;
    public float weight{get; private set;}
   
    private float minPrice => this._minPrice;
    public float maxPrice => this._maxPrice;
    public float price{get; private set;}

    private void Awake(){
        weight = Random.Range(_minWeight , _maxWeight);
        price = Random.Range(_minPrice , _maxPrice);
    }
}
