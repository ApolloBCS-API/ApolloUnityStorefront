using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ProductsArea: MonoBehaviour {
	private const string cartStr = " items ";
	public Sprite sprSuccessPurchase;
	public GameObject popupOutOfLives;
	public UILivesBalance balanceUI;

    private CATEGORY_TYPE currentCategory = CATEGORY_TYPE.ALL;
	private ITEM_TYPE currentType = ITEM_TYPE.ALL;

    

    // Use this for initialization
    void Start () {
        ReloadArea();
	}

    public void ReloadArea() {
		RemoveChildProduct();

        Sprite[] myIcons = Resources.LoadAll<Sprite>("Sprites/Icons");

        List<DigitalGoodModel> listDigital = DigitalGoodController.Instance.GetDigitalGoods(currentCategory,currentType);
		for (int i = 0; i < listDigital.Count; i++) {
			GameObject productNode = Instantiate(Resources.Load ("Prefabs/Items/Digital Good")) as GameObject;
			Transform transProduct = productNode.transform;
			transProduct.SetParent(transform);
			transProduct.localScale = Vector3.one;
			transProduct.Find("Description/TextName").GetComponent<Text>().text = listDigital[i].name;
			transProduct.Find("Description/TextDesc").GetComponent<Text>().text = listDigital[i].description; //descStr.Length > 100 ? descStr.Substring(0,100) + " ..." : descStr ;
                                                                                                              //transProduct.Find("ImgProduct").GetComponent<Image>().sprite = listIcons[0];

            foreach (var icon in myIcons)
            {
                if (icon.name == listDigital[i].sprName)
                {
                    transProduct.Find("ImgProduct").GetComponent<Image>().sprite = icon;
                    break;
                }
            }

            int livesValue = listDigital[i].lives;
			int id = listDigital[i].id;
			transProduct.Find("Description/Lives/Value").GetComponent<Text>().text = livesValue.ToString();//(node[i]["Stars"]);

			if(listDigital[i].isSale){
				transProduct.Find("ImgSale").gameObject.SetActive(true);
			}

			Button b = transProduct.Find("Description/Button Add To Cart").GetComponent<Button>();
			b.onClick.AddListener(() => StartCoroutine(PurchaseThisItem(id, transProduct, livesValue)));
		}
	}

	private void SortAreaByPrice(bool isLowFirst){
		RemoveChildProduct();
		List<DigitalGoodModel> listDigital = DigitalGoodController.Instance.GetDigitalGoods(currentCategory,currentType);
		if(isLowFirst){
			listDigital = listDigital.OrderBy(o=>o.lives).ToList();
		} else {
			listDigital = listDigital.OrderByDescending(o=>o.lives).ToList();
		}
		for (int i = 0; i < listDigital.Count; i++) {
			GameObject productNode = Instantiate(Resources.Load ("Prefabs/Items/Digital Good")) as GameObject;
			Transform transProduct = productNode.transform;
			transProduct.SetParent(transform);
			transProduct.localScale = Vector3.one;
			transProduct.Find("Description/TextName").GetComponent<Text>().text = listDigital[i].name;
			// string descStr = node[i]["Description"];
			transProduct.Find("Description/TextDesc").GetComponent<Text>().text = listDigital[i].description; //descStr.Length > 100 ? descStr.Substring(0,100) + " ..." : descStr ;
			int livesValue = listDigital[i].lives;
			int id = listDigital[i].id;
			transProduct.Find("Description/Lives/Value").GetComponent<Text>().text = livesValue.ToString();//(node[i]["Stars"]);
			
			Button b = transProduct.Find("Description/Button Add To Cart").GetComponent<Button>();
			b.onClick.AddListener(() => StartCoroutine(PurchaseThisItem(id,transProduct, livesValue)));
		}
	}

	private void SortByFree(){
		RemoveChildProduct();
		List<DigitalGoodModel> listDigital = DigitalGoodController.Instance.GetDigitalGoods(currentCategory,currentType);
		for (int i = 0; i < listDigital.Count; i++) {
			if(listDigital[i].lives == 0){
				GameObject productNode = Instantiate(Resources.Load ("Prefabs/Items/Digital Good")) as GameObject;
				Transform transProduct = productNode.transform;
				transProduct.SetParent(transform);
				transProduct.localScale = Vector3.one;
				transProduct.Find("Description/TextName").GetComponent<Text>().text = listDigital[i].name;
				// string descStr = node[i]["Description"];
				transProduct.Find("Description/TextDesc").GetComponent<Text>().text = listDigital[i].description; //descStr.Length > 100 ? descStr.Substring(0,100) + " ..." : descStr ;
				int livesValue = listDigital[i].lives;
				int id = listDigital[i].id;
				transProduct.Find("Description/Lives/Value").GetComponent<Text>().text = livesValue.ToString();//(node[i]["Stars"]);
				
				Button b = transProduct.Find("Description/Button Add To Cart").GetComponent<Button>();
				b.onClick.AddListener(() => StartCoroutine(PurchaseThisItem(id,transProduct, livesValue)));
			}
		}
	}

	public void SwitchCategory(CATEGORY_TYPE catType) {
		currentCategory = catType;
	}

	public void SwitchType(ITEM_TYPE itemType) {
		currentType = itemType;
	}

	private void RemoveChildProduct(){
		for (int i = 0; i < transform.childCount; i++) {
			Destroy(transform.GetChild(i).gameObject);
		}
	}

	public void SortProduct(Button pSender){
		switch(pSender.name){
		case "Button Best Selling":
			ReloadArea();
			break;
		case "Button Lower Price":
			SortAreaByPrice(true);
			break;
		case "Button Bigger Price":
			SortAreaByPrice(false);
			break;
		case "Button Free":
			SortByFree();
			break;
		default:break;
		}
	}

	IEnumerator PurchaseThisItem(int id,Transform tDigital,int livesValue) {
		if(DigitalGoodController.Instance.PurchaseDigitalGoods(id)){
			Sprite original = tDigital.Find("ImgProduct").GetComponent<Image>().sprite;
			tDigital.Find("ImgProduct").GetComponent<Image>().sprite = sprSuccessPurchase;
            balanceUI.UpdateTextBalance();
			yield return new WaitForSeconds(2.0f);
			tDigital.Find("ImgProduct").GetComponent<Image>().sprite = original;
		} else {
			popupOutOfLives.SetActive(true);
		}
	}
}
