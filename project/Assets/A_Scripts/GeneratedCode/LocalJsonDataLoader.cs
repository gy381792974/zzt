using System.IO;
using LitJson;
using UnityEngine;

//自动生成,不要在该文件里写任何代码
public class LocalJsonDataLoader
{
	private string filePath;
	private string jsonValue;
	private TextAsset textAsset;
	public void LoadInEditor()
	{
		//加载 Adorn_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Adorn_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Adorn_Data.DataArray = JsonMapper.ToObject<Adorn_Property[]>(jsonValue);
		Adorn_Data.SetAdornDataLenth();

		//加载 AppConst_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/AppConst_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		AppConst_Data.DataArray = JsonMapper.ToObject<AppConst_Property>(jsonValue);

		//加载 BuildArea_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/BuildArea_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		BuildArea_Data.DataArray = JsonMapper.ToObject<BuildArea_Property[]>(jsonValue);
		BuildArea_Data.SetBuildAreaDataLenth();

		//加载 BuildCommboInital_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/BuildCommboInital_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		BuildCommboInital_Data.DataArray = JsonMapper.ToObject<BuildCommboInital_Property[]>(jsonValue);
		BuildCommboInital_Data.SetBuildCommboInitalDataLenth();

		//加载 BuildInital_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/BuildInital_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		BuildInital_Data.DataArray = JsonMapper.ToObject<BuildInital_Property[]>(jsonValue);
		BuildInital_Data.SetBuildInitalDataLenth();

		//加载 BuildMaskArea_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/BuildMaskArea_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		BuildMaskArea_Data.DataArray = JsonMapper.ToObject<BuildMaskArea_Property[]>(jsonValue);
		BuildMaskArea_Data.SetBuildMaskAreaDataLenth();

		//加载 Chair_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Chair_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Chair_Data.DataArray = JsonMapper.ToObject<Chair_Property[]>(jsonValue);
		Chair_Data.SetChairDataLenth();

		//加载 CubeConfig_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CubeConfig_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CubeConfig_Data.DataArray = JsonMapper.ToObject<CubeConfig_Property[]>(jsonValue);
		CubeConfig_Data.SetCubeConfigDataLenth();

		//加载 CubeItem_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CubeItem_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CubeItem_Data.DataArray = JsonMapper.ToObject<CubeItem_Property[]>(jsonValue);
		CubeItem_Data.SetCubeItemDataLenth();

		//加载 Cube_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Cube_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Cube_Data.DataArray = JsonMapper.ToObject<Cube_Property[]>(jsonValue);
		Cube_Data.SetCubeDataLenth();

		//加载 CusFashion_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CusFashion_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CusFashion_Data.DataArray = JsonMapper.ToObject<CusFashion_Property[]>(jsonValue);
		CusFashion_Data.SetCusFashionDataLenth();

		//加载 CustomerNorBubble_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CustomerNorBubble_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CustomerNorBubble_Data.DataArray = JsonMapper.ToObject<CustomerNorBubble_Property[]>(jsonValue);
		CustomerNorBubble_Data.SetCustomerNorBubbleDataLenth();

		//加载 CustomerNormal_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CustomerNormal_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CustomerNormal_Data.DataArray = JsonMapper.ToObject<CustomerNormal_Property[]>(jsonValue);
		CustomerNormal_Data.SetCustomerNormalDataLenth();

		//加载 CustomerSpecial_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/CustomerSpecial_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		CustomerSpecial_Data.DataArray = JsonMapper.ToObject<CustomerSpecial_Property[]>(jsonValue);
		CustomerSpecial_Data.SetCustomerSpecialDataLenth();

		//加载 Equip_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Equip_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Equip_Data.DataArray = JsonMapper.ToObject<Equip_Property[]>(jsonValue);
		Equip_Data.SetEquipDataLenth();

		//加载 GuideStep_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/GuideStep_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		GuideStep_Data.DataArray = JsonMapper.ToObject<GuideStep_Property[]>(jsonValue);
		GuideStep_Data.SetGuideStepDataLenth();

		//加载 Item_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Item_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Item_Data.DataArray = JsonMapper.ToObject<Item_Property[]>(jsonValue);
		Item_Data.SetItemDataLenth();

		//加载 KitchenLevel_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/KitchenLevel_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		KitchenLevel_Data.DataArray = JsonMapper.ToObject<KitchenLevel_Property[]>(jsonValue);
		KitchenLevel_Data.SetKitchenLevelDataLenth();

		//加载 SplitArray_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/SplitArray_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		SplitArray_Data.DataArray = JsonMapper.ToObject<SplitArray_Property[]>(jsonValue);
		SplitArray_Data.SetSplitArrayDataLenth();

		//加载 Staff_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Staff_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Staff_Data.DataArray = JsonMapper.ToObject<Staff_Property[]>(jsonValue);
		Staff_Data.SetStaffDataLenth();

		//加载 Staff_Level_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Staff_Level_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Staff_Level_Data.DataArray = JsonMapper.ToObject<Staff_Level_Property[]>(jsonValue);
		Staff_Level_Data.SetStaff_LevelDataLenth();

		//加载 StallLevel_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/StallLevel_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		StallLevel_Data.DataArray = JsonMapper.ToObject<StallLevel_Property[]>(jsonValue);
		StallLevel_Data.SetStallLevelDataLenth();

		//加载 Stall_Data
		filePath =$"{AB_ResFilePath.jsonGameDatasRootDir}/Stall_Data.txt";
		jsonValue = File.ReadAllText(filePath);
		Stall_Data.DataArray = JsonMapper.ToObject<Stall_Property[]>(jsonValue);
		Stall_Data.SetStallDataLenth();

		jsonValue = null;
		filePath = null;
	}
	public void LoadInAssetBundle()
	{
		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("adorn_data","Adorn_Data");
		Adorn_Data.DataArray = JsonMapper.ToObject<Adorn_Property[]>(textAsset.text);
		Adorn_Data.SetAdornDataLenth();
		AssetMgr.Instance.UnloadAsset("adorn_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("appconst_data","AppConst_Data");
		AppConst_Data.DataArray = JsonMapper.ToObject<AppConst_Property>(textAsset.text);
		AssetMgr.Instance.UnloadAsset("appconst_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("buildarea_data","BuildArea_Data");
		BuildArea_Data.DataArray = JsonMapper.ToObject<BuildArea_Property[]>(textAsset.text);
		BuildArea_Data.SetBuildAreaDataLenth();
		AssetMgr.Instance.UnloadAsset("buildarea_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("buildcommboinital_data","BuildCommboInital_Data");
		BuildCommboInital_Data.DataArray = JsonMapper.ToObject<BuildCommboInital_Property[]>(textAsset.text);
		BuildCommboInital_Data.SetBuildCommboInitalDataLenth();
		AssetMgr.Instance.UnloadAsset("buildcommboinital_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("buildinital_data","BuildInital_Data");
		BuildInital_Data.DataArray = JsonMapper.ToObject<BuildInital_Property[]>(textAsset.text);
		BuildInital_Data.SetBuildInitalDataLenth();
		AssetMgr.Instance.UnloadAsset("buildinital_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("buildmaskarea_data","BuildMaskArea_Data");
		BuildMaskArea_Data.DataArray = JsonMapper.ToObject<BuildMaskArea_Property[]>(textAsset.text);
		BuildMaskArea_Data.SetBuildMaskAreaDataLenth();
		AssetMgr.Instance.UnloadAsset("buildmaskarea_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("chair_data","Chair_Data");
		Chair_Data.DataArray = JsonMapper.ToObject<Chair_Property[]>(textAsset.text);
		Chair_Data.SetChairDataLenth();
		AssetMgr.Instance.UnloadAsset("chair_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("cubeconfig_data","CubeConfig_Data");
		CubeConfig_Data.DataArray = JsonMapper.ToObject<CubeConfig_Property[]>(textAsset.text);
		CubeConfig_Data.SetCubeConfigDataLenth();
		AssetMgr.Instance.UnloadAsset("cubeconfig_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("cubeitem_data","CubeItem_Data");
		CubeItem_Data.DataArray = JsonMapper.ToObject<CubeItem_Property[]>(textAsset.text);
		CubeItem_Data.SetCubeItemDataLenth();
		AssetMgr.Instance.UnloadAsset("cubeitem_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("cube_data","Cube_Data");
		Cube_Data.DataArray = JsonMapper.ToObject<Cube_Property[]>(textAsset.text);
		Cube_Data.SetCubeDataLenth();
		AssetMgr.Instance.UnloadAsset("cube_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("cusfashion_data","CusFashion_Data");
		CusFashion_Data.DataArray = JsonMapper.ToObject<CusFashion_Property[]>(textAsset.text);
		CusFashion_Data.SetCusFashionDataLenth();
		AssetMgr.Instance.UnloadAsset("cusfashion_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("customernorbubble_data","CustomerNorBubble_Data");
		CustomerNorBubble_Data.DataArray = JsonMapper.ToObject<CustomerNorBubble_Property[]>(textAsset.text);
		CustomerNorBubble_Data.SetCustomerNorBubbleDataLenth();
		AssetMgr.Instance.UnloadAsset("customernorbubble_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("customernormal_data","CustomerNormal_Data");
		CustomerNormal_Data.DataArray = JsonMapper.ToObject<CustomerNormal_Property[]>(textAsset.text);
		CustomerNormal_Data.SetCustomerNormalDataLenth();
		AssetMgr.Instance.UnloadAsset("customernormal_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("customerspecial_data","CustomerSpecial_Data");
		CustomerSpecial_Data.DataArray = JsonMapper.ToObject<CustomerSpecial_Property[]>(textAsset.text);
		CustomerSpecial_Data.SetCustomerSpecialDataLenth();
		AssetMgr.Instance.UnloadAsset("customerspecial_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("equip_data","Equip_Data");
		Equip_Data.DataArray = JsonMapper.ToObject<Equip_Property[]>(textAsset.text);
		Equip_Data.SetEquipDataLenth();
		AssetMgr.Instance.UnloadAsset("equip_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("guidestep_data","GuideStep_Data");
		GuideStep_Data.DataArray = JsonMapper.ToObject<GuideStep_Property[]>(textAsset.text);
		GuideStep_Data.SetGuideStepDataLenth();
		AssetMgr.Instance.UnloadAsset("guidestep_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("item_data","Item_Data");
		Item_Data.DataArray = JsonMapper.ToObject<Item_Property[]>(textAsset.text);
		Item_Data.SetItemDataLenth();
		AssetMgr.Instance.UnloadAsset("item_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("kitchenlevel_data","KitchenLevel_Data");
		KitchenLevel_Data.DataArray = JsonMapper.ToObject<KitchenLevel_Property[]>(textAsset.text);
		KitchenLevel_Data.SetKitchenLevelDataLenth();
		AssetMgr.Instance.UnloadAsset("kitchenlevel_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("splitarray_data","SplitArray_Data");
		SplitArray_Data.DataArray = JsonMapper.ToObject<SplitArray_Property[]>(textAsset.text);
		SplitArray_Data.SetSplitArrayDataLenth();
		AssetMgr.Instance.UnloadAsset("splitarray_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("staff_data","Staff_Data");
		Staff_Data.DataArray = JsonMapper.ToObject<Staff_Property[]>(textAsset.text);
		Staff_Data.SetStaffDataLenth();
		AssetMgr.Instance.UnloadAsset("staff_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("staff_level_data","Staff_Level_Data");
		Staff_Level_Data.DataArray = JsonMapper.ToObject<Staff_Level_Property[]>(textAsset.text);
		Staff_Level_Data.SetStaff_LevelDataLenth();
		AssetMgr.Instance.UnloadAsset("staff_level_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("stalllevel_data","StallLevel_Data");
		StallLevel_Data.DataArray = JsonMapper.ToObject<StallLevel_Property[]>(textAsset.text);
		StallLevel_Data.SetStallLevelDataLenth();
		AssetMgr.Instance.UnloadAsset("stalllevel_data",true,true);

		textAsset = AssetMgr.Instance.LoadAsset<TextAsset>("stall_data","Stall_Data");
		Stall_Data.DataArray = JsonMapper.ToObject<Stall_Property[]>(textAsset.text);
		Stall_Data.SetStallDataLenth();
		AssetMgr.Instance.UnloadAsset("stall_data",true,true);
		textAsset = null;
	}
}
