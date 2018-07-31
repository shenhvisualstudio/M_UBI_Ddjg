//
//文件名：    GetBillDetail.aspx.cs
//功能描述：  获取电子提送货单详细信息
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

namespace M_UBI_Ddjg.Bill
{
    public partial class GetBillDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //身份校验
            if (!InterfaceTool.IdentityVerify(Request))
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, "身份认证错误！").DicInfo());
                return;
            }

            //二维码内容
            string strQRCode = Request.Form["QRCode"];

            try
            {
                if (strQRCode == null)
                {
                    Json = JsonConvert.SerializeObject(new DicPackage(false, null, "参数错误，获取电子提送货单详细信息失败！").DicInfo());
                    return;
                }

                IBillDetail iBillDetail = new IBillDetail();
                Json = iBillDetail.GetBillDetail(strQRCode);
            }
            catch (Exception ex)
            {
                Json = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取电子提送货单详细信息数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
            }
        }
        protected string Json;
    }
}

//strQRCode = "http://boea.cn/M_Hmw/CardTask.html?info=900553";
//strQRCode = "?inGateNo=C1hstQ2Erq1/LueUh6E24ibwfQELqEUO";  //新苏港
//strQRCode = "?inGateNo=C1hstQ2Erq2HzTkX8JclJ67EZiobEyhu";  //新陆桥
//strQRCode = "?inGateNo=C1hstQ2Erq2h75LXUM47GS5t1ejiVgeI";  //新陆桥M102
//strQRCode = "?inGateNo=C1hstQ2Erq2IquHPWlju7bm3JGqHKj0I";  //东联802
//strQRCode = "?inGateNo=C1hstQ2Erq2ORDod7U1Di+R8P1OGdMwW";  //新陆桥西货场
//strQRCode = "?inGateNo=C1hstQ2Erq1b4Dw9N6NyhWY5Bih7/oER";  //西货场204