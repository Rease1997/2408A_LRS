using System.Collections;
using System.Collections.Generic;
using W.GameFrameWork.ExcelTool;
/*
* Author:W
* Excel表转换生成
* SkillData
*/
namespace HotFix
{
	[System.Serializable]
	public class SkillDataParSer
	{
		public List<SkillData> data = new List<SkillData>();
		public List<SkillData> Data
		{
			get
			{
				return data;
			} 
		}	} 
	[System.Serializable]
	public class SkillData:ExcelItem
	{
	/// <summary>
	/// 物品ID
	/// <summary>
	public int ID;
	/// <summary>
	/// 名称
	/// <summary>
	public string Name;
	/// <summary>
	/// 标题
	/// <summary>
	public string Title;
	/// <summary>
	/// 商品描述
	/// <summary>
	public string Des;
	/// <summary>
	/// 物品类型
	/// <summary>
	public int Type;

	}
}