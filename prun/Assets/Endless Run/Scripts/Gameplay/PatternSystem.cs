/// <summary>
/// Pattern system.
/// This script use for manage pattern
/// 
/// 
/// DO NOT EDIT THIS SCRIPT !!
/// 
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

[System.Serializable]
public class PatternSystem : MonoBehaviour {
	//Building
	public enum StateBuilding{
		Build_1, Build_2, Build_3, Build_4, Null
	}
	[System.Serializable]
	public class SetBuilding{
		
		public int[] stateBuilding_Left = new int[8];
		public int[] stateBuilding_Right = new int[8];
	}
	
	[System.Serializable]
	public class SetBuildingAmount{
		public int[] stateBuilding_Left = new int[4];
		public int[] stateBuilding_Right = new int[4];
	}
	
	//Item
	[System.Serializable]
	public class SetItem{
		public Vector2[] itemType_Left = new Vector2[31];
		public Vector2[] itemType_SubLeft = new Vector2[31];
		public Vector2[] itemType_Middle = new Vector2[31];
		public Vector2[] itemType_SubRight = new Vector2[31];
		public Vector2[] itemType_Right = new Vector2[31];
	}
	[System.Serializable]
	public class FloorItemSlot{
		public bool[] floor_Slot_Left, floor_Slot_SubLeft, floor_Slot_Middle, floor_Slot_SubRight,floor_Slot_Right;	
	}
	
	//Floor
	[System.Serializable]
	public class Floor{
		public bool[] floor_Slot_Left, floor_Slot_Right;	
	}
	
	[System.Serializable]
	public class QueueFloor{
		public Floor floorClass;
		public FloorItemSlot floorItemSlotClass;
		public GameObject floorObj;
		public List<Building> getBuilding = new List<Building>();
		public GameObject waterObj;
		public List<Item> getItem = new List<Item>();
	}
	
	[System.Serializable]
	public class LevelItem{
		[HideInInspector]
		public string level = "Pattern";
		public List<SetItem> patternItem = new List<SetItem>();	
	}
	
	[System.Serializable]
	public class Item_Type{
		public List<Item> itemList = new List<Item>();	
	}
	
	[System.Serializable]
	public class SetFloatItemType{
		public List<int> item = new List<int>();
	}
	
	[HideInInspector] public List<Vector3> defaultPosBuilding_Left = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosBuilding_Right = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosItem_Left = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosItem_SubLeft = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosItem_Middle = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosItem_SubRight = new List<Vector3>();

	[HideInInspector] public List<Vector3> defaultPosItem_Right = new List<Vector3>();
	
	//Prefab
	public List<GameObject> building_Pref = new List<GameObject>();
	public List<GameObject> item_Pref = new List<GameObject>(); 
	public GameObject spawnObj_Pref;
	public List<GameObject> floor_Pref = new List<GameObject>();
	public GameObject FinalFloor_Pref;
	public GameObject water_Pref;
	

	public List<SetBuilding> patternBuilding = new List<SetBuilding>();
	public List<SetItem> patternItem = new List<SetItem>();
	private float lastt = .0f;
	private float lastft = .0f;
	private int[] amountBuildingSpawn;
	private int[] amountItemSpawn;
	private int[] amountFloorSpawn;
	private int xxxFloorSpawn = 4;
	//private int amountWaterSpawn = 4;
	private float nextPosFloor = 32;
	public bool loadingComplete;
	public float loadingPercent;
	
	public static PatternSystem instance;
	
	//GameObject
	private List<GameObject> building_Obj = new List<GameObject>();
	private List<GameObject> item_Obj = new List<GameObject>();
	private List<GameObject> floor_Obj = new List<GameObject>();
	private List<GameObject> water_Obj = new List<GameObject>();

	
	//Building
	private List<Building> building_Script = new List<Building>();
	private int[] maxAmountBuilding;
	
	//Type Item
	private List<Item_Type> item_Type_Script = new List<Item_Type>();
	private List<int> amount_Item_Pattern_Left = new List<int>();
	private List<int> amount_Item_Pattern_SubLeft = new List<int>();
	private List<int> amount_Item_Pattern_Middle = new List<int>();
	private List<int> amount_Item_Pattern_SubRight = new List<int>();
	private List<int> amount_Item_Pattern_Right = new List<int>();
	
	
	private List<Floor> floor_Slot = new List<Floor>();
	private List<FloorItemSlot> floor_item_Slot = new List<FloorItemSlot>();
	public List<QueueFloor> queneFloor = new List<QueueFloor>();
	public List<QueueFloor> qFloor = null;

	public GameObject spawnObj_Obj;
	private ColliderSpawnCheck colSpawnCheck;
	
	//variable value in game
	private int randomPattern;
	private int randomItem;
	private Vector3 posFloorLast;
	
	//Defalut
	private Vector3 posStart = new Vector3(-100,-100,-100);
	private Vector3 angleLeft = new Vector3(0,180,0);
	private Vector3 angleRight = new Vector3(0,0,0);

