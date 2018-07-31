//
//文件名：    BindingVehilceNum.aspx.cs
//功能描述：  绑定车牌号
//创建时间：  2016/03/25
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
using YGSoft.IPort.Data;

namespace M_UBI_Ddjg.Vehicle
{
    public partial class BindingVehilceNum : System.Web.UI.Page
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
            //车牌号
            string strVehicleNum = Request.Params["VehicleNum"];

            try
            {
                if (strCodeUser == null || strVehicleNum == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，修改密码失败！").DicInfo());
                    return;

                }

                string strSql =
                    string.Format("select * from TB_APP_DDJG_VEHICLE where code_user = '{0}'", strCodeUser);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)//插入
                {
                    strSql = 
                        string.Format(@"insert into TB_APP_DDJG_VEHICLE (code_user, vehiclenum) 
                                        values('{0}','{1}')", strCodeUser, strVehicleNum);
                }
                else//更新
                { 
                    strSql = 
                        string.Format(@"update TB_APP_DDJG_VEHICLE
                                        set code_user='{0}',vehiclenum='{1}'",
                                        strCodeUser, strVehicleNum);
                }
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathMa).ExecuteNonQuery(strSql);

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "绑定车牌号成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：绑定车牌号数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}

//strCodeUser = "121907";
//strOVehicleNum = "苏A23456";