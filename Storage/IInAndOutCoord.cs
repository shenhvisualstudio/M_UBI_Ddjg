//
//文件名：    IInAndOutCoord.cs
//功能描述：  进出点经、纬度坐标接口类
//创建时间：  2016/03/22
//作者：      
//修改时间：  
//修改描述：  暂无
//
using Leo;
using Newtonsoft.Json;
using ServiceInterface.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


namespace M_UBI_Ddjg.Storage
{
    public class IInAndOutCoord
    {
        //Iport公司编码
        private string[] strCodeCompanyArray = 
        { 
            "010246",    //新苏港"405"    
            "010219",    //新路桥"191"
        };

        #region 获取公司编码
        public string[] GetCodeCompany()
        {
            return strCodeCompanyArray;
        }
        #endregion

        #region 获取各公司进出点经、纬度坐标
        /// <summary>
        /// 获取各公司进出点经、纬度坐标
        /// </summary>
        /// <param name="strCodeCompany">公司编码</param>
        /// <returns>进出点经、纬度坐标</returns>
        public DataTable GetInAndOutCoord(string strCodeCompany)
        {
            string strSql =
                string.Format(@"select in_long,in_latitude,out_long,out_latitude,code_storage 
                                from TB_CODE_STORAGE 
                                where code_department='{0}' and isusing='1'",
                                strCodeCompany);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            return dt;
        }
        #endregion
    }
}