//
//文件名：    DeleteEvaluation.aspx.cs
//功能描述：  删除评价记录
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
    public partial class DeleteEvaluation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //ID
            string strId = Request.Params["Id"];

            try
            {
                if (strId == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，删除评价记录失败！").DicInfo());
                    return;
                }

                string strSql =
                    string.Format(@"delete
                                    from CGATE.TB_FREIGHT_EVALUATE
                                    where id ='{0}'",
                                    strId);
                new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteNonQuery(strSql);

                Json = JsonConvert.SerializeObject(new DicPackage(true, null, "删除成功！").DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：删除评价记录数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}
//strId = "139858";