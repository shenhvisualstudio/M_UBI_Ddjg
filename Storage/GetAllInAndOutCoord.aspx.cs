//
//文件名：    GetAllInAndOutCoord.aspx.cs
//功能描述：  获取所有场地进出点经、纬度坐标
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
    public partial class GetAllInAndOutCoord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            try
            {
                IInAndOutCoord iStorage = new IInAndOutCoord();
                List<List<string>> listArray = new List<List<string>>();
                foreach (string strCodeCompany in iStorage.GetCodeCompany())
                {
                    var dt = iStorage.GetInAndOutCoord(strCodeCompany);     
                    if (dt.Rows.Count > 0)
                    {      
                        for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                        {
                            List<string> list = new List<string>();
                            list.Add(dt.Rows[iRow]["in_long"].ToString());
                            list.Add(dt.Rows[iRow]["in_latitude"].ToString());
                            list.Add(dt.Rows[iRow]["out_long"].ToString());
                            list.Add(dt.Rows[iRow]["out_latitude"].ToString());
                            list.Add(dt.Rows[iRow]["code_storage"].ToString().ToUpper());
                            listArray.Add(list);    
                        }                 
                    }               
                }

                Json = JsonConvert.SerializeObject(new DicPackage(false, listArray.ToArray(), null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取所有场地进出点经、纬度坐标数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}