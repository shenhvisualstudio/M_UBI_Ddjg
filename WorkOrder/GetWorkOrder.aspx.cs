//
//文件名：    GetWorkOrder.aspx.cs
//功能描述：  获取工单数据
//创建时间：  2016/04/13
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

namespace M_UBI_Ddjg.WorkOrder
{
    public partial class GetWorkOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //工单编码
            string strWoNo = Request.Params["WoNo"];

            try
            {
                string strSql =
                      string.Format(@"select ingateno
                                      from TB_PRO_ConsignVehicle 
                                      where ingateno='{0}'",
                                      strWoNo);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "无运输申报！").DicInfo());
                    return;
                }

                strSql =
                      string.Format(@"select b.coord1,b.latitude1,b.longitude1,b.coord2,b.latitude2,b.longitude2,b.coord3,b.latitude3,b.longitude3,b.coord4,b.latitude4,b.longitude4,b.coord5,b.latitude5,b.longitude5
                                      from TB_PRO_ConsignVehicle a, VW_PRO_MULTIPOINT_ORDER b 
                                      where a.code_department=b.code_department and a.code_storage=b.code_storage and a.ingateno='{0}'",
                                      strWoNo);
                dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "本导航暂未支持该公司！").DicInfo());
                    return;
                }

                List<List<string>> listArray = new List<List<string>>();
                List<string> list = null;
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["coord1"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["latitude1"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["longitude1"])))
                {
                    list = new List<string>();
                    list.Add(Convert.ToString(dt.Rows[0]["coord1"]));
                    list.Add(Convert.ToString(dt.Rows[0]["latitude1"]));
                    list.Add(Convert.ToString(dt.Rows[0]["longitude1"]));
                    listArray.Add(list);
                }
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["coord2"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["latitude2"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["longitude2"])))
                {
                    list = new List<string>();
                    list.Add(Convert.ToString(dt.Rows[0]["coord2"]));
                    list.Add(Convert.ToString(dt.Rows[0]["latitude2"]));
                    list.Add(Convert.ToString(dt.Rows[0]["longitude2"]));
                    listArray.Add(list);
                }
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["coord3"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["latitude3"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["longitude3"])))
                {
                    list = new List<string>();
                    list.Add(Convert.ToString(dt.Rows[0]["coord3"]));
                    list.Add(Convert.ToString(dt.Rows[0]["latitude3"]));
                    list.Add(Convert.ToString(dt.Rows[0]["longitude3"]));
                    listArray.Add(list);
                }
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["coord4"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["latitude4"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["longitude4"])))
                {
                    list = new List<string>();
                    list.Add(Convert.ToString(dt.Rows[0]["coord4"]));
                    list.Add(Convert.ToString(dt.Rows[0]["latitude4"]));
                    list.Add(Convert.ToString(dt.Rows[0]["longitude4"]));
                    listArray.Add(list);
                }
                if (!string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["coord5"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["latitude5"])) && !string.IsNullOrWhiteSpace(Convert.ToString(dt.Rows[0]["longitude5"])))
                {
                    list = new List<string>();
                    list.Add(Convert.ToString(dt.Rows[0]["coord5"]));
                    list.Add(Convert.ToString(dt.Rows[0]["latitude5"]));
                    list.Add(Convert.ToString(dt.Rows[0]["longitude5"]));
                    listArray.Add(list);
                }

                if (listArray.Count == 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "无途经点！").DicInfo());
                }
                else
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(true, listArray.ToArray(), null).DicInfo());
                }
                
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取工单数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}
//strWoNo = "1506115822";//新苏港
//strWoNo = "1502065105";//新陆桥
//strWoNo = "1502065101";//测试
//strWoNo = "1606146877";//东联802
//strWoNo = "1606309920";//西货场204