	public PatternSystem(){
		SettingVariableFirst ();
	}

	void SettingVariableFirst(){
		if (defaultPosBuilding_Left.Count <= 0) {
			Vector3 pos = new Vector3(-3,0,12);
			for(int i = 0; i < 8; i++){
				defaultPosBuilding_Left.Add(new Vector3(pos.x,pos.y,pos.z-(i*4)));
			}
		}

		if(defaultPosBuilding_Right.Count <= 0){
			Vector3 pos = new Vector3(3,0,16);
			for(int i = 0; i < 8; i++){
				defaultPosBuilding_Right.Add(new Vector3(pos.x,pos.y,pos.z-(i*4)));
			}
		}

		if(defaultPosItem_Left.Count <= 0){
			Vector3 pos = new Vector3(-1.8f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Left.Add(new Vector3(pos.x,pos.y,pos.z-i));
			}
		}

		if(defaultPosItem_SubLeft.Count <= 0){
			Vector3 pos = new Vector3(-0.9f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_SubLeft.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}

		if(defaultPosItem_Middle.Count <= 0){
			Vector3 pos = new Vector3(0,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Middle.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}

		if(defaultPosItem_SubRight.Count <= 0){
			Vector3 pos = new Vector3(0.9f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_SubRight.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}

		if(defaultPosItem_Right.Count <= 0){
			Vector3 pos = new Vector3(1.8f,0,15);
			for(int i = 0; i < 31; i++){
				defaultPosItem_Right.Add(new Vector3(pos.x, pos.y, pos.z-i));
			}
		}

		if (patternBuilding.Count <= 0) {
			patternBuilding.Add(new SetBuilding());
		}

		if(patternItem.Count <= 0){
			patternItem.Add(new SetItem());
		}
	}

	void Start(){
		instance = this;
		StartCoroutine(CalAmountItem());
	}

	//void FixedUpdate() {
	//	float t = Time.time;
	//	Debug.Log ("t="+ (t-lastft));
	//	lastft = t;
	//}
	
	private List<SetFloatItemType> _itemType = new List<SetFloatItemType>();
	private SetFloatItemType itemTypeMax;
	IEnumerator CalAmountItem(){
		//25%
		ConvertPatternToItemTpyeSet();
		itemTypeMax = new SetFloatItemType();
		int i = 0;
		while(i < item_Pref.Count){
			itemTypeMax.item.Add(0);
			i++;
		}
		i = 0;
		loadingPercent = 1;
		while(i < _itemType.Count){
			int j = 0;
			while(j < _itemType[i].item.Count){
				if(_itemType[i].item[j] > itemTypeMax.item[j]){
					itemTypeMax.item[j] = _itemType[i].item[j];	
				}
				j++;
			}
			i++;
		}
		i = 0;
		loadingPercent = 3;
		amountItemSpawn = new int[itemTypeMax.item.Count];
		while(i < amountItemSpawn.Length){
			amountItemSpawn[i] = itemTypeMax.item[i] * xxxFloorSpawn;
			amountItemSpawn[i]++;
			i++;
		}
		yield return 0;
		loadingPercent = 5;
		StartCoroutine(CalAmountBuilding());
	}
	
	private void ConvertPatternToItemTpyeSet(){
		int i = 0;
		while(i < patternItem.Count){
			_itemType.Add(new SetFloatItemType());
			int j = 0;
			while(j < item_Pref.Count){
				_itemType[i].item.Add(0);
				j++;
			}
			i++;	
		}
		i = 0;
		while(i < patternItem.Count){
			int j = 0;
			//Left
			while(j < patternItem[i].itemType_Left.Length){
				int k = 0;
				while(k < item_Pref.Count){
					if(patternItem[i].itemType_Left[j].x == k+1){
						_itemType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			j = 0;
			//Middle
			while(j < patternItem[i].itemType_Middle.Length){
				int k = 0;
				while(k < item_Pref.Count){
					if(patternItem[i].itemType_Middle[j].x == k+1){
						_itemType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			j = 0;
			//Right
			while(j < patternItem[i].itemType_Right.Length){
				int k = 0;
				while(k < item_Pref.Count){
					if(patternItem[i].itemType_Right[j].x == k+1){
						_itemType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			i++;
		}
	}
	
	//Building 
	private List<SetFloatItemType> _buildingType = new List<SetFloatItemType>();
	private SetFloatItemType buildTypeMax;
	IEnumerator CalAmountBuilding(){
		//50%
		ConvertPatternToBuildingTpyeSet();
		buildTypeMax = new SetFloatItemType();
		int i = 0;
		while(i < building_Pref.Count){
			buildTypeMax.item.Add(0);
			i++;
		}
		i = 0;
		loadingPercent = 7;
		while(i < _buildingType.Count){
			int j = 0;
			while(j < _buildingType[i].item.Count){
				if(_buildingType[i].item[j] > buildTypeMax.item[j]){
					buildTypeMax.item[j] = _buildingType[i].item[j];	
				}
				j++;
			}
			i++;
		}
		i = 0;
		loadingPercent = 9;
		amountBuildingSpawn = new int[buildTypeMax.item.Count];
		while(i < amountBuildingSpawn.Length){
			amountBuildingSpawn[i] = buildTypeMax.item[i] * xxxFloorSpawn;
			amountBuildingSpawn[i]++;
			i++;
		}
		i = 0;
		int cnt = floor_Pref.Count;
		amountFloorSpawn = new int[cnt];
		while(i < amountFloorSpawn.Length){
			amountFloorSpawn[i] = 4;
			i++;
		}
		yield return 0;
		loadingPercent = 10;
		StartCoroutine(InitBuilding());
	}
	
	private void ConvertPatternToBuildingTpyeSet(){
		int i = 0;
		while(i < patternBuilding.Count){
			_buildingType.Add(new SetFloatItemType());
			int j = 0;
			while(j < building_Pref.Count){
				_buildingType[i].item.Add(0);
				j++;
			}
			i++;	
		}
		i = 0;
		while(i < patternBuilding.Count){
			int j = 0;
			//Left
			while(j < patternBuilding[i].stateBuilding_Left.Length){
				int k = 0;
				while(k < building_Pref.Count){
					if(patternBuilding[i].stateBuilding_Left[j] == k+1){
						_buildingType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			j = 0;
			//Right
			while(j < patternBuilding[i].stateBuilding_Right.Length){
				int k = 0;
				while(k < building_Pref.Count){
					if(patternBuilding[i].stateBuilding_Right[j] == k+1){
						_buildingType[i].item[k] += 1;
					}
					
					k++;
				}
				j++;
			}
			i++;
		}
	}
	
	IEnumerator InitBuilding(){
		//75%
		int i = 0;
		ItemManager.Instance.ClearAll ();
		GameObject builds = new GameObject ();
		builds.name = "Buildings";
		while(i < building_Pref.Count){
			int j = 0;
			while(j < amountBuildingSpawn[i]){
				if (building_Pref [i] != null) {
					GameObject go = (GameObject)Instantiate (building_Pref [i], posStart, Quaternion.identity);
					go.name = "Building[" + i + "][" + j + "]";
					go.transform.parent = builds.transform;
					building_Obj.Add (go);
					Building building = go.GetComponent<Building> ();
					building.buildIndex = i;
					building_Script.Add (building);
				}
				j++;
				yield return 0;
			}
			i++;
			yield return 0;
		}
		loadingPercent = 30;
		i = 0;
		GameObject items = new GameObject ();
		items.name = "Items";
		while(i < item_Pref.Count){
			int j = 0;
			item_Type_Script.Add(new Item_Type());
			amount_Item_Pattern_Left.Add(0);
			amount_Item_Pattern_SubLeft.Add(0);
			amount_Item_Pattern_Middle.Add(0);
			amount_Item_Pattern_SubRight.Add(0);
			amount_Item_Pattern_Right.Add(0);
			while(j < amountItemSpawn[i]){
				GameObject go = (GameObject)Instantiate(item_Pref[i], posStart, Quaternion.identity);
				go.name = "Item["+i+"]["+j+"]";
				go.transform.parent = items.transform;
				item_Obj.Add(go);
				ItemManager.Instance.Add(go);
				item_Type_Script[i].itemList.Add(go.GetComponent<Item>());
				item_Type_Script[i].itemList[j].itemID = i+1;
				j++;
				yield return 0;
			}
			i++;
			yield return 0;
		}
		i = 0;
		loadingPercent = 60;
		GameObject floors = new GameObject ();
		floors.name = "Floors";
		while (i < floor_Pref.Count) {
			int j = 0;
			floor_Obj.Clear ();
			floor_Slot.Clear ();
			floor_item_Slot.Clear ();
			//Debug.Log (amountFloorSpawn.Length);
			//GameObject go = (GameObject)Instantiate (floor_Pref[i], posStart, Quaternion.identity);
			//go.name = "Floor["+i+"]["+j+"]";
			//go.transform.parent = floors.transform;
			//Animator ani = go.GetComponentInChildren<Animator> ();
			//if (ani) {
			//	GameObject seaj = ani.gameObject;
			//	ItemManager.Instance.Add (seaj);
			//}
			//floor_Obj.Add (go);
			while (j < amountFloorSpawn[i]) {
				GameObject go = (GameObject)Instantiate (floor_Pref[i], posStart, Quaternion.identity);
				//go.name = "Floor["+i+"]["+j+"]";
				go.transform.parent = floors.transform;
				Animator ani = go.GetComponentInChildren<Animator> ();
				if (ani) {
					GameObject seaj = ani.gameObject;
					ItemManager.Instance.Add (seaj);
				}
				floor_Obj.Add (go);
				floor_Slot.Add (new Floor ());
				floor_Slot [j].floor_Slot_Left = new bool[defaultPosBuilding_Left.Count];
				floor_Slot [j].floor_Slot_Right = new bool[defaultPosBuilding_Right.Count];
				floor_item_Slot.Add (new FloorItemSlot ());
				floor_item_Slot [j].floor_Slot_Left = new bool[defaultPosItem_Left.Count];
				floor_item_Slot [j].floor_Slot_SubLeft = new bool[defaultPosItem_SubLeft.Count];
				floor_item_Slot [j].floor_Slot_Middle = new bool[defaultPosItem_Middle.Count];
				floor_item_Slot [j].floor_Slot_SubRight = new bool[defaultPosItem_SubRight.Count];
				floor_item_Slot [j].floor_Slot_Right = new bool[defaultPosItem_Right.Count];
				QueueFloor qfloor = new QueueFloor ();

				if(GameAttribute.Instance.isFinalLoop)
					qfloor.floorObj = Instantiate(FinalFloor_Pref) as GameObject;
				else
					qfloor.floorObj = floor_Obj [j];
				
				qfloor.floorClass = floor_Slot [j];
				qfloor.floorItemSlotClass = floor_item_Slot [j];
				queneFloor.Add (qfloor);
				j++;
				yield return 0;
			}
			i++;
			yield return 0;
		}
		Shuffle (queneFloor);

		i = 0;
		loadingPercent = 90;
		GameObject waters = new GameObject ();
		waters.name = "Waters";
		while(i < xxxFloorSpawn) {
			if (water_Pref != null) {
				GameObject go = (GameObject)Instantiate (water_Pref, posStart, Quaternion.identity);
				go.name = "Water[" + i + "]";
				go.transform.parent = waters.transform;
				water_Obj.Add (go);
			}
			i++;
			yield return 0;
		}

		loadingPercent = 100;
		spawnObj_Obj = (GameObject)Instantiate(spawnObj_Pref, posStart, Quaternion.identity);
		colSpawnCheck = spawnObj_Obj.GetComponentInChildren<ColliderSpawnCheck>();
		colSpawnCheck.headParent = spawnObj_Obj;
		//SoundManager.Instance.PlayBGM ("main", true);
		StartCoroutine(SetPosStarter());
	}

	public void Shuffle<T>(IList<T> list)  
	{
		//List<T> RandomItems = new List<T>();
		System.Random random = new System.Random();
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			//RandomItems.Add(list[random.Next(0, list.Count + 1)]);
			int k = random.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		} 
		//Debug.Log(list);
	}
	
	IEnumerator SetPosStarter(){
		//100%
		Vector3 pos = Vector3.zero;
		pos.z = nextPosFloor;
		int i = 0;
		//Debug.Log ("queneFloor.Count="+queneFloor.Count);
		//Debug.Log ("floor_Obj.Count=" + floor_Obj.Count);
		qFloor = queneFloor.GetRange(0, 4);
		queneFloor.RemoveRange (0, 4);
		//int a = 0;
		while(i < qFloor.Count){
			AddBuildingToFloor(qFloor[i]);
			qFloor [i].floorObj.transform.position = pos;
			if (water_Obj.Count > 0) {
				GameObject waterob = water_Obj [0];
				water_Obj.RemoveAt (0);
				AddWaterToFloor (qFloor [i], waterob, pos);
			}
			pos.z += nextPosFloor;
			i++;
			yield return 0;
		}
		posFloorLast = pos;
		pos = Vector3.zero;
		pos.z += nextPosFloor*2;
		colSpawnCheck.headParent.transform.position = pos;
		//Debug.Log ("colSpawnCheck.headParent.transform.position.z="+colSpawnCheck.headParent.transform.position.z);
		yield return new WaitForSeconds(0.5f);
		loadingComplete = true;
		StartCoroutine(WaitCheckFloor());
		//yield return 0;
	}
	
	IEnumerator WaitCheckFloor(){
		//float t = Time.time;
		//Debug.Log ("WaitCheckFloor() t="+ (t-lastft));
		//lastft = t;
		while(colSpawnCheck.isCollision == false){
			yield return 0;
		}
		//Debug.Log ("colSpawnCheck.isCollision="+colSpawnCheck.isCollision);
		colSpawnCheck.isCollision = false;
		StartCoroutine(SetPosFloor());
		yield return 0;

	}
	
	IEnumerator SetPosFloor(){
		//Debug.Log ("SetPosFloor()");
		Vector3 pos = Vector3.zero;
		pos.z = colSpawnCheck.headParent.transform.position.z;
		pos.z += nextPosFloor;
		float t = Time.time;
		//Debug.Log ("SetPosFloor("+pos.z+") "+ GameAttribute.Instance.Distance+" "+(t-lastt));
		lastt = t;
		//Debug.Log ("zzzz"+pos.z);
		colSpawnCheck.headParent.transform.position = pos;
		colSpawnCheck.nextPos = colSpawnCheck.headParent.transform.position.z;
		/*
		int i = 0;
		while(i < qFloor[0].floorClass.floor_Slot_Left.Length){
			qFloor[0].floorClass.floor_Slot_Left[i] = false;
			qFloor[0].floorClass.floor_Slot_Right[i] = false;
			i++;
			yield return 0;
		}
		i = 0;
		while(i < qFloor[0].floorItemSlotClass.floor_Slot_Left.Length){
			qFloor[0].floorItemSlotClass.floor_Slot_Left[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_SubLeft[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_Middle[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_SubRight[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_Right[i] = false;
			i++;
			yield return 0;
		}
		i = 0;
		int itemCount = qFloor[0].getItem.Count;
		while(i < itemCount){
			qFloor[0].getItem[0].itemActive = false;
			qFloor[0].getItem[0].transform.parent = null;
			qFloor[0].getItem[0].transform.position = posStart;
			ReturnItemWithType(qFloor[0].getItem[0]);
			qFloor[0].getItem.RemoveRange(0,1);
			i++;
			yield return 0;
		}
		i = 0;
		int buildingCount = qFloor[0].getBuilding.Count;
		while(i < buildingCount){
			qFloor[0].getBuilding[0].transform.parent = null;
			qFloor[0].getBuilding[0].transform.position = posStart;
			qFloor[0].getBuilding[0].buildingActive = false;
			qFloor[0].getBuilding.RemoveRange(0,1);
			i++;
			yield return 0;
		}
		*/
		if (qFloor [0].waterObj != null) {
			GameObject waterob = qFloor [0].waterObj;
			water_Obj.Add (waterob);
			qFloor [0].waterObj = null;
		}
		StartCoroutine(Cleaning());
		StartCoroutine(AddBuilding());
		yield return 0;
	}

	IEnumerator Cleaning(){
		int i = 0;
		while(i < qFloor[0].floorClass.floor_Slot_Left.Length){
			qFloor[0].floorClass.floor_Slot_Left[i] = false;
			qFloor[0].floorClass.floor_Slot_Right[i] = false;
			i++;
			yield return 0;
		}
		i = 0;
		while(i < qFloor[0].floorItemSlotClass.floor_Slot_Left.Length){
			qFloor[0].floorItemSlotClass.floor_Slot_Left[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_SubLeft[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_Middle[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_SubRight[i] = false;
			qFloor[0].floorItemSlotClass.floor_Slot_Right[i] = false;
			i++;
			yield return 0;
		}
		i = 0;
		int itemCount = qFloor[0].getItem.Count;
		while(i < itemCount){
			qFloor[0].getItem[0].itemActive = false;
			qFloor[0].getItem[0].transform.parent = null;
			qFloor[0].getItem[0].transform.position = posStart;
			ReturnItemWithType(qFloor[0].getItem[0]);
			qFloor[0].getItem.RemoveRange(0,1);
			i++;
			yield return 0;
		}
		i = 0;
		int buildingCount = qFloor[0].getBuilding.Count;
		while(i < buildingCount){
			qFloor[0].getBuilding[0].transform.parent = null;
			qFloor[0].getBuilding[0].transform.position = posStart;
			qFloor[0].getBuilding[0].buildingActive = false;
			qFloor[0].getBuilding.RemoveRange(0,1);
			i++;
			yield return 0;
		}
		if (qFloor [0].waterObj != null) {
			GameObject waterob = qFloor [0].waterObj;
			water_Obj.Add (waterob);
			qFloor [0].waterObj = null;
		}
	}
	
	IEnumerator AddBuilding(){
		QueueFloor qfloor = qFloor[0];
		qFloor.RemoveRange(0, 1);
		//Debug.Log ("AddBuilding("+qfloor.floorObj.name+")");
		queneFloor.Add (qfloor);

		qfloor = queneFloor [0];
		queneFloor.RemoveRange(0, 1);
		if(GameAttribute.Instance.isFinalLoop)
			qfloor.floorObj = Instantiate(FinalFloor_Pref) as GameObject;
			
		//Debug.Log ("qfloor="+qfloor.floorObj.name);
		
		int i = 0;
		randomPattern = Random.Range(0, patternBuilding.Count);
		randomItem = Random.Range(0, patternItem.Count);
		while(i < building_Script.Count){
			int j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Left.Length){
				CheckAddBuilding_Left(i, j, qfloor);
				j++;
			}
			j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Right.Length){
				CheckAddBuilding_Right(i, j, qfloor);
				j++;
			}
			i++;	
		}
		yield return 0;
		i = 0;
		CheckTypeItemFormAdd(qfloor, i);
		yield return 0;
		qfloor.floorObj.transform.position = posFloorLast;

		if (water_Obj.Count > 0) {
			GameObject waterob = water_Obj [0];
			water_Obj.RemoveAt (0);
			AddWaterToFloor (qfloor, waterob, posFloorLast);
		}
		//if(qfloor.waterObj)
		//	qfloor.waterObj.transform.position = posFloorLast;

		posFloorLast.z += nextPosFloor;
		qFloor.Add(qfloor);
		StartCoroutine(WaitCheckFloor());
		yield return 0;
	}
	
	public void Reseted(){
		Vector3 pos = Vector3.zero;
		nextPosFloor = 32;
		pos.z += nextPosFloor;
		colSpawnCheck.headParent.transform.position = pos;
		colSpawnCheck.nextPos = colSpawnCheck.headParent.transform.position.z;

		int y = 0;
		while(y < qFloor.Count){
			int i = 0;
			while(i < qFloor[y].floorClass.floor_Slot_Left.Length){
				qFloor[y].floorClass.floor_Slot_Left[i] = false;
				qFloor[y].floorClass.floor_Slot_Right[i] = false;
				i++;
			}
			i = 0;
			int itemCount = qFloor[y].getItem.Count;
			while(i < itemCount){
				qFloor[y].getItem[0].itemActive = false;
				qFloor[y].getItem[0].transform.parent = null;
				qFloor[y].getItem[0].transform.position = posStart;
				ReturnItemWithType(qFloor[y].getItem[0]);
				qFloor[y].getItem.RemoveRange(0,1);
				i++;
			}
			i = 0;
			int buildingCount = qFloor[y].getBuilding.Count;
			while(i < buildingCount){
				qFloor[y].getBuilding[0].transform.parent = null;
				qFloor[y].getBuilding[0].transform.position = posStart;
				qFloor[y].getBuilding[0].buildingActive = false;
				qFloor[y].getBuilding.RemoveRange(0,1);
				i++;
			}
			i = 0;
			while(i < qFloor[y].floorItemSlotClass.floor_Slot_Left.Length){
				qFloor[y].floorItemSlotClass.floor_Slot_Left[i] = false;
				qFloor[y].floorItemSlotClass.floor_Slot_SubLeft[i] = false;
				qFloor[y].floorItemSlotClass.floor_Slot_Middle[i] = false;
				qFloor[y].floorItemSlotClass.floor_Slot_SubRight[i] = false;
				qFloor[y].floorItemSlotClass.floor_Slot_Right[i] = false;
				i++;	
			}
			qFloor [y].floorObj.transform.position = new Vector3 (-100, -100, -100);

			//i = 0;
			if(qFloor[y].waterObj!=null){
				GameObject water = qFloor [y].waterObj;
				water.transform.position = new Vector3 (-100, -100, -100);
				water_Obj.Add (water);
				qFloor [y].waterObj = null;
				//i++;
			}
			y++;
		}
		//QueueFloor qfloor = qFloor.;
		//qFloor.RemoveRange(0, 1);
		//Debug.Log ("AddBuilding("+qfloor.floorObj.name+")");
		queneFloor.AddRange(qFloor);
		qFloor.Clear ();

		int x = 0;
		while (x < queneFloor.Count) {
			queneFloor[x].floorObj.transform.position = new Vector3 (-100, -100, -100);
			//if(queneFloor[x].waterObj!=null)
			//	queneFloor[x].waterObj.transform.position = new Vector3 (-100, -100, -100);
			x++;
		}
		posFloorLast.z = 32;
		StopAllCoroutines();
		StartCoroutine(SetPosStarter());
	}
	
	// Function Call
	#region 

	void AddWaterToFloor(QueueFloor floor, GameObject water, Vector3 pos) {
		//Debug.Log ("addWaterToFloor("+floor.floorObj.name + " "+ water.name+")");
		floor.waterObj = water;
		if(pos!=Vector3.zero)
			water.transform.position = pos;
	}

	void AddBuildingToFloor(QueueFloor floor){
		//Debug.Log ("AddBuildingToFloor("+floor.floorObj.name+")");
		int i = 0;
		randomPattern = Random.Range(0, patternBuilding.Count);
		randomItem = Random.Range(0, patternItem.Count);
		while(i < building_Script.Count){
			int j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Left.Length){
				CheckAddBuilding_Left(i,j,floor);
				j++;
			}
			j = 0;
			while(j < patternBuilding[randomPattern].stateBuilding_Right.Length){
				CheckAddBuilding_Right(i,j,floor);
				j++;
			
			}
			i++;	
		}
		i = 0;
		CheckTypeItemFormAdd(floor, i);
	}
	
	void ReturnItemWithType(Item _item){
		int i = 0;
		while(i < amountItemSpawn.Length){
			ReturnItem(_item, i+1);
			i++;
		}
		i = 0;
		while(i < amount_Item_Pattern_Right.Count){
			amount_Item_Pattern_Left[i] = 0;
			amount_Item_Pattern_SubLeft[i] = 0;
			amount_Item_Pattern_Middle[i] = 0;
			amount_Item_Pattern_SubRight[i] = 0;
			amount_Item_Pattern_Right[i] = 0;
			i++;
		}
	}
	
	void ReturnItem(Item _item, int itemID){
		if(_item.itemID == itemID){
			item_Type_Script[itemID-1].itemList.Add(_item);	
		}
	}
	
	void CheckTypeItemFormAdd(QueueFloor floor, int i){
		while(i < patternItem[randomItem].itemType_Left.Length){
			int j = 0;
			while(j < amount_Item_Pattern_Left.Count){
				if(patternItem[randomItem].itemType_Left[i].x == j+1){
					amount_Item_Pattern_Left[j] += 1;	
				}
				j++;
			}
			i++;
		}
		i = 0;

		while(i < patternItem[randomItem].itemType_SubLeft.Length){
			int j = 0;
			while(j < amount_Item_Pattern_SubLeft.Count){
				if(patternItem[randomItem].itemType_SubLeft[i].x == j+1){
					amount_Item_Pattern_SubLeft[j] += 1;	
				}
				j++;
			}
			i++;
		}
		i = 0;
		
		while(i < patternItem[randomItem].itemType_Middle.Length){
			int j = 0;
			while(j < amount_Item_Pattern_Middle.Count){
				if(patternItem[randomItem].itemType_Middle[i].x == j+1){
					amount_Item_Pattern_Middle[j] += 1;	
				}
				j++;
			}
			i++;
		}
		i = 0;

		while(i < patternItem[randomItem].itemType_SubRight.Length){
			int j = 0;
			while(j < amount_Item_Pattern_SubRight.Count){
				if(patternItem[randomItem].itemType_SubRight[i].x == j+1){
					amount_Item_Pattern_SubRight[j] += 1;	
				}
				j++;
			}
			i++;
		}
		i = 0;
		
		while(i < patternItem[randomItem].itemType_Right.Length){
			int j = 0;
			while(j < amount_Item_Pattern_Right.Count){
				if(patternItem[randomItem].itemType_Right[i].x == j+1){
					amount_Item_Pattern_Right[j] += 1;	
				}
				j++;
			}
			i++;
		}
		i = 0;
		
		//Add Item To Floor Left	
		while(i < patternItem[randomItem].itemType_Left.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Left(floor, i, s+1);
				s++;
			}
			i++;
		}
		i = 0;

		while(i < patternItem[randomItem].itemType_SubLeft.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_SubLeft(floor, i, s+1);
				s++;
			}
			i++;
		}
		i = 0;
		
		//Add Item To Floor Middle
		while(i < patternItem[randomItem].itemType_Middle.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Middle(floor, i, s+1);
				s++;
			}
			i++;
		}
		i = 0;

		while(i < patternItem[randomItem].itemType_SubRight.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_SubRight(floor, i, s+1);
				s++;
			}
			i++;
		}
		i = 0;
		
		//Add Item To Floor Right
		while(i < patternItem[randomItem].itemType_Right.Length){
			int s = 0;
			while(s < amountItemSpawn.Length){
				AddItemWihtType_Right(floor, i, s+1);
				s++;
			}
			i++;
		}
		i = 0;
	}
	
	void AddItemWihtType_Left(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Left[slotIndex].x == type){
			int j = 0;
			while(j < amount_Item_Pattern_Left[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false
					   && floor.floorItemSlotClass.floor_Slot_Left[slotIndex] == false){
						SetPosItem_Left_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Left[slotIndex].y);
						j = 0;
					}
				}
				
				j++;
			}
		}	
	}

	void AddItemWihtType_SubLeft(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_SubLeft[slotIndex].x == type){
			int j = 0;
			while(j < amount_Item_Pattern_SubLeft[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false
					   && floor.floorItemSlotClass.floor_Slot_SubLeft[slotIndex] == false){
						SetPosItem_SubLeft_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_SubLeft[slotIndex].y);
						j = 0;
					}
				}
				
				j++;
			}
		}	
	}
	
	void AddItemWihtType_Middle(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Middle[slotIndex].x == type){
			int j = 0;
			while(j < amount_Item_Pattern_Middle[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false
					   && floor.floorItemSlotClass.floor_Slot_Middle[slotIndex] == false){
						SetPosItem_Middle_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Middle[slotIndex].y);
						j = 0;
					}
				}
				
				j++;
			}
		}	
	}

	void AddItemWihtType_SubRight(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_SubRight[slotIndex].x == type){
			int j = 0;
			while(j < amount_Item_Pattern_SubRight[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false
					   && floor.floorItemSlotClass.floor_Slot_SubRight[slotIndex] == false){
						SetPosItem_SubRight_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_SubRight[slotIndex].y);
						j = 0;
					}
				}
				j++;
			}
		}	
	}
	
	void AddItemWihtType_Right(QueueFloor floor, int slotIndex,int type){
		if(patternItem[randomItem].itemType_Right[slotIndex].x == type){
			int j = 0;
			while(j < amount_Item_Pattern_Right[type-1]){
				if(j < item_Type_Script[type-1].itemList.Count){
					if(item_Type_Script[type-1].itemList[j].itemActive == false
					   && floor.floorItemSlotClass.floor_Slot_Right[slotIndex] == false){
						SetPosItem_Right_For_Type(slotIndex,type-1,j,floor, patternItem[randomItem].itemType_Right[slotIndex].y);
						j = 0;
					}
				}
				j++;
			}
		}	
	}
	
	void CheckAddBuilding_Left(int i, int j, QueueFloor floor){
		int index = 0;
		
		while(index < building_Pref.Count){
			if(patternBuilding[randomPattern].stateBuilding_Left[j] == index+1 && floor.floorClass.floor_Slot_Left[j] == false){
				if(building_Script[i].buildingActive == false && building_Script[i].buildIndex == index){
					SetPosBuilding_Left(i,j,floor);
					index = building_Pref.Count;
				}
			}
			index++;
		}
	}
	
	void CheckAddBuilding_Right(int i, int j, QueueFloor floor){
		//Debug.Log ("CheckAddBuilding_Right("+floor.floorObj.name+")");
		int index = 0;
		
		while(index < building_Pref.Count){
			//Debug.Log ("zzzzzzzzzzzzzzz");
			//Debug.Log("xxxxxxxxxxxxx "+patternBuilding[randomPattern].stateBuilding_Right[j]);
			//Debug.Log("yyyyyyyyyyyyy "+floor.floorClass.floor_Slot_Right[j]);
			if(patternBuilding[randomPattern].stateBuilding_Right[j] == index+1 && floor.floorClass.floor_Slot_Right[j] == false){
				//Debug.Log ("aaaaaaa "+building_Script[i].buildingActive+" xxx "+ building_Script[i].buildIndex);
				if (building_Script [i].buildingActive == false && building_Script [i].buildIndex == index) {
					//Debug.Log ("bbbbbbbb " + floor.floorObj.name);
					SetPosBuilding_Right (i, j, floor);
					index = building_Pref.Count;
				}
				//else {
					//Debug.Log ("fffffff"+building_Script [i].buildingActive);
					//Debug.Log ("zzzzz"+building_Script [i].buildIndex);
				//}
			}
			index++;
		}
	
	}
	
	void SetPosBuilding_Left(int i, int j, QueueFloor floor){
		building_Script[i].transform.parent = floor.floorObj.transform;
		building_Script[i].transform.localPosition = defaultPosBuilding_Left[j];
		building_Script[i].transform.eulerAngles = angleLeft;
		building_Script[i].buildingActive = true;
		floor.floorClass.floor_Slot_Left[j] = true;
		floor.getBuilding.Add(building_Script[i]);
	}
	
	void SetPosBuilding_Right(int i, int j, QueueFloor floor){
		//Debug.Log ("setPostRight("+ i +" "+j +" "+ floor.floorObj.name+")");
		building_Script[i].transform.parent = floor.floorObj.transform;
		building_Script[i].transform.localPosition = defaultPosBuilding_Right[j];
		building_Script[i].transform.eulerAngles = angleRight;
		building_Script[i].buildingActive = true;
		floor.floorClass.floor_Slot_Right[j] = true;
		floor.getBuilding.Add(building_Script[i]);
	}
	
	void SetPosItem_Left_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_Left[i].x, defaultPosItem_Left[i].y + height, defaultPosItem_Left[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Left[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}

	void SetPosItem_SubLeft_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_SubLeft[i].x, defaultPosItem_SubLeft[i].y + height, defaultPosItem_SubLeft[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_SubLeft[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}
	
	void SetPosItem_Middle_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3(defaultPosItem_Middle[i].x, defaultPosItem_Middle[i].y + height, defaultPosItem_Middle[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Middle[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}

	void SetPosItem_SubRight_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3( defaultPosItem_SubRight[i].x, defaultPosItem_SubRight[i].y + height, defaultPosItem_SubRight[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_SubRight[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}
	
	void SetPosItem_Right_For_Type(int i, int j, int countItem, QueueFloor floor, float height){
		item_Type_Script[j].itemList[countItem].transform.parent = floor.floorObj.transform;
		item_Type_Script[j].itemList[countItem].transform.localPosition = new Vector3( defaultPosItem_Right[i].x, defaultPosItem_Right[i].y + height, defaultPosItem_Right[i].z);
		item_Type_Script[j].itemList[countItem].itemActive = true;
		floor.floorItemSlotClass.floor_Slot_Right[i] = true;
		floor.getItem.Add(item_Type_Script[j].itemList[countItem]);
		
		item_Type_Script[j].itemList.RemoveRange(countItem,1);
	}
	
	
	#endregion
}	
