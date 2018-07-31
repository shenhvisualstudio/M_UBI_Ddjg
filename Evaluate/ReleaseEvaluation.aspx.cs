//
//文件名：    ReleaseEvaluation.aspx.cs
//功能描述：  发布评价
//创建时间：  2016/03/28
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

namespace M_UBI_Ddjg.Evaluate
{
    public partial class ReleaseEvaluation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //用户编码
            string strCodeUser = Request.Params["CodeUser"];
            //运输申报编号
            string strInGateNo = Request.Params["InGateNo"];
            //导航评价
            string strLevelNavigation = Request.Params["LevelNavigation"];
            //效率评价
            string strLevelEfficiency = Request.Params["LevelEfficiency"];
            //服务平滑
            string strLevelService = Request.Params["LevelService"];
            //评价
            string strComment = Request.Params["Comment"];

            strComment = strComment == null ? string.Empty : strComment;

            try
            {
                if (strCodeUser == null || strInGateNo == null || strLevelNavigation == null || strLevelEfficiency == null || strLevelService == null || strComment == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，发布评价失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select id,vehicle,cgno,taskno from V_CONSIGN_VEHICLE_QUICK
                                    where ingateno ='{0}'", 
                                    strInGateNo);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "无效运输申报编号！").DicInfo());
                    return;
                }
                
                strSql =
                    string.Format(@"insert into CGATE.TB_FREIGHT_EVALUATE (consignvehicle_id,driver_mark,cgno,taskno,ingateno,vehicle,level_navigation,level_efficiency,level_service,code_user,driver_remark) 
                                    values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                    Convert.ToString(dt.Rows[0]["id"]), "0", Convert.ToString(dt.Rows[0]["cgno"]), Convert.ToString(dt.Rows[0]["taskno"]), strInGateNo, Convert.ToString(dt.Rows[0]["vehicle"]),
                                    strLevelNavigation, strLevelEfficiency, strLevelService, strCodeUser, strComment);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteNonQuery(strSql);

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "发布成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：发布评价数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}
//strCodeUser = "111111111";
//strInGateNo = "1506115822";
//strLevelNavigation = "3.5";
//strLevelEfficiency = "3.5";
//strLevelService = "3.5";
//strComment = "很好，很满意！";