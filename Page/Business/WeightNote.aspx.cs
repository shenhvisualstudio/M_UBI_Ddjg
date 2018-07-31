//
//文件名：    WeightNote.aspx.cs
//功能描述：  衡重查询
//创建时间：  2016/07/13
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
using ServiceInterface.Common;
using Leo;

namespace M_UBI_Ddjg.Page.Business
{
    public partial class WeightNote : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                string strMessage = Request.Params["message"];
                var strW = strMessage.Split(' ');
                string strVehicleNum = strW[0];
                string strMinRow = strW[1];
                string strMaxRow = strW[2];

                //strCardNum = "600675";

                string strSql = string.Empty;
 
                strSql =
                    string.Format(@"select count(consign) as sum 
                                    from BalanceCenter..vw_Metages 
                                    where truck like '{0}%' and datediff(day,finishtime,getdate())<=7",
                                    strVehicleNum);

                var dt0 = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
                if (dt0.Rows.Count <= 0)
                {
                    var arry0 = 0;
                    Json = JsonConvert.SerializeObject(arry0);
                    return;
                }

      
                strSql =
                    string.Format(@"select top {2} consign,finishtime,Ticket,Truck,Amount,weight2,weight1,(weight2-weight1) weight,operator2,balance2 
                                    from (select TOP {1} consign,finishtime,Ticket,Truck,Amount,weight2,weight1,(weight2-weight1) weight,operator2,balance2 
                                    from BalanceCenter..vw_Metages 
                                    where truck like '{0}%' and datediff(day,finishtime,getdate())<=7 order by finishtime asc)a 
                                    order by finishtime desc",
                                    strVehicleNum, Convert.ToInt32(dt0.Rows[0]["sum"]) - Convert.ToInt32(strMinRow) + 1, Convert.ToInt32(strMaxRow) - Convert.ToInt32(strMinRow) + 1);
       
                var dt1 = new Leo.SqlServer.DataAccess(RegistryKey.KeyPathBc).ExecuteTable(strSql);
                if (dt1.Rows.Count <= 0)
                {
                    var arry0 = 0;
                    Json = JsonConvert.SerializeObject(arry0);
                    return;
                }

                var arry1 = new Leo.Data.Table(dt1).ToArray();
                Json = JsonConvert.SerializeObject(arry1);

            }
            catch (Exception)
            {
                return;
            }
        }
        protected string Json;
    }
}