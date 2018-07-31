//
//文件名：    IStorageCoord.cs
//功能描述：  剁位信息接口类
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
    public class IStorageCoord
    {
        //Iport公司编码
        private string[] strCodeCompanyArray = 
        { 
            "010219",    //新路桥"191"
            "010246",    //新苏港"405"    
            "010111",    //东联"77"
            "010116",    //东泰"39"
            "010113",    //东源"111"
            "010118"     //东粮"135"
        };

        #region 获取公司编码
        public string[] GetCodeCompany()
        {
            return strCodeCompanyArray;
        }
        #endregion

        #region 获取各公司港剁位信息
        /// <summary>
        /// 获取各公司港剁位信息
        /// </summary>
        /// <param name="strCodeCompany">公司编码</param>
        /// <returns>剁位信息</returns>
        public DataTable GetStorageCoord(string strCodeCompany)
        {
            switch (strCodeCompany)
            {
                case "010219":
                    return Get_XLQ_StorageCoord();
                case "010246":
                case "010111":
                case "010116":
                case "010113":
                case "010118":
                    return Get_Other_StorageCoord(strCodeCompany);
                default:
                    throw new Exception("错误对象索引！");
            }
        }
        #endregion

        #region 获取其他公司剁位信息
        /// <summary>
        /// 获取新苏港剁位信息
        /// </summary>
        /// <param name="strCodeCompany">公司编码</param>
        /// <returns>剁位信息</returns>
        private DataTable Get_Other_StorageCoord(string strCodeCompany)
        {
            try
            {
                ServiceXLQ.Service1SoapClient service = new ServiceXLQ.Service1SoapClient();
                string strSql =
                         string.Format("select distinct * from VW_MASS_INFO where code_department='{0}'", strCodeCompany);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                return dt;
            }
            catch
            {
                return null;
            } 
        }
        #endregion

        #region 获取新陆桥剁位信息
        /// <summary>
        /// 获取新陆桥剁位信息
        /// </summary>
        /// <returns>剁位信息</returns>
        private DataTable Get_XLQ_StorageCoord()
        {
            string strJson = string.Empty;
            try
            {
                ServiceXLQ.Service1SoapClient service = new ServiceXLQ.Service1SoapClient();
                DataSet ds = service.getBoundary();
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch
            {
                return null;
            }            
        }
        #endregion
    }
}