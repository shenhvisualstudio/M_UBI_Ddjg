//
//文件名：    IBillDetail.cs
//功能描述：  电子提送货单详细信息接口类
//创建时间：  2016/03/22
//作者：      
//修改时间：  
//修改描述：  暂无
//
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Leo;
using ServiceInterface.Common;

namespace M_UBI_Ddjg.Bill
{
    /// <summary>
    /// Excel单元格字体下划线方式
    /// </summary>
    public enum QRCodeType
    {
        /// <summary>
        /// 港通卡
        /// </summary>
        GTK,
        /// <summary>
        /// 电子提送货单
        /// </summary>
        DZTSHD,
    }
    
    public class IBillDetail
    {
        //二维码类型
        private QRCodeType strQRCodeType { get; set; }

        BillDetailE billDetailE = new BillDetailE();

        #region 获取电子提送货单详细信息
        /// <summary>
        /// 获取电子提送货单详细信息
        /// </summary>
        /// <param name="strRCode"></param>
        /// <returns></returns>
        public string GetBillDetail(string strRCode)
        {
            string strJson = string.Empty;
            if (!GetQRCodeType(strRCode))
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "无效二维码！").DicInfo());
                return strJson;
            }

            switch (strQRCodeType)
            {
                //港通卡
                case QRCodeType.GTK:
                    strJson = GetInfoForHarborPassCard(strRCode);
                    break;
                //电子提送货单
                case QRCodeType.DZTSHD:
                    strJson = GetInfoForElectronicDeliverBill(strRCode);
                    break;
                default:
                    throw new Exception("错误的对象索引！");
            }

            return strJson;
        }
        #endregion 


        #region 获取二维码类型
        /// <summary>
        /// 获取二维码类型
        /// </summary>
        /// <param name="RCode"></param>
        /// <returns></returns>
        private bool GetQRCodeType(string strRCode)
        {
            //港通卡
            if (strRCode.Contains("http://boea.cn/M_Hmw/CardTask.html"))
            {
                strQRCodeType = QRCodeType.GTK;
                return true;
            }
            
            //电子提送货单
            if (strRCode.Contains("inGateNo"))
            {
                strQRCodeType = QRCodeType.DZTSHD;
                return true;
            }
                 
            return false;
        }
        #endregion 

        #region 获取港通卡编号
        /// <summary>
        /// 获取港通卡编号
        /// </summary>
        /// <param name="strRCode">二维码</param>

        private string GetInfoForHarborPassCard(string strRCode)
        {
            string strJson = string.Empty;
            try
            {
                //获取编码
                string strFixed = "http://boea.cn/M_Hmw/CardTask.html?info=";
                billDetailE.StrExter_No = strRCode.Substring(strFixed.Length, strRCode.Length - strFixed.Length);

                strJson = CheckCard();
                if (strJson != string.Empty)
                {
                    return strJson;
                }

                strJson = CheckVehicle();
                if (strJson != string.Empty)
                {
                    return strJson;
                }


                string strSql =
                    string.Format(@"select INGATENO,CODE_DEPARTMENT,CODE_DEPARTMENT_HB,CODE_STORAGE,CODE_BOOTH
                                    from V_CONSIGN_VEHICLE_ONLY_QUICK where VEHICLE='{0}' and EXTER_NO='{1}' and STATE_MARK<'9'",
                                    billDetailE.Vehicle, billDetailE.StrExter_No);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "该车[" + billDetailE.Vehicle + "]无有效运输申报！").DicInfo());
                    return strJson;
                }

                string[] strArray = new string[4];
                strArray[0] = dt.Rows[0]["code_department"].ToString();
                strArray[1] = dt.Rows[0]["code_storage"].ToString();
                strArray[2] = dt.Rows[0]["code_booth"].ToString();
                strArray[3] = dt.Rows[0]["ingateno"].ToString();

                strJson = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
                return strJson;
            }
            catch (Exception ex)
            {
             
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取港通卡编号数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
                return strJson;
            }
   
        }
        #endregion

        #region 获取电子提送货单编号
        /// <summary>
        /// 获取电子提送货单编号
        /// </summary>
        /// <param name="strRCode">二维码</param>

        private string GetInfoForElectronicDeliverBill(string strRCode)
        {
            string strJson = string.Empty;
            try
            {
                //获取编码
                string strFixed = "?inGateNo=";
                string strNum = strRCode.Substring(strFixed.Length, strRCode.Length - strFixed.Length);
                strNum = EncryptionTool.DES_Encrypt("gljsy", strNum);

                string strSql =
                    string.Format(@"select code_department,code_storage,code_booth,ingateno  
                                    from V_CONSIGN_VEHICLE_QUICK   
                                    where RID='{0}'",
                                    strNum);
                var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
                if (dt.Rows.Count <= 0)
                {
                    strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "该货单不存在！").DicInfo());
                    return strJson;
                }

                string[] strArray = new string[4];
                strArray[0] = dt.Rows[0]["code_department"].ToString();
                strArray[1] = dt.Rows[0]["code_storage"].ToString();
                strArray[2] = dt.Rows[0]["code_booth"].ToString();
                strArray[3] = dt.Rows[0]["ingateno"].ToString();

                strJson = JsonConvert.SerializeObject(new DicPackage(true, strArray, null).DicInfo());
                return strJson;
            }
            catch (Exception ex)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, string.Format("{0}：获取电子提送货单编号数据发生异常。{1}", ex.Source, ex.Message)).DicInfo());
                return strJson;
            }
 
        }
        #endregion

        /// <summary>
        /// 校验卡号
        /// </summary>
        /// <returns></returns>
        private string CheckCard()
        {
            string strJson = string.Empty;

            string strSql =
                           string.Format(@"select VEHICLE from TRANSIT.CARD where STATE='1' and FORBID_MARK='0' and EXTER_NO='{0}'",
                                           billDetailE.StrExter_No);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count <= 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "该卡[" + billDetailE.StrExter_No + "]尚未办理生效！").DicInfo());
                return strJson;
            }

            billDetailE.Vehicle = Convert.ToString(dt.Rows[0]["VEHICLE"]);

            return strJson;
        }

        /// <summary>
        /// 校验车号
        /// </summary>
        /// <returns></returns>
        private string CheckVehicle()
        {
            string strJson = string.Empty;

            string strSql =
            string.Format(@"select VEHICLE from TRANSIT.VEH_BLACKLIST where STATE='1' and VEHICLE='{0}'",
                            billDetailE.Vehicle);
            var dt = new Leo.Oracle.DataAccess(RegistryKey.KeyPathHarbor).ExecuteTable(strSql);
            if (dt.Rows.Count > 0)
            {
                strJson = JsonConvert.SerializeObject(new DicPackage(false, null, "该车[" + billDetailE.Vehicle + "]输入黑名单！").DicInfo());
                return strJson;
            }

            return strJson;

        }
    }
}