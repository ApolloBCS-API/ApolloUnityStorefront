using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;

public class DigitalGoodController : MonoBehaviour {
	private static DigitalGoodController _instance;
	public static DigitalGoodController Instance {
		get{
			if(_instance == null) {
				GameObject dgManager = GameObject.Find ("Apollo_Controller") as GameObject;
				if(dgManager == null) {
					dgManager = GameObject.Instantiate(Resources.Load ("Prefabs/Apollo_Controller")) as GameObject;
				}
				_instance = dgManager.GetComponent<DigitalGoodController>();
	   		 }	
	   		 return _instance;
		}
	}
	
	private Dictionary<int,DigitalGoodModel> _listDigitalGood = new Dictionary<int,DigitalGoodModel>();
	
	void Awake () {
		string json = ((TextAsset)Resources.Load("Data/digitals")).text;
		JSONNode node = JSON.Parse(json);
		for(int i=0;i<node.Count;i++){
            for (int item = 0; item < node.Count; item++)
            {
                if (node[item]["Index"].AsInt == i)
                {
                    DigitalGoodModel dModel = new DigitalGoodModel();
                    dModel.id = node[item]["Id"].AsInt;
                    dModel.name = node[item]["ProductName"];
                    dModel.description = node[item]["Description"];
                    dModel.isSale = node[item]["IsSale"].AsBool;
                    dModel.sprName = node[item]["sprName"];
                    dModel.category = (CATEGORY_TYPE)node[item]["Category"].AsInt;
                    dModel.itemType = (ITEM_TYPE)node[item]["Type"].AsInt;
                    dModel.lives  = node[item]["Lives"].AsInt;
                    _listDigitalGood.Add(dModel.id, dModel);
                    break;
                }
            }
		}
	}
	
	public bool PurchaseDigitalGoods(int id){
		bool isSuccessPurchase = false;
		if(_listDigitalGood.ContainsKey(id)) {
			DigitalGoodModel dgModel = _listDigitalGood[id];
			isSuccessPurchase = FakeAccountController.Instance.DecreaseLives(dgModel.lives);
		}
		return isSuccessPurchase;
	}
	
	public List<DigitalGoodModel> GetDigitalGoods(CATEGORY_TYPE catType,ITEM_TYPE itemType) {
		List<DigitalGoodModel> listResult = new List<DigitalGoodModel>();
		foreach(KeyValuePair<int,DigitalGoodModel> model in _listDigitalGood){
			if(catType == CATEGORY_TYPE.ALL){
				if(itemType == ITEM_TYPE.ALL){
					listResult.Add(model.Value);
				} else {
					if(model.Value.itemType == itemType){
						listResult.Add (model.Value);
					}
				}
			} else if(model.Value.category == catType) {
				if(itemType == ITEM_TYPE.ALL) {
					listResult.Add (model.Value);
				} else if(model.Value.itemType == itemType) {
					listResult.Add(model.Value);
				}
			}
		}
		return listResult;
	}
	
	public List<ITEM_TYPE> GetTypeByCategory(CATEGORY_TYPE catType){
		List<ITEM_TYPE> listResult = new List<ITEM_TYPE>();
		switch(catType){
			case CATEGORY_TYPE.CONSUMABLE:
				listResult.Add(ITEM_TYPE.HEALTH);
				listResult.Add(ITEM_TYPE.MANA);
				listResult.Add(ITEM_TYPE.SORCERY);
				break;
			case CATEGORY_TYPE.UPGRADES:
				listResult.Add(ITEM_TYPE.WEAPONS);
				listResult.Add(ITEM_TYPE.ARMOR);
				listResult.Add(ITEM_TYPE.ACCESSORIES);
				break;
			default:break;
		}
		return listResult;
	}
}
