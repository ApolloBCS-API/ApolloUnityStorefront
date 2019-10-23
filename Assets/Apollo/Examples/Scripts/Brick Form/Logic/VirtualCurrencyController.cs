using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class VirtualCurrencyController : MonoBehaviour {
	private static VirtualCurrencyController _instance;
	public static VirtualCurrencyController Instance {
		get{
			if(_instance == null) {
				GameObject vcManager = GameObject.Find ("Apollo_Controller") as GameObject;
				if(vcManager == null) {
					vcManager = GameObject.Instantiate(Resources.Load ("Prefabs/Apollo_Controller")) as GameObject;
				}
				_instance = vcManager.GetComponent<VirtualCurrencyController>();
	   		 }	
	   		 return _instance;
		}
	}
	
	private Dictionary<int,VirtualCurrencyModel> _listVirtualCurrency = new Dictionary<int,VirtualCurrencyModel>();
	
	void Start () {
		string json = ((TextAsset)Resources.Load("Data/virtual")).text;
		JSONNode node = JSON.Parse(json);
		for(int i=0;i<node.Count;i++) {
			VirtualCurrencyModel vcModel = new VirtualCurrencyModel();
			vcModel.id = node[i]["Id"].AsInt;
			vcModel.name = node[i]["LivesValue"];
			vcModel.livesValue = node[i]["LivesValue"].AsInt;
			vcModel.bonusValue = node[i]["BonusValue"].AsInt;
			vcModel.price = node[i]["Price"].AsDouble;
			vcModel.quantity = 1;
			_listVirtualCurrency.Add(vcModel.id,vcModel);
		}
	}
	
	public List<VirtualCurrencyModel> GetListVirtualCurrencyModel() {
		List<VirtualCurrencyModel> listResult = new List<VirtualCurrencyModel>();
		foreach (var item in _listVirtualCurrency) {
			listResult.Add(item.Value);
		}
		return listResult;
	}

	public VirtualCurrencyModel GetModelById(int id) {
		return _listVirtualCurrency[id];
	}
}
