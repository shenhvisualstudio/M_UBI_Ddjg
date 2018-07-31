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
    public partial class Weight : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                var message = Request.Params["message"];
                var str = message.ToString().Split(' ');
                var userCode = str[0];
                var DelegationNum = str[1];
                var minRow = str[2];
                var maxRow = str[3];

                var sql =
                    string.Format(
                            "select count(consign) as sum from BalanceCenter..vw_Metages where taskno='{0}'", DelegationNum);
                var dt = new Leo.SqlServer.DataAccess(Leo.RegistryKey.KeyPathBc).ExecuteTable(sql);
                if (dt.Rows.Count == 0)
                {
                    var arry = 0;
                    Json = JsonConvert.SerializeObject(arry);
                    return;
                }

                //sql =
                //    string.Format(
                //        "select count(consign) as vehiclesum, sum(weight2) as  roughsum,sum(weight1) as taresum, sum(weight2-weight1) as suttlesum from BalanceCenter..vw_Metages where taskno='{0}'",
                //        DelegationNum);
                //var dt0 = new Leo.SqlServer.DataAccess(Leo.RegistryKey.KeyPathBc).ExecuteTable(sql);
                //if (dt0.Rows.Count == 0)
                //{
                //    var arry = 0;
                //    Json = JsonConvert.SerializeObject(arry);
                //    return;
                //}
                //var arry0 = new Leo.Data.Table(dt0).ToArray();

                //sql =
                //    string.Format(
                //        "select sum(Weight2-Weight1)/count(Weight2-Weight1)*count(*) as WeightV from BalanceCenter..Metage  where  taskno='{0}'",
                //        DelegationNum);
                //var dt1 = new Leo.SqlServer.DataAccess(Leo.RegistryKey.KeyPathBc).ExecuteTable(sql);
                //if (dt1.Rows.Count == 0)
                //{
                //    var arry = 0;
                //    Json = JsonConvert.SerializeObject(arry);
                //    return;
                //}
                //var arry1 = new Leo.Data.Table(dt1).ToArray();

                //sql =
                //    string.Format(
                //        " select weight from BalanceCenter..consign where taskno='{0}'",
                //        DelegationNum);
                //var dt2 = new Leo.SqlServer.DataAccess(Leo.RegistryKey.KeyPathBc).ExecuteTable(sql);
                //if (dt2.Rows.Count == 0)
                //{
                //    var arry = 0;
                //    Json = JsonConvert.SerializeObject(arry);
                //    return;
                //}
                //var arry2 = new Leo.Data.Table(dt2).ToArray();

                sql =
                    string.Format(
                        "select top {2} consign,convert(varchar(100), finishtime, 2),Ticket,Truck,Amount,weight2,weight1,(weight2-weight1) weight ,operator2 from (select TOP {1} consign,finishtime,Ticket,Truck,Amount,weight2,weight1,(weight2-weight1) weight ,operator2 from BalanceCenter..vw_Metages where taskno='{0}' order by finishtime asc)a order by finishtime desc",
                        DelegationNum, Convert.ToInt32(dt.Rows[0]["sum"]) - Convert.ToInt32(minRow) + 1, Convert.ToInt32(maxRow) - Convert.ToInt32(minRow) + 1);
                var dt3 = new Leo.SqlServer.DataAccess(Leo.RegistryKey.KeyPathBc).ExecuteTable(sql);
                if (dt3.Rows.Count == 0)
                {
                    var arry = 0;
                    Json = JsonConvert.SerializeObject(arry);
                    return;
                }
                var arry3 = new Leo.Data.Table(dt3).ToArray();

                var arrys = new Array[1];
                //arrys[0] = arry0;
                //arrys[1] = arry1;
                //arrys[2] = arry2;
                arrys[0] = arry3;
                Json = JsonConvert.SerializeObject(arrys);

            }
            catch (Exception ex)
            {
                //LogTool.WriteLog(typeof(Weight), ex);
            }
        }
        protected string Json;
    }
}