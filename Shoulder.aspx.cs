﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using IntakeSheet;
using System.Configuration;
using System.IO;
using log4net;

public partial class Shoulder : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _CurBP = "Shoulder";
    string Position = "";
    string pos = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();
    ILog log = log4net.LogManager.GetLogger(typeof(Shoulder));


    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        switch (Position)
        {
            case "L":
                pos = "left";
                break;
            case "R":
                pos = "right";
                break;
            case "B":
                pos = "bilateral";
                break;

        }
        Session["PageName"] = "Shoulder";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            ViewState["saveDaigno"] = "0";
            BindROM();
            checkTP();
            if (Session["PatientIE_ID"] != null)
            {
                _CurIEid = Session["PatientIE_ID"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as count1 FROM tblbpShoulder WHERE PatientIE_ID= " + Session["PatientIE_ID"].ToString() + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter da = new SqlDataAdapter(cm);
                cn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds);
                cn.Close();
                DataRow rw = ds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("count1") == 0);
                if (rw != null)
                {
                    // row exists
                    PopulateUIDefaults();
                    BindDataGrid();
                    bindCC(pos);
                    bindPE(pos);
                }
                else
                {
                    PopulateUI(_CurIEid);
                    BindDataGrid();
                }
                if (Position != "")
                {
                    switch (Position)
                    {
                        case "L":

                            //first div
                            //WrapLeft.Visible = true;
                            //wrpRight.Visible = false;
                            //second div
                            //wrpPELeft.Visible = true;
                            //wrpPERight.Visible = false;
                            //Right
                            //txtAbductionLeft.ReadOnly = true;
                            //txtFlexionLeft.ReadOnly = true;
                            //txtExtRotationLeft.ReadOnly = true;
                            //txtIntRotationLeft.ReadOnly = true;
                            //Left
                            //txtAbductionLeftWas.ReadOnly = false;
                            //txtFlexionLeftWas.ReadOnly = false;
                            //txtExtRotationLeftWas.ReadOnly = false;
                            //txtIntRotationLeftWas.ReadOnly = false;
                            //Left checkbox
                            //chkNeerLeft.Enabled = true;
                            //chkHawkinLeft.Enabled = true;
                            //chkYergasonsLeft.Enabled = true;
                            //chkDropArmLeft.Enabled = true;
                            //chkReverseBeerLeft.Enabled = true;
                            ////Right checkbox
                            //chkNeerRight.Enabled = false;
                            //chkHawkinRight.Enabled = false;
                            //chkYergasonsRight.Enabled = false;
                            //chkDropArmRight.Enabled = false;
                            //chkReverseBeerRight.Enabled = false;
                            //Left 
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "left";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "left";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "left";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "left";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "left";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "left";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "left";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "left";

                            break;
                        case "R":
                            pos = "right";
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = false;
                            //second div
                            //wrpPELeft.Visible = false;
                            //wrpPERight.Visible = true;
                            //Left
                            //txtAbductionLeftWas.ReadOnly = true;
                            //txtFlexionLeftWas.ReadOnly = true;
                            //txtExtRotationLeftWas.ReadOnly = true;
                            //txtIntRotationLeftWas.ReadOnly = true;
                            //right
                            //txtAbductionLeft.ReadOnly = false;
                            //txtFlexionLeft.ReadOnly = false;
                            //txtExtRotationLeft.ReadOnly = false;
                            //txtIntRotationLeft.ReadOnly = false;
                            //Left checkbox
                            //chkNeerLeft.Enabled = false;
                            //chkHawkinLeft.Enabled = false;
                            //chkYergasonsLeft.Enabled = false;
                            //chkDropArmLeft.Enabled = false;
                            //chkReverseBeerLeft.Enabled = false;
                            ////Right checkbox
                            //chkNeerRight.Enabled = true;
                            //chkHawkinRight.Enabled = true;
                            //chkYergasonsRight.Enabled = true;
                            //chkDropArmRight.Enabled = true;
                            //chkReverseBeerRight.Enabled = true;
                            //Dropdown
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "right";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "right";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "right";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "right";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "right";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "right";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "right";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "right";

                            break;
                        case "B":
                            pos = "bilateral";
                            //first div
                            //wrpRight.Visible = true;
                            //WrapLeft.Visible = true;
                            //second div
                            //wrpPELeft.Visible = true;
                            //wrpPERight.Visible = true;
                            //Left
                            //txtAbductionLeftWas.ReadOnly = false;
                            //txtFlexionLeftWas.ReadOnly = false;
                            //txtExtRotationLeftWas.ReadOnly = false;
                            //txtIntRotationLeftWas.ReadOnly = false;
                            //right
                            //txtAbductionLeft.ReadOnly = false;
                            //txtFlexionLeft.ReadOnly = false;
                            //txtExtRotationLeft.ReadOnly = false;
                            //txtIntRotationLeft.ReadOnly = false;
                            //Left checkbox
                            //chkNeerLeft.Enabled = true;
                            //chkHawkinLeft.Enabled = true;
                            //chkYergasonsLeft.Enabled = true;
                            //chkDropArmLeft.Enabled = true;
                            //chkReverseBeerLeft.Enabled = true;
                            ////Right checkbox
                            //chkNeerRight.Enabled = true;
                            //chkHawkinRight.Enabled = true;
                            //chkYergasonsRight.Enabled = true;
                            //chkDropArmRight.Enabled = true;
                            //chkReverseBeerRight.Enabled = true;
                            //Dropdown
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "bilateral";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "bilateral";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "bilateral";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "bilateral";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "bilateral";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "bilateral";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "bilateral";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "bilateral";

                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("Page1.aspx");
            }
            bindgridPoup();
        }
        BindDCDataGrid();
        Logger.Info(Session["uname"].ToString() + "- Visited in  Shoulder for -" + Convert.ToString(Session["LastNameIE"]) + Convert.ToString(Session["FirstNameIE"]) + "-" + DateTime.Now);
    }


    public string SaveUI(string ieID, string ieMode, bool bpChecked)
    {
        long _ieID = Convert.ToInt64(ieID);
        string _ieMode = "";

        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpShoulder WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["PatientIE_ID"] = _ieID;

            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["CCvalue"] = hdCCvalue.Value;
            TblRow["CCvalueoriginal"] = hdorgCC.Value;

            TblRow["PEvalue"] = hdPEvalue.Value;
            TblRow["PEvalueoriginal"] = hdorgPE.Value;

            TblRow["PESides"] = hdPESides.Value;
            TblRow["PESidesText"] = hdPESidesText.Value;
            TblRow["TPText"] = hdTPText.Value;

            string strname = "", strleft = "", strright = "", strnormal = "";

            for (int i = 0; i < repROM.Items.Count; i++)
            {
                Label lblname = repROM.Items[i].FindControl("lblname") as Label;
                TextBox txtleft = repROM.Items[i].FindControl("txtleft") as TextBox;
                TextBox txtright = repROM.Items[i].FindControl("txtright") as TextBox;
                TextBox txtnormal = repROM.Items[i].FindControl("txtnormal") as TextBox;

                strname = strname + "," + lblname.Text;
                strleft = strleft + "," + txtleft.Text;
                strright = strright + "," + txtright.Text;
                strnormal = strnormal + "," + txtnormal.Text;
            }

            TblRow["LeftROM"] = strleft.Substring(1);
            TblRow["RightROM"] = strright.Substring(1);
            TblRow["NormalROM"] = strnormal.Substring(1);
            TblRow["NameROM"] = strname.Substring(1);

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
        if (_ieMode == "New")
            return "Shoulder has been added...";
        else if (_ieMode == "Update")
            return "Shoulder has been updated...";
        else if (_ieMode == "Delete")
            return "Shoulder has been deleted...";
        else
            return "";


    }
    public void PopulateStrightFwd()
    {
        //bool bLeft = bL;
        //bool bRight = bR;
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtAbductionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtAbductionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal.Visibility = System.Windows.Visibility.Visible;
        //txtAbductionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtExtRotationNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFlexionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtIntRotationNormal.Visibility = System.Windows.Visibility.Visible;

        //wrpPELeft.IsEnabled = bLeft;
        //grdROMLeft.IsEnabled = bLeft;
        //wrpLeft1.IsEnabled = bLeft;
        //wrpLeft2.IsEnabled = bLeft;
        //wrpLeft3.IsEnabled = bLeft;
        //grdTestLeft.IsEnabled = bLeft;

        //txtAbductionLeft.IsEnabled = bLeft;
        //txtFlexionLeft.IsEnabled = bLeft;
        //txtExtRotationLeft.IsEnabled = bLeft;
        //txtIntRotationLeft.IsEnabled = bLeft;
        //txtAbductionLeftWas.IsEnabled = bLeft;
        //txtFlexionLeftWas.IsEnabled = bLeft;
        //txtExtRotationLeftWas.IsEnabled = bLeft;
        //txtIntRotationLeftWas.IsEnabled = bLeft;

        //wrpPERight.IsEnabled = bRight;
        //grdROMRight.IsEnabled = bRight;
        //wrpRight1.IsEnabled = bRight;
        //wrpRight2.IsEnabled = bRight;
        //grdTestRight.IsEnabled = bRight;


        //txtAbductionRight.IsEnabled = bRight;
        //txtFlexionRight.IsEnabled = bRight;
        //txtExtRotationRight.IsEnabled = bRight;
        //txtIntRotationRight.IsEnabled = bRight;
        //txtAbductionRightWas.IsEnabled = bRight;
        //txtFlexionRightWas.IsEnabled = bRight;
        //txtExtRotationRightWas.IsEnabled = bRight;
        //txtIntRotationRightWas.IsEnabled = bRight;

        //chkNeerLeft.IsEnabled = bLeft;
        //chkNeerLeft.IsChecked = bLeft;
        //chkHawkinLeft.IsEnabled = bLeft;
        //chkHawkinLeft.IsChecked = bLeft;
        //chkYergasonsLeft.IsEnabled = bLeft;
        //chkDropArmLeft.IsEnabled = bLeft;
        //chkReverseBeerLeft.IsEnabled = bLeft;

        //chkNeerRight.IsEnabled = bRight;
        //chkNeerRight.IsChecked = bRight;
        //chkHawkinRight.IsEnabled = bRight;
        //chkHawkinRight.IsChecked = bRight;
        //chkYergasonsRight.IsEnabled = bRight;
        //chkDropArmRight.IsEnabled = bRight;
        //chkReverseBeerRight.IsEnabled = bRight;

        //if (bLeft && bRight)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 3;
        //else if (bLeft)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 1;
        //else
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 2;
    }

    public void PopulateUI(string ieID)
    {


        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpShoulder WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];


            hdTPText.Value = TblRow["TPText"].ToString();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();

            string cc = sqlTbl.Rows[0]["CCvalue"].ToString();

            string p = Request.QueryString["P"].ToLower();



            CF.InnerHtml = cc;

            string pe = sqlTbl.Rows[0]["PEvalue"].ToString();


            divPE.InnerHtml = pe;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "sideFun", "displaySide('" + p + "');", true);



            int val = checkTP();
            // ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ",'" + pos + "')", true);

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "checkTP(" + val.ToString() + ",'" + pos + "',1);bindSidesVal('" + sqlTbl.Rows[0]["PESides"].ToString() + "','" + sqlTbl.Rows[0]["PESidesText"].ToString() + "')", true);

            //cboSprainStrainSide.Text = TblRow["SprainStrainSide"].ToString().Trim();
            //chkSprainStrain.Checked = Convert.ToBoolean(TblRow["SprainStrain"]);
            //cboDerangmentSide.Text = TblRow["DerangmentSide"].ToString().Trim();
            //chkDerangment.Checked = Convert.ToBoolean(TblRow["Derangment"]);
            //cboSyndromeSide.Text = TblRow["SyndromeSide"].ToString().Trim();
            //chkSyndrome.Checked = Convert.ToBoolean(TblRow["Syndrome"]);
            //chkPlan.Checked = Convert.ToBoolean(TblRow["Plan"]);
            //cboScanType.Text = TblRow["ScanType"].ToString().Trim();
            //cboScanSide.Text = TblRow["ScanSide"].ToString().Trim();
            // chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            _fldPop = false;
        }

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }
    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        //cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Shoulder");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;

            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            //if (isTP == false)
            //    divTP.Attributes.Add("style", "display:none");
            //else
            //    divTP.Attributes.Add("style", "display:block");

            //txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            //chkSharpLeft.Checked = node.SelectSingleNode("SharpLeft") == null ? chkSharpLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpLeft").InnerText);
            //chkSharpLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkSharpLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            //chkShootingLeft.Checked = node.SelectSingleNode("ShootingLeft") == null ? chkShootingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingLeft").InnerText);
            //chkThrobblingLeft.Checked = node.SelectSingleNode("ThrobblingLeft") == null ? chkThrobblingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingLeft").InnerText);
            //chkPulsatingLeft.Checked = node.SelectSingleNode("PulsatingLeft") == null ? chkPulsatingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingLeft").InnerText);
            //chkDullLeft.Checked = node.SelectSingleNode("DullLeft") == null ? chkDullLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DullLeft").InnerText);
            //chkAchyLeft.Checked = node.SelectSingleNode("AchyLeft") == null ? chkAchyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyLeft").InnerText);
            //chkWorseLyingLeft.Checked = node.SelectSingleNode("WorseLyingLeft") == null ? chkWorseLyingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingLeft").InnerText);
            //chkWorseMovementLeft.Checked = node.SelectSingleNode("WorseMovementLeft") == null ? chkWorseMovementLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementLeft").InnerText);
            ////chkWorseRaisingLeft.Checked = node.SelectSingleNode("WorseRaisingLeft") == null ? chkWorseRaisingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRaisingLeft").InnerText);
            //chkWorseLiftingLeft.Checked = node.SelectSingleNode("WorseLiftingLeft") == null ? chkWorseLiftingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLiftingLeft").InnerText);
            //chkWorseRotationLeft.Checked = node.SelectSingleNode("WorseRotationLeft") == null ? chkWorseRotationLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRotationLeft").InnerText);
            //chkWorseWorkingLeft.Checked = node.SelectSingleNode("WorseWorkingLeft") == null ? chkWorseWorkingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorkingLeft").InnerText);
            //chkWorseActivitiesLeft.Checked = node.SelectSingleNode("WorseActivitiesLeft") == null ? chkWorseActivitiesLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesLeft").InnerText);
            //chkImprovedRestingLeft.Checked = node.SelectSingleNode("ImprovedRestingLeft") == null ? chkImprovedRestingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingLeft").InnerText);
            //chkImprovedMedicationLeft.Checked = node.SelectSingleNode("ImprovedMedicationLeft") == null ? chkImprovedMedicationLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
            //chkImprovedTherapyLeft.Checked = node.SelectSingleNode("ImprovedTherapyLeft") == null ? chkImprovedTherapyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
            //chkImprovedSleepingLeft.Checked = node.SelectSingleNode("ImprovedSleepingLeft") == null ? chkImprovedSleepingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
            //txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;
            //chkSharpRight.Checked = node.SelectSingleNode("SharpRight") == null ? chkSharpRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpRight").InnerText);
            //chkElectricRight.Checked = node.SelectSingleNode("ElectricRight") == null ? chkElectricRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricRight").InnerText);
            //chkShootingRight.Checked = node.SelectSingleNode("ShootingRight") == null ? chkShootingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingRight").InnerText);
            //chkThrobblingRight.Checked = node.SelectSingleNode("ThrobblingRight") == null ? chkThrobblingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingRight").InnerText);
            //chkPulsatingRight.Checked = node.SelectSingleNode("PulsatingRight") == null ? chkPulsatingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingRight").InnerText);
            //chkDullRight.Checked = node.SelectSingleNode("DullRight") == null ? chkDullRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DullRight").InnerText);
            //chkAchyRight.Checked = node.SelectSingleNode("AchyRight") == null ? chkAchyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyRight").InnerText);
            //chkWorseLyingRight.Checked = node.SelectSingleNode("WorseLyingRight") == null ? chkWorseLyingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingRight").InnerText);
            //chkWorseMovementRight.Checked = node.SelectSingleNode("WorseMovementRight") == null ? chkWorseMovementRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementRight").InnerText);
            //// chkWorseRaisingRight.Checked = node.SelectSingleNode("WorseRaisingRight") == null ? chkWorseRaisingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRaisingRight").InnerText);
            //chkWorseLiftingRight.Checked = node.SelectSingleNode("WorseLiftingRight") == null ? chkWorseLiftingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLiftingRight").InnerText);
            //chkWorseRotationRight.Checked = node.SelectSingleNode("WorseRotationRight") == null ? chkWorseRotationRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRotationRight").InnerText);
            //chkWorseWorkingRight.Checked = node.SelectSingleNode("WorseWorkingRight") == null ? chkWorseWorkingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorkingRight").InnerText);
            //chkWorseActivitiesRight.Checked = node.SelectSingleNode("WorseActivitiesRight") == null ? chkWorseActivitiesRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesRight").InnerText);
            //chkImprovedRestingRight.Checked = node.SelectSingleNode("ImprovedRestingRight") == null ? chkImprovedRestingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingRight").InnerText);
            //chkImprovedMedicationRight.Checked = node.SelectSingleNode("ImprovedMedicationRight") == null ? chkImprovedMedicationRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationRight").InnerText);
            //chkImprovedTherapyRight.Checked = node.SelectSingleNode("ImprovedTherapyRight") == null ? chkImprovedTherapyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyRight").InnerText);
            //chkImprovedSleepingRight.Checked = node.SelectSingleNode("ImprovedSleepingRight") == null ? chkImprovedSleepingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingRight").InnerText);

            //txtAbductionRightWas.Text = node.SelectSingleNode("ShoulderAbdNormal") == null ? txtAbductionRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderAbdNormal").InnerText;
            //txtFlexionRightWas.Text = node.SelectSingleNode("ShoulderFlexNormal") == null ? txtFlexionRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderFlexNormal").InnerText;
            //txtExtRotationRightWas.Text = node.SelectSingleNode("ShoulderExtRotNormal") == null ? txtExtRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderExtRotNormal").InnerText;
            //txtIntRotationRightWas.Text = node.SelectSingleNode("ShoulderIntRot") == null ? txtIntRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderIntRot").InnerText;


            //txtAbductionLeftWas.Text = node.SelectSingleNode("AbductionRight") == null ? txtAbductionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("AbductionRight").InnerText;
            //txtFlexionLeftWas.Text = node.SelectSingleNode("FlexionRight") == null ? txtFlexionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("FlexionRight").InnerText;
            //txtExtRotationLeftWas.Text = node.SelectSingleNode("ExtRotationRight") == null ? txtExtRotationLeftWas.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationRight").InnerText;
            //txtIntRotationLeftWas.Text = node.SelectSingleNode("IntRotationRight") == null ? txtIntRotationLeftWas.Text.ToString().Trim() : node.SelectSingleNode("IntRotationRight").InnerText;

            //txtAbductionLeft.Text = node.SelectSingleNode("AbductionLeft") == null ? txtAbductionLeft.Text.ToString().Trim() : node.SelectSingleNode("AbductionLeft").InnerText;
            //txtFlexionLeft.Text = node.SelectSingleNode("FlexionLeft") == null ? txtFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("FlexionLeft").InnerText;
            //txtExtRotationLeft.Text = node.SelectSingleNode("ExtRotationLeft") == null ? txtExtRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationLeft").InnerText;
            //txtIntRotationLeft.Text = node.SelectSingleNode("IntRotationLeft") == null ? txtIntRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("IntRotationLeft").InnerText;

            //txtPalpationText1Left.Text = node.SelectSingleNode("PalpationText1Left") == null ? txtPalpationText1Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Left").InnerText;
            //// txtPalpationText2Left.Text = node.SelectSingleNode("PalpationText2Left") == null ? txtPalpationText2Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Left").InnerText;

            //chkACJointLeft.Checked = node.SelectSingleNode("ACJointLeft") == null ? chkACJointLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ACJointLeft").InnerText);
            //chkGlenohumeralLeft.Checked = node.SelectSingleNode("GlenohumeralLeft") == null ? chkGlenohumeralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("GlenohumeralLeft").InnerText);
            //chkCorticoidLeft.Checked = node.SelectSingleNode("CorticoidLeft") == null ? chkCorticoidLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("CorticoidLeft").InnerText);
            //chkSupraspinatusLeft.Checked = node.SelectSingleNode("SupraspinatusLeft") == null ? chkSupraspinatusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SupraspinatusLeft").InnerText);
            //chkScapularLeft.Checked = node.SelectSingleNode("ScapularLeft") == null ? chkScapularLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapularLeft").InnerText);
            //chkDeepLabralLeft.Checked = node.SelectSingleNode("DeepLabralLeft") == null ? chkDeepLabralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DeepLabralLeft").InnerText);
            //chkDeltoidLeft.Checked = node.SelectSingleNode("DeltoidLeft") == null ? chkDeltoidLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DeltoidLeft").InnerText);
            //chkTrapeziusLeft.Checked = node.SelectSingleNode("TrapeziusLeft") == null ? chkTrapeziusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("TrapeziusLeft").InnerText);
            //chkEccymosisLeft.Checked = node.SelectSingleNode("EccymosisLeft") == null ? chkEccymosisLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("EccymosisLeft").InnerText);
            //chkEdemaLeft.Checked = node.SelectSingleNode("EdemaLeft") == null ? chkEdemaLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("EdemaLeft").InnerText);
            //chkRangeOfMotionLeft.Checked = node.SelectSingleNode("RangeOfMotionLeft") == null ? chkRangeOfMotionLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("RangeOfMotionLeft").InnerText);
            //txtPalpationText1Right.Text = node.SelectSingleNode("PalpationText1Right") == null ? txtPalpationText1Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Right").InnerText;
            //// txtPalpationText2Right.Text = node.SelectSingleNode("PalpationText2Right") == null ? txtPalpationText2Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Right").InnerText;
            //chkACJointRight.Checked = node.SelectSingleNode("ACJointRight") == null ? chkACJointRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ACJointRight").InnerText);
            //chkGlenohumeralRight.Checked = node.SelectSingleNode("GlenohumeralRight") == null ? chkGlenohumeralRight.Checked : Convert.ToBoolean(node.SelectSingleNode("GlenohumeralRight").InnerText);
            //chkCorticoidRight.Checked = node.SelectSingleNode("CorticoidRight") == null ? chkCorticoidRight.Checked : Convert.ToBoolean(node.SelectSingleNode("CorticoidRight").InnerText);
            //chkSupraspinatusRight.Checked = node.SelectSingleNode("SupraspinatusRight") == null ? chkSupraspinatusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SupraspinatusRight").InnerText);
            //chkScapularRight.Checked = node.SelectSingleNode("ScapularRight") == null ? chkScapularRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapularRight").InnerText);
            //chkDeepLabralRight.Checked = node.SelectSingleNode("DeepLabralRight") == null ? chkDeepLabralRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DeepLabralRight").InnerText);
            //chkDeltoidRight.Checked = node.SelectSingleNode("DeltoidRight") == null ? chkDeltoidRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DeltoidRight").InnerText);
            //chkTrapeziusRight.Checked = node.SelectSingleNode("TrapeziusRight") == null ? chkTrapeziusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("TrapeziusRight").InnerText);
            //chkEccymosisRight.Checked = node.SelectSingleNode("EccymosisRight") == null ? chkEccymosisRight.Checked : Convert.ToBoolean(node.SelectSingleNode("EccymosisRight").InnerText);
            //chkEdemaRight.Checked = node.SelectSingleNode("EdemaRight") == null ? chkEdemaRight.Checked : Convert.ToBoolean(node.SelectSingleNode("EdemaRight").InnerText);
            //chkRangeOfMotionRight.Checked = node.SelectSingleNode("RangeOfMotionRight") == null ? chkRangeOfMotionRight.Checked : Convert.ToBoolean(node.SelectSingleNode("RangeOfMotionRight").InnerText);
            //chkNeerLeft.Checked = node.SelectSingleNode("NeerLeft") == null ? chkNeerLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("NeerLeft").InnerText);
            //chkHawkinLeft.Checked = node.SelectSingleNode("HawkinLeft") == null ? chkHawkinLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("HawkinLeft").InnerText);
            //chkYergasonsLeft.Checked = node.SelectSingleNode("YergasonsLeft") == null ? chkYergasonsLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("YergasonsLeft").InnerText);
            //chkDropArmLeft.Checked = node.SelectSingleNode("DropArmLeft") == null ? chkDropArmLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DropArmLeft").InnerText);
            //chkReverseBeerLeft.Checked = node.SelectSingleNode("ReverseBeerLeft") == null ? chkReverseBeerLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ReverseBeerLeft").InnerText);
            //chkNeerRight.Checked = node.SelectSingleNode("NeerRight") == null ? chkNeerRight.Checked : Convert.ToBoolean(node.SelectSingleNode("NeerRight").InnerText);
            //chkHawkinRight.Checked = node.SelectSingleNode("HawkinRight") == null ? chkHawkinRight.Checked : Convert.ToBoolean(node.SelectSingleNode("HawkinRight").InnerText);
            //chkYergasonsRight.Checked = node.SelectSingleNode("YergasonsRight") == null ? chkYergasonsRight.Checked : Convert.ToBoolean(node.SelectSingleNode("YergasonsRight").InnerText);
            //chkDropArmRight.Checked = node.SelectSingleNode("DropArmRight") == null ? chkDropArmRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DropArmRight").InnerText);
            //chkReverseBeerRight.Checked = node.SelectSingleNode("ReverseBeerRight") == null ? chkReverseBeerRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ReverseBeerRight").InnerText);

            //cboTPSide1.Text = node.SelectSingleNode(pos + "/TPSide1") == null ? cboTPSide1.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide1").InnerText;
            //txtTPText1.Text = node.SelectSingleNode(pos + "/TPText1") == null ? txtTPText1.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText1").InnerText;
            //cboTPSide2.Text = node.SelectSingleNode(pos + "/TPSide2") == null ? cboTPSide2.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide2").InnerText;
            //txtTPText2.Text = node.SelectSingleNode(pos + "/TPText2") == null ? txtTPText2.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText2").InnerText;
            //cboTPSide3.Text = node.SelectSingleNode(pos + "/TPSide3") == null ? cboTPSide3.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide3").InnerText;
            //txtTPText3.Text = node.SelectSingleNode(pos + "/TPText3") == null ? txtTPText3.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText3").InnerText;
            //cboTPSide4.Text = node.SelectSingleNode(pos + "/TPSide4") == null ? cboTPSide4.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide4").InnerText;
            //txtTPText4.Text = node.SelectSingleNode(pos + "/TPText4") == null ? txtTPText4.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText4").InnerText;
            //cboTPSide5.Text = node.SelectSingleNode(pos + "/TPSide5") == null ? cboTPSide5.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide5").InnerText;
            //txtTPText5.Text = node.SelectSingleNode(pos + "/TPText5") == null ? txtTPText5.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText5").InnerText;
            //cboTPSide6.Text = node.SelectSingleNode(pos + "/TPSide6") == null ? cboTPSide6.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide6").InnerText;
            //txtTPText6.Text = node.SelectSingleNode(pos + "/TPText6") == null ? txtTPText6.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText6").InnerText;
            //cboTPSide7.Text = node.SelectSingleNode(pos + "/TPSide7") == null ? cboTPSide7.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide7").InnerText;
            //txtTPText7.Text = node.SelectSingleNode(pos + "/TPText7") == null ? txtTPText7.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText7").InnerText;
            //cboTPSide8.Text = node.SelectSingleNode(pos + "/TPSide8") == null ? cboTPSide8.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide8").InnerText;

            //txtTPText8.Text = node.SelectSingleNode("TPText8") == null ? txtTPText8.Text.ToString().Trim() : node.SelectSingleNode("TPText8").InnerText;
            // txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            //txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            //cboSprainStrainSide.Text = node.SelectSingleNode("SprainStrainSide") == null ? cboSprainStrainSide.Text.ToString().Trim() : node.SelectSingleNode("SprainStrainSide").InnerText;
            //chkSprainStrain.Checked = node.SelectSingleNode("SprainStrain") == null ? chkSprainStrain.Checked : Convert.ToBoolean(node.SelectSingleNode("SprainStrain").InnerText);
            //cboDerangmentSide.Text = node.SelectSingleNode("DerangmentSide") == null ? cboDerangmentSide.Text.ToString().Trim() : node.SelectSingleNode("DerangmentSide").InnerText;
            //chkDerangment.Checked = node.SelectSingleNode("Derangment") == null ? chkDerangment.Checked : Convert.ToBoolean(node.SelectSingleNode("Derangment").InnerText);
            //cboSyndromeSide.Text = node.SelectSingleNode("SyndromeSide") == null ? cboSyndromeSide.Text.ToString().Trim() : node.SelectSingleNode("SyndromeSide").InnerText;
            //chkSyndrome.Checked = node.SelectSingleNode("Syndrome") == null ? chkSyndrome.Checked : Convert.ToBoolean(node.SelectSingleNode("Syndrome").InnerText);
            //chkPlan.Checked = node.SelectSingleNode("Plan") == null ? chkPlan.Checked : Convert.ToBoolean(node.SelectSingleNode("Plan").InnerText);
            //  chkElectricLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkElectricLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            //cboScanType.Text = node.SelectSingleNode("ScanType") == null ? cboScanType.Text.ToString().Trim() : node.SelectSingleNode("ScanType").InnerText;
            //cboScanSide.Text = node.SelectSingleNode("ScanSide") == null ? cboScanSide.Text.ToString().Trim() : node.SelectSingleNode("ScanSide").InnerText;
            _fldPop = false;
        }
    }
    public void BindDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Standards = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' Order By BodyPart,Heading";
            SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " and PatientFU_ID is null  AND BodyPart = '" + _CurBP + "'  and IsConsidered=0 Order By BodyPart,Heading";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Standards);
            dgvStandards.DataSource = "";
            dgvStandards.DataSource = Standards.DefaultView;
            dgvStandards.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }
    public string SaveStandards(string ieID)
    {

        string ids = string.Empty;
        try
        {
            foreach (GridViewRow row in dgvStandards.Rows)
            {
                string Procedure_ID, MCODE, BodyPart, Heading, CCDesc, PEDesc, ADesc, PDesc;

                Procedure_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                Heading = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PDesc = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;

                ids += Session["PatientIE_ID"].ToString() + ",";
                SaveStdUI(ieID, Procedure_ID, Heading, PDesc);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Standard(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }
    public void SaveStdUI(string ieID, string iStdID, string heading, string pdesc)
    {
        string[] _Type = iStdID.Split('_');
        int _StdID = Convert.ToInt32(_Type[0]);
        string Part = Convert.ToString(_Type[1]);

        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        //long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + ieID + " AND ProcedureDetail_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        //if (sqlTbl.Rows.Count == 0 && StdChecked == true)
        //    _ieMode = "New";
        //else if (sqlTbl.Rows.Count == 0 && StdChecked == false)
        //    _ieMode = "None";
        //else if (sqlTbl.Rows.Count > 0 && StdChecked == false)
        //    _ieMode = "Delete";
        //else
        _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["ProcedureDetail_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;

            if (Part.Equals("R"))
            {
                TblRow["Heading"] = heading.ToString().Trim();
                TblRow["PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("S"))
            {
                TblRow["S_Heading"] = heading.ToString().Trim();
                TblRow["S_PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("E"))
            {
                TblRow["E_Heading"] = heading.ToString().Trim();
                TblRow["E_PDesc"] = pdesc.ToString().Trim();
            }

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }

    private void Page_Loaded_1(object sender, EventArgs e) //RoutedEventArgs 
    {
        PopulateStrightFwd();
    }

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        //SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        //Response.Redirect("AddDiagnosis.aspx");
    }

    private void AddStd_Click(object sender, EventArgs e) //RoutedEventArgs e
    {

        BindDataGrid();

    }
    public string SaveDiagnosis(string ieID)
    {
        string ids = string.Empty;
        try
        {
            RemoveDiagCodesDetail(ieID);
            string codeId = "", codes = "", desc = "";
            foreach (GridViewRow row in dgvDiagCodes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string Description, DiagCode, DiagCode_ID;

                    DiagCode_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                    //        DiagCodeDetail_ID = row.Cells[2].Controls.OfType<HiddenField>().FirstOrDefault().Value;

                    Description = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    DiagCode = row.Cells[0].Controls.OfType<TextBox>().FirstOrDefault().Text;

                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    //if (isChecked)
                    //{
                    //    //ids += DiagCode_ID + ",";
                    //    SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    //}
                    if (isChecked)
                    {
                        //ids += DiagCode_ID + ",";
                        codeId = codeId + "@" + DiagCode_ID;
                        codes = codes + "@" + DiagCode;
                        desc = desc + "@" + Description;
                        // SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    }
                }
            }
            gDbhelperobj.SaveDiagUI(ieID, null, codeId, true, _CurBP, desc, codes);
            BindDCDataGrid();
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Diagnosis Code(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }
    public void SaveDiagUI(string ieID, string iDiagID, bool DiagChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["Diag_Master_ID"] = _DiagID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }
    public void BindDCDataGrid()
    {
        _CurIEid = Session["PatientIE_ID"].ToString();
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        try
        {
            if (!IsPostBack)
            {
                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientIE_ID = " + _CurIEid + " and PatientFU_ID is null AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
                oSQLCmd.Connection = oSQLConn;
                oSQLCmd.CommandText = SqlStr;
                oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
                oSQLAdpr.Fill(Diagnosis);
                dgvDiagCodes.DataSource = "";
                dgvDiagCodes.DataSource = Diagnosis.DefaultView;
                dgvDiagCodes.DataBind();
                oSQLAdpr.Dispose();
                oSQLConn.Close();
            }
            else
            {
                if (ViewState["DiagnosisList"] != null)
                {
                    List<Adddiagnosis> objList = (List<Adddiagnosis>)ViewState["DiagnosisList"];

                    dgvDiagCodes.DataSource = objList;
                    dgvDiagCodes.DataBind();
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        if (ViewState["saveDaigno"].ToString() == "1")
            SaveDiagnosis(Session["PatientIE_ID"].ToString());
        SaveUI(Session["PatientIE_ID"].ToString(), ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["PatientIE_ID"].ToString());
    }


    private void bindgridPoup()
    {
        try
        {
            string _CurBodyPart = _CurBP;
            //string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";

            //DataSet ds = new DataSet();
            //DataTable Standards = new DataTable();
            //string SqlStr = "";
            //if (_CurIEid != "")
            //    SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS(" + _CurIEid + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            //else
            //    SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTS('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            //ds = gDbhelperobj.selectData(SqlStr);

            SqlParameter[] param = new SqlParameter[4];

            param[0] = new SqlParameter("@bPart", _CurBodyPart);
            param[1] = new SqlParameter("@PatientIE_ID", _CurIEid);
            param[2] = new SqlParameter("@PatientFU_ID", 0);
            param[3] = new SqlParameter("@cnd", txDesc.Text.Trim());

            DataSet ds = new DBHelperClass().executeSelectSP("GetDaignoCodesIE", param);


            DataTable newTable = new DataTable();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (Request.QueryString["P"].ToLower() == "l")
                    newTable = ds.Tables[0].Select(" Description like '%left%' ").CopyToDataTable();
                else if (Request.QueryString["P"].ToLower() == "r")
                    newTable = ds.Tables[0].Select(" Description like '%right%' ").CopyToDataTable();

                dgvDiagCodesPopup.DataSource = newTable;
                dgvDiagCodesPopup.DataBind();
            }

            if (newTable != null && newTable.Rows.Count > 0)
            {
                dgvDiagCodesPopup.DataSource = newTable;
                dgvDiagCodesPopup.DataBind();
            }
            else
            {
                dgvDiagCodesPopup.DataSource = ds;
                dgvDiagCodesPopup.DataBind();
            }

        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        ViewState["saveDaigno"] = "1";
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDCDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "Test", "closeModelPopup()", true);
    }

    public string SaveStandardsPopup(string ieID)
    {
        List<Adddiagnosis> objList = new List<Adddiagnosis>();
        Adddiagnosis obj = new Adddiagnosis();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvDiagCodesPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    obj = new Adddiagnosis();
                    obj.Diag_Master_ID = dgvDiagCodesPopup.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.DiagCode = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Description = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        ids += obj.DiagCode_ID + ",";
                        //  SaveStdUI(ieID, obj.DiagCode_ID, true, obj.BodyPart, obj.Description, obj.DiagCode);
                        objList.Add(obj);
                    }
                    //else
                    //{ SaveStdUI(ieID, obj.DiagCode_ID, false, obj.BodyPart, obj.Description, obj.DiagCode); }

                }
            }
            ViewState["DiagnosisList"] = objList;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }

    public void SaveStdUI(string ieID, string iDiagID, bool DiagIsChecked, string bp, string dcd, string dc)
    {
        try
        {
            string _ieMode = "";
            long _ieID = Convert.ToInt64(ieID);
            long _DiagID = Convert.ToInt64(iDiagID);
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID;
            SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
            SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
            DataTable sqlTbl = new DataTable();
            sqlAdapt.Fill(sqlTbl);
            DataRow TblRow;

            if (sqlTbl.Rows.Count == 0 && DiagIsChecked == true)
                _ieMode = "New";
            else if (sqlTbl.Rows.Count == 0 && DiagIsChecked == false)
                _ieMode = "None";
            else if (sqlTbl.Rows.Count > 0 && DiagIsChecked == false)
                _ieMode = "Delete";
            else
                _ieMode = "Update";

            if (_ieMode == "New")
                TblRow = sqlTbl.NewRow();
            else if (_ieMode == "Update" || _ieMode == "Delete")
            {
                TblRow = sqlTbl.Rows[0];
                TblRow.AcceptChanges();
            }
            else
                TblRow = null;

            if (_ieMode == "Update" || _ieMode == "New")
            {
                TblRow["Diag_Master_ID"] = _DiagID;
                TblRow["PatientIE_ID"] = _ieID;
                TblRow["BodyPart"] = bp.ToString().Trim();
                TblRow["DiagCode"] = dc.ToString().Trim();
                TblRow["Description"] = dcd.ToString().Trim();

                if (_ieMode == "New")
                {
                    TblRow["CreatedBy"] = "Admin";
                    TblRow["CreatedDate"] = DateTime.Now;
                    sqlTbl.Rows.Add(TblRow);
                }
                sqlAdapt.Update(sqlTbl);
            }
            else if (_ieMode == "Delete")
            {
                TblRow.Delete();
                sqlAdapt.Update(sqlTbl);
            }
            if (TblRow != null)
                TblRow.Table.Dispose();
            sqlTbl.Dispose();
            sqlCmdBuilder.Dispose();
            sqlAdapt.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindgridPoup();
    }

    protected void RemoveDiagCodesDetail(string PatientIE_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblDiagCodesDetail WHERE PatientIE_ID=" + PatientIE_ID + " and BodyPart like '%" + _CurBP + "%'";
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }

    public int checkTP()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        int val = 0;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        // cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Shoulder");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            if (isTP == false)
                val = 0;
            else
                val = 1;

        }

        return val;

    }

    public void bindCC(string p)
    {
        string path = Server.MapPath("~/Template/ShoulderCC.html");
        string body = File.ReadAllText(path);

        //if (p == "left")
        //    body = body.Replace("#rigthtdiv", "style='display:none'");
        //else if (p == "right")
        //    body = body.Replace("#leftdiv", "style='display:none'");

        var gender = Session["Gender"].ToString().ToLower() == "mr." ? "He" : "She";

        body = body.Replace("#gender", gender);

        hdorgCC.Value = body;
        CF.InnerHtml = body;

    }

    public void bindPE(string p)
    {
        string path = Server.MapPath("~/Template/ShoulderPE.html");
        string body = File.ReadAllText(path);

        if (p == "left")
            p = "l";
        else if (p == "right")
            p = "r";
        else
            p = "b";


        divPE.InnerHtml = body;

        hdorgPE.Value = body;

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sideFun", "displaySide('" + p + "')", true);

    }

    protected void BindROM()
    {


        long _ieID = Convert.ToInt64(Session["PatientIE_ID"]);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpShoulder WHERE PatientIE_ID = " + _ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        oSQLConn.Close();
        if (sqlTbl.Rows.Count > 0)
        {
            //if (Request["P"] == "R")
            //{
            //    this.removeROM("LeftROM", sqlTbl.Rows[0]["PatientDetail_ID"].ToString(), sqlTbl.Rows[0]["NameROM"].ToString());
            //}
            //else if (Request["P"] == "L")
            //{
            //    this.removeROM("RightROM", sqlTbl.Rows[0]["PatientDetail_ID"].ToString(), sqlTbl.Rows[0]["NameROM"].ToString());
            //}


            string[] strname, strleft, strright, strnormal;
            if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
            {
                strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
                strleft = sqlTbl.Rows[0]["LeftROM"].ToString().Split(',');
                strright = sqlTbl.Rows[0]["RightROM"].ToString().Split(',');
                strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


                // Create the Table
                DataTable OrdersTable = new DataTable("ROM");
                // Build the Orders schema
                OrdersTable.Columns.Add("name", Type.GetType("System.String"));
                OrdersTable.Columns.Add("left", Type.GetType("System.String"));
                OrdersTable.Columns.Add("right", Type.GetType("System.String"));
                OrdersTable.Columns.Add("normal", Type.GetType("System.String"));

                DataRow workRow;

                for (int i = 0; i < strname.Length; i++)
                {

                    workRow = OrdersTable.NewRow();
                    workRow[0] = strname[i];
                    workRow[1] = strleft[i];
                    workRow[2] = strright[i];
                    workRow[3] = strnormal[i];
                    OrdersTable.Rows.Add(workRow);
                }

                if (OrdersTable.Rows.Count != 0)
                {
                    repROM.DataSource = OrdersTable;
                    repROM.DataBind();
                }
            }
            else
                getXMLROMvalue();
        }
        else
        {
            getXMLROMvalue();
        }
    }

    private void getXMLROMvalue()
    {
        //open the tender xml file  
        XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Shoulder.xml"));
        //reading the xml data  
        DataSet ds = new DataSet();
        ds.ReadXml(xmlreader);
        xmlreader.Close();
        //if ds is not empty  
        if (ds.Tables.Count != 0)
        {
            repROM.DataSource = ds;
            repROM.DataBind();
        }
    }

    protected void repROM_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Request["P"] != null)
            {
                if (Request["P"] == "R")
                {
                    TextBox txtleft = e.Item.FindControl("txtleft") as TextBox;
                    txtleft.ReadOnly = true;
                }
                else if (Request["P"] == "L")
                {
                    TextBox txtright = e.Item.FindControl("txtright") as TextBox;
                    txtright.ReadOnly = true;
                }
            }
        }
    }


    protected void chkRemove_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["saveDaigno"] = "1";
    }


}