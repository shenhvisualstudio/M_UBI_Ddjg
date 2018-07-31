//
//文件名：    BillDetailE.aspx.cs
//功能描述：  电子提送有单数据集
//创建时间：  2015/09/24
//作者：      
//修改时间：  暂无
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace M_UBI_Ddjg.Bill
{
    public class BillDetailE
    {
        #region 公共属性

        /// <summary>
        /// 卡面号
        /// </summary>
        private string strExter_No;

        /// <summary>
        /// 车号
        /// </summary>
        private string vehicle;

        public string StrExter_No
        {
            get
            {
                return strExter_No;
            }

            set
            {
                strExter_No = value;
            }
        }

        public string Vehicle
        {
            get
            {
                return vehicle;
            }

            set
            {
                vehicle = value;
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化
        /// </summary>
        public BillDetailE() {

            strExter_No = string.Empty;
            vehicle = string.Empty;

        }
        #endregion

    }
}