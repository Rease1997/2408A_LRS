using System.Collections;
using System.Collections.Generic;
using W.GameFrameWork.ExcelTool;
/*
* Author:W
* Excel表转换生成
* ShopData
*/
namespace HotFix
{
    [System.Serializable]
    public class ShopDataParSer
    {
        public List<ShopData> data = new List<ShopData>();
        public List<ShopData> Data
        {
            get
            {
                return data;
            }
        }
    }
    [System.Serializable]
    public class ShopData : ExcelItem
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
        /// 商品描述
        /// <summary>
        public string Des;
        /// <summary>
        /// 物品类型
        /// <summary>
        public int Type;
        /// <summary>
        /// 图集路径
        /// <summary>
        public string ImgPath;
        /// <summary>
        /// 图片名字
        /// <summary>
        public string ImgName;

    }
}