//
//文件名：    GetEvaluation.aspx.cs
//功能描述：  获取评价记录
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
    public partial class GetEvaluation : System.Web.UI.Page
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
            var strStartRow = Request.Params["StartRow"];
            //行数
            var strCount = Request.Params["Count"];
            //用户编码
            string strCodeUser = Request.Params["CodeUser"];

            try
            {
                if (strStartRow == null || strCount == null || strCodeUser == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取评价记录失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"select id,level_navigation,level_efficiency,level_service,to_char(createtime, 'yyyy-mm-dd HH24:mi:ss') as createtime,driver_remark
                                    from CGATE.TB_FREIGHT_EVALUATE
                                    where code_user ='{0}' and rownum < {1}
                                    order by createtime desc",
                                    strCodeUser, Convert.ToUInt32(strStartRow) + Convert.ToUInt32(strCount));
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql, Convert.ToInt32(strStartRow), Convert.ToInt32(strStartRow) + Convert.ToInt32(strCount) - 1);
                if (dt.Rows.Count <= 0)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "暂无评价记录").DicInfo());
                    return;
                }

                string[,] strArray = new string[dt.Rows.Count, 6];
                for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                {
                    strArray[iRow, 0] = Convert.ToString(dt.Rows[iRow]["id"]);
                    strArray[iRow, 1] = Convert.ToString(dt.Rows[iRow]["level_navigation"]);
                    strArray[iRow, 2] = Convert.ToString(dt.Rows[iRow]["level_efficiency"]);
                    strArray[iRow, 3] = Convert.ToString(dt.Rows[iRow]["level_service"]);
                    strArray[iRow, 4] = Convert.ToString(dt.Rows[iRow]["createtime"]);
                    strArray[iRow, 5] = Convert.ToString(dt.Rows[iRow]["driver_remark"]);
                }

                Json = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取评价记录数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}
//strStartRow = "1";
//strCount = "10";
//strCodeUser = "173ff7d8702a4919876608562a39d216";