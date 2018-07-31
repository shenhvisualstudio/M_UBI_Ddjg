//
//文件名：    GetGoods.aspx.cs
//功能描述：  获取货源记录
//创建时间：  2016/03/21
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

namespace M_UBI_Ddjg.Goods
{
    public partial class GetGoods : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //数据起始行
            string strStartRow = Request.Params["StartRow"];
            //行数
            string strCount = Request.Params["Count"];
            //默认为5条
            strStartRow = "1";
            strCount = "5";


            try
            {
                if (strStartRow == null || strCount == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取货源记录失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select * from TB_DMT_CARGO
                                    where is_end ='0' 
                                    order by opeartetime desc");
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathWlxgx).ExecuteTable(strSql, Convert.ToInt32(strStartRow), Convert.ToInt32(strStartRow) + Convert.ToInt32(strCount) - 1);
                if (dt.Rows.Count <= 0)
                {
                    string strWarning = strStartRow == "1" ? "暂无数据！" : "暂无更多数据！";
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, strWarning).DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 17];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = dt.Rows[iRow]["pkid"].ToString();
                    strArray[iRow, 1] = dt.Rows[iRow]["sfd_province"].ToString();
                    strArray[iRow, 2] = dt.Rows[iRow]["sfd_city"].ToString();
                    strArray[iRow, 3] = dt.Rows[iRow]["sfd_county"].ToString();
                    strArray[iRow, 4] = dt.Rows[iRow]["sfd_address"].ToString();
                    strArray[iRow, 5] = dt.Rows[iRow]["mdd_province"].ToString();
                    strArray[iRow, 6] = dt.Rows[iRow]["mdd_city"].ToString();
                    strArray[iRow, 7] = dt.Rows[iRow]["mdd_county"].ToString();
                    strArray[iRow, 8] = dt.Rows[iRow]["mdd_address"].ToString();
                    strArray[iRow, 9] = dt.Rows[iRow]["cargoname"].ToString();
                    strArray[iRow, 10] = dt.Rows[iRow]["weight"].ToString();
                    strArray[iRow, 11] = dt.Rows[iRow]["vehicletype"].ToString();
                    strArray[iRow, 12] = dt.Rows[iRow]["vehiclelen"].ToString();
                    strArray[iRow, 13] = dt.Rows[iRow]["diatance"].ToString();
                    strArray[iRow, 14] = dt.Rows[iRow]["opeartetime"].ToString();
                    strArray[iRow, 15] = dt.Rows[iRow]["contact_man"].ToString();
                    strArray[iRow, 16] = dt.Rows[iRow]["mobile"].ToString();                               
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取货源记录数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}