//
//文件名：    GetInAndOutCoord.aspx.cs
//功能描述：  获取场地进出点经、纬度坐标
//创建时间：  2016/03/22
//作者：      
//修改时间：  
//修改描述：  暂无
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;

namespace M_UBI_Ddjg.Storage
{
    public partial class GetInAndOutCoord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //公司编码
            string strCodeCompany = Request.Params["CodeCompany"];
            //库场编码
            string strCodeStorage = Request.Params["CodeStorage"];

            try
            {
                if (strCodeCompany == null || strCodeStorage == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取场地进出点经、纬度坐标失败！").DicInfo());
                    return;
                }

                string strSql =
                              string.Format(@"select a.in_long,a.in_latitude,a.out_long,a.out_latitude
                                              from TB_CODE_STORAGE a, TB_CODE_DEPARTMENT b
                                              where a.code_department=b.code_department and a.code_storage='{0}' and b.code_company_pbas='{1}' and a.isusing='1'",
                                              strCodeStorage, strCodeCompany);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无数据！").DicInfo());
                    return;
                }
                
                string[] strArray = new string[4];
                strArray[0] = Convert.ToString(dt.Rows[0]["in_long"].ToString());
                strArray[1] = Convert.ToString(dt.Rows[0]["in_latitude"].ToString());
                strArray[2] = Convert.ToString(dt.Rows[0]["out_long"].ToString());
                strArray[3] = Convert.ToString(dt.Rows[0]["out_latitude"].ToString());

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取场地进出点经、纬度坐标数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}
//strCodeCompany = "405";
//strCodeStorage = "2";
//strCodeCompany = "191";
//strCodeStorage = "XHC";