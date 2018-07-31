//
//文件名：    GetStorageCoord.aspx.cs
//功能描述：  获取剁位信息
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
    public partial class GetStorageCoord : System.Web.UI.Page
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
                IStorageCoord iStorage = new IStorageCoord();

                List<List<string>> listArray = new List<List<string>>();
                foreach (string strCodeCompany in iStorage.GetCodeCompany())
                {
                    var dt = iStorage.GetStorageCoord(strCodeCompany);
                    if (dt == null)
                    {
                        continue;
                    }
                     
                    if (dt.Rows.Count > 0)
                    {
                        if (strCodeCompany == "010219")
                        {
                            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                            {
                                List<string> list = new List<string>();
                                list.Add(dt.Rows[iRow]["mass_num"].ToString());
                                list.Add(dt.Rows[iRow]["vertex_count"].ToString());
                                list.Add(dt.Rows[iRow]["vertex1_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex1_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex2_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex2_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex3_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex3_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex4_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex4_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex5_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex5_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex6_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex6_long"].ToString());
                                list.Add(dt.Rows[iRow]["areacode"].ToString().ToUpper());
                                listArray.Add(list);
                            }
                        }
                        else
                        {
                            for (int iRow = 0; iRow < dt.Rows.Count; iRow++)
                            {
                                List<string> list = new List<string>();
                                list.Add(dt.Rows[iRow]["mass_num"].ToString());
                                list.Add(dt.Rows[iRow]["vertex_count"].ToString());
                                list.Add(dt.Rows[iRow]["vertex1_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex1_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex2_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex2_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex3_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex3_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex4_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex4_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex5_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex5_long"].ToString());
                                list.Add(dt.Rows[iRow]["vertex6_latitude"].ToString());
                                list.Add(dt.Rows[iRow]["vertex6_long"].ToString());
                                list.Add(dt.Rows[iRow]["code_storage"].ToString().ToUpper());
                                listArray.Add(list);
                            }                
                        }
                    }
                }                              

                Json = JsonConvert.SerializeObject(new DicPackage(true,  listArray.ToArray(), null).DicInfo());
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取剁位信息数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}