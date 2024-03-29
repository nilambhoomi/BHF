﻿using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Templatestorepdf : System.Web.UI.Page
{
    DBHelperClass db = new DBHelperClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            LoadPatientIE("", 1);

        }
        if (!this.IsPostBack)
        {
            DirectoryInfo rootInfo = new DirectoryInfo(Server.MapPath("~/TemplateStore/"));
            this.PopulateTreeView(rootInfo, null);
        }
    }
    protected void btnPNS_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/PNS.aspx");
    }
    private void PopulateTreeView(DirectoryInfo dirInfo, TreeNode treeNode)
    {
        foreach (DirectoryInfo directory in dirInfo.GetDirectories())
        {
            TreeNode directoryNode = new TreeNode
            {
                Text = directory.Name,
                Value = directory.FullName
            };

            if (treeNode == null)
            {
                //If Root Node, add to TreeView.
                TreeView1.Nodes.Add(directoryNode);
            }
            else
            {
                //If Child Node, add to Parent Node.

                treeNode.ChildNodes.Add(directoryNode);

            }

            //Get all files in the Directory.
            foreach (FileInfo file in directory.GetFiles())
            {
                if (Session["reportAccess"] != null)
                {
                    if (Session["reportAccess"].ToString().ToLower().Contains(file.Name.ToLower()))
                    {
                        //Add each file as Child Node.
                        TreeNode fileNode = new TreeNode
                        {
                            Text = file.Name,
                            Value = file.FullName,
                            ShowCheckBox = true
                            //Target = "_blank",
                            //  NavigateUrl = (new Uri(Server.MapPath("~/"))).MakeRelativeUri(new Uri(file.FullName)).ToString()

                        };
                        //ShowCheckBox = true
                        fileNode.PopulateOnDemand = true;
                        // Set additional properties for the node.
                        fileNode.SelectAction = TreeNodeSelectAction.Expand;


                        directoryNode.ChildNodes.Add(fileNode);
                    }
                }
            }

            PopulateTreeView(directory, directoryNode);
        }
    }
    private void LoadPatientIE(string query, int pageindex)
    {
        try
        {
            int totalcount;
            DataSet dt = new DataSet();

            dt = db.PatientIE_getAll(query, pageindex, 10, out totalcount);
            if (dt.Tables[0].Rows.Count > 0)
            {
                rpview.DataSource = dt;
                rpview.DataBind();
            }
            else
            {
                rpview.DataSource = null;
                rpview.DataBind();
            }
            PopulatePager(totalcount, pageindex);
            //lblcount.Text = totalcount.ToString();
        }
        catch (Exception ex)
        {
        }
    }
    private void PopulatePager(int recordCount, int currentPage)
    {
        List<ListItem> pages = new List<ListItem>();
        int startIndex, endIndex;
        int pagerSpan = 5;

        //Calculate the Start and End Index of pages to be displayed.
        double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(10));
        int pageCount = (int)Math.Ceiling(dblPageCount);

        startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
        endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
        if (currentPage > pagerSpan % 2)
        {
            if (currentPage == 2)
            {
                endIndex = 5;
            }
            else
            {
                endIndex = currentPage + 2;
            }
        }
        else
        {
            endIndex = (pagerSpan - currentPage) + 1;
        }

        if (endIndex - (pagerSpan - 1) > startIndex)
        {
            startIndex = endIndex - (pagerSpan - 1);
        }

        if (endIndex > pageCount)
        {
            endIndex = pageCount;
            startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
        }

        //Add the First Page Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("First", "1"));
        }

        //Add the Previous Button.
        if (currentPage > 1)
        {
            pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
        }

        //Add the Next Button.
        if (currentPage < pageCount)
        {
            pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
        }

        //Add the Last Button.
        if (currentPage != pageCount)
        {
            pages.Add(new ListItem("Last", pageCount.ToString()));
        }

        if (recordCount > 0)
        {
            lbl_page_no.InnerText = currentPage.ToString();
            lbl_total_page.InnerText = pageCount.ToString();


            rptPager.DataSource = pages;
            rptPager.DataBind();
        }
        else
        {
            div_page.Style.Add("display", "none");
            rptPager.DataSource = null;
            rptPager.DataBind();
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);

        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", pageIndex);
        }
        else
            this.LoadPatientIE("", pageIndex);
    }
    protected void lnk_openIE_Click(object sender, EventArgs e)
    {
        LinkButton btn = sender as LinkButton;
        Label parentname = new Label();
        Label childfilename = new Label();
        Label names = new Label();
        Label fpname = new Label();
        Label lpname = new Label();

        if (TreeView1.CheckedNodes.Count <= 0)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;

        }
        else if (TreeView1.CheckedNodes.Count >= 2)
        {
            // lblMessage.ImageUrl = Server.MapPath("~/img/select one.gif");
            //lblMessage.Visible = true;
        }
        else if (TreeView1.CheckedNodes.Count == 1)
        {

            if (TreeView1.CheckedNodes.Count > 0 && TreeView1.CheckedNodes.Count < 2)
            {

                foreach (TreeNode node in TreeView1.CheckedNodes)
                {
                    parentname.Text = node.Parent.Text;
                    childfilename.Text = node.Text;
                }
                string name = childfilename.Text;
                if (string.IsNullOrWhiteSpace(childfilename.Text))
                { }
                else
                {
                    Session["filename"] = childfilename.Text;
                }
                bindEditData(btn.CommandArgument);
                var pdfPath = Path.Combine(Server.MapPath("~/TemplateStore\\" + parentname.Text + "\\" + childfilename.Text));

                #region Generate pdf code
                string partialName = Convert.ToString(btn.CommandArgument);
                DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(Server.MapPath("~/Sign/"));
                FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(partialName + "*.*");
                string fullName = string.Empty;
                foreach (FileInfo foundFile in filesInDir)
                {
                    fullName = foundFile.FullName;
                }
                if (!string.IsNullOrEmpty(fullName))
                {
                    if (childfilename.Text.Contains("NF packet.pdf"))
                    {
                        float[] xaxis;
                        float[] yaxis;
                        string newfilename = childfilename.Text;
                        if (Session["sex"].ToString() == "Mr.")
                        {
                            xaxis = new float[] { 0f, 170f, 100f, 350f, 70f, 40f, 0f,150f,0f,0f,0f };
                            yaxis = new float[] { 0f, 540f, 325f, 310f, 250f, 80f, 0f,500f,0f,0f,0f  };
                        }
                        else
                        {
                            xaxis = new float[] { 0f, 170f, 100f, 350f, 70f, 40f, 0f,150f,0f,0f,0f };
                            yaxis = new float[] { 0f, 540f, 325f, 310f, 250f, 80f, 0f,500f,0f,0f,0f  };
                        }

                        string imagepath = fullName;
                        setPDF(pdfPath, newfilename, imagepath, xaxis, yaxis);
                        pdfPath = Server.MapPath("~/PdfForms/" + newfilename);
                    }
                    
                    if (childfilename.Text.Contains("WC packet.pdf"))
                    {
                        float[] xaxis;
                        float[] yaxis;
                        string newfilename = childfilename.Text;
                        if (Session["sex"].ToString() == "Mr.")
                        {
                             xaxis = new float[] { 0f, 170f, 100f, 120f, 70f, 0f, 150f,120f,0f,0f,0f,120f,0f,0f,0f,0f };
                            yaxis = new float[] { 0f, 540f, 325f, 80f, 250f, 0f, 500f,110f,0f,0f,0f,100f,0f,0f,0f,0f  };
                        }
                        else
                        {
                              xaxis = new float[] { 0f, 170f, 100f, 0f, 70f, 0f, 150f,0f,0f,0f,0f,0f,0f,0f,0f,0f };
                            yaxis = new float[] { 0f, 540f, 325f, 0f, 250f, 0f, 500f,0f,0f,0f,0f,0f,0f,0f,0f,0f  };
                        }
                        string imagepath = fullName;
                        setPDF(pdfPath, newfilename, imagepath, xaxis, yaxis);
                        pdfPath = Server.MapPath("~/PdfForms/" + newfilename);
                    }
                }
                #endregion

                names.Text = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                fpname.Text = Convert.ToString(Session["fname"]);
                lpname.Text = Convert.ToString(Session["lname"]);
                var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);
                if (!string.IsNullOrEmpty(txtproc_code_date.Text))
                {
                    formFieldMap["txt_date"] = txtproc_code_date.Text;
                }
                else
                {
                    formFieldMap["txt_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                }
                formFieldMap["txt_todDate"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                formFieldMap["txt_name"] = names.Text;
                formFieldMap["txt_eMail"] = Convert.ToString(Session["eMail"]);
                formFieldMap["txt_city"] = Convert.ToString(Session["city"]);
                formFieldMap["txt_Inscity"] = Convert.ToString(Session["Inscity"]);
                formFieldMap["txt_state"] = Convert.ToString(Session["state"]);
                formFieldMap["txt_Insstate"] = Convert.ToString(Session["Insstate"]);
                formFieldMap["txt_zip"] = Convert.ToString(Session["zip"]);
                formFieldMap["txt_Inszip"] = Convert.ToString(Session["Inszip"]);
                 formFieldMap["Textdefaultasneeded"] = "As needed";
                
                if (Session["Phone"] != null)
                    if (Session["Phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_Phone2"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_Phone2"] = Session["Phone"].ToString();
                    }
                if (Session["work_phone"] != null)
                    if (Session["work_phone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_work_phone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_work_phone"] = Session["work_phone"].ToString();
                    }
                if (Session["InsPhone"] != null)
                    if (Session["InsPhone"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_InsPhone"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_InsPhone"] = Session["InsPhone"].ToString();
                    }
                if (Session["ssn"] != null)
                    if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_ssn"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_ssn"] = Session["ssn"].ToString();
                        //formFieldMap["txt_ssn"] = "";
                    }
                formFieldMap["txt_InsCo"] = Convert.ToString(Session["InsCo"]);
                formFieldMap["txt_ClaimNumber"] = Convert.ToString(Session["ClaimNumber"]);
                formFieldMap["txt_admitting_surgeon"] = "Billy Ford, MD";
                formFieldMap["txt_admitting_surgeon_ppc"] = "Billy Ford, MD";
                formFieldMap["txt_contact_persion_at_clinic"] = "Keilanya Pierre";
                formFieldMap["txt_phnodr"] = "516-984-7834";
                formFieldMap["txt_Referring_Physician_Phone"] = "516-984-7834";
                formFieldMap["txt_H_C_Provider_Name"] = "Billy Ford, MD";
                formFieldMap["txt_License_State_Of"] = " New York";
                formFieldMap["txt_License_Number"] = "243182";
                formFieldMap["chk_2"] = "true";
                //formFieldMap["chk_2"] = "Checked";
                formFieldMap["txt_Referring_Clinic"] = Convert.ToString(Session["LocationPdf"]);
                formFieldMap["txt_Referring_Physician"] = "Billy Ford, MD";
                formFieldMap["txt_Referring_Physician_ppc"] = "Billy Ford, MD";
                formFieldMap["txt_phnodrppc"] = "516-984-7834";
                formFieldMap["txt_c_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_c_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_fname"] = Convert.ToString(Session["fname"]);
                formFieldMap["txt_mname"] = Convert.ToString(Session["mname"]);
                formFieldMap["txt_lname"] = Convert.ToString(Session["lname"]);
                formFieldMap["txt_landfname"] = Convert.ToString(Session["lname"]) + "," + " " + Convert.ToString(Session["fname"]);



                formFieldMap["txt_address"] = Convert.ToString(Session["Address"]);

                formFieldMap["txt_addressCityStateZip"] = (!string.IsNullOrEmpty(Convert.ToString(Session["Address"])) ? Convert.ToString(Session["Address"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["city"])) ? " ," + Convert.ToString(Session["city"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["state"])) ? " ," + Convert.ToString(Session["state"]) : string.Empty) + (!string.IsNullOrEmpty(Convert.ToString(Session["zip"])) ? " ," + Convert.ToString(Session["zip"]) : string.Empty);
                formFieldMap["txt_Insaddress"] = Convert.ToString(Session["InsAddress"]);
                formFieldMap["txt_LocAdd"] = Convert.ToString(Session["LocAdd"]);
                if (Session["WCBGroup"] != null)
                    if (string.IsNullOrWhiteSpace(Session["WCBGroup"].ToString()))
                    {
                        formFieldMap["txt_WCBGroup"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_WCBGroup"] = Session["WCBGroup"].ToString();
                    }
                if (string.IsNullOrWhiteSpace(Session["AGE"].ToString()))
                {
                    formFieldMap["txt_age"] = "";
                }
                else
                {
                    formFieldMap["txt_age"] = Convert.ToString(Session["AGE"]);
                }

                if (Session["mob"] != null)
                    if (Session["mob"].ToString().Split().Last().All(char.IsDigit))
                    {
                        formFieldMap["txt_mob"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_mob"] = Session["mob"].ToString();
                    }
                if (Session["dob"] != null)
                    if (string.IsNullOrWhiteSpace(Session["dob"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_dob"] = Session["dob"].ToString();
                        DateTime dob;
                        if (Session["dob"] != null && DateTime.TryParseExact(Session["dob"].ToString(), "MM-dd-yyyy", null, DateTimeStyles.None, out dob))
                        {

                            formFieldMap["txtdaydob"] = Convert.ToString(dob.Day);
                            formFieldMap["txtmonthdob"] = Convert.ToString(dob.Month);
                            formFieldMap["txtyeardob"] = Convert.ToString(dob.Year);
                        }
                    }
                if (Session["Attorney"] != null)
                    if (string.IsNullOrWhiteSpace(Session["Attorney"].ToString()))
                    {
                        formFieldMap["txt_attorney"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorney"] = Session["Attorney"].ToString();
                    }
                if (Session["AttorneyPhno"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyPhno"].ToString()))
                    {
                        formFieldMap["txt_attorneyPhno"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyPhno"] = Session["AttorneyPhno"].ToString();
                    }
                if (Session["AttorneyAdd"] != null)
                    if (string.IsNullOrWhiteSpace(Session["AttorneyAdd"].ToString()))
                    {
                        formFieldMap["txt_attorneyAdd"] = "";
                    }
                    else
                    {
                        formFieldMap["txt_attorneyAdd"] = Session["AttorneyAdd"].ToString();
                    }

                if (Session["Adjuster"] != null && Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 1)
                {
                    formFieldMap["txtAdjuster"] = Convert.ToString(Session["Adjuster"]).Split('~')[0];
                    if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 2)
                    {
                        formFieldMap["txtAdjusterph"] = Convert.ToString(Session["Adjuster"]).Split('~')[1];
                        if (Convert.ToString(Session["Adjuster"]).Split('~').Count() >= 3)
                        { formFieldMap["txtAdjusterext"] = Convert.ToString(Session["Adjuster"]).Split('~')[2]; }
                    }
                }

                formFieldMap["txt_policy_no"] = Convert.ToString(Session["policy_no"]);

                formFieldMap["txt_c_dob"] = Convert.ToString(Session["dob"]);
                formFieldMap["txt_c_name"] = Convert.ToString(Session["fname"]) + " " + Convert.ToString(Session["lname"]);
                formFieldMap["txt_claim_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                formFieldMap["txt_claim_dateDay"] = Convert.ToString(System.DateTime.Now.ToString("dd"));
                formFieldMap["txt_claim_dateMonth"] = Convert.ToString(System.DateTime.Now.ToString("MM"));
                formFieldMap["txt_claim_dateYear"] = Convert.ToString(System.DateTime.Now.ToString("yyyy"));
                formFieldMap["txtproc_code_date"] = txtproc_code_date.Text;
                formFieldMap["txt_Procedure_Code"] = txtProcedureCode.Text;
                if (Session["sex"] != null)
                    if (Session["sex"].ToString() == "Mr.")
                    {

                        formFieldMap["txt_sex"] = "Male";
                        formFieldMap["txt_male"] = "X";
                    }
                    else if (Session["sex"].ToString() == "Ms.")
                    {
                        formFieldMap["txt_sex"] = "Female";
                        formFieldMap["txt_female"] = "X";
                    }
                if (Session["ssn"] != null)
                {
                    string ssn = Session["ssn"].ToString();
                    if (string.IsNullOrWhiteSpace(ssn))
                    {
                    }
                    else
                    {
                        if (ssn.Split().Last().All(char.IsDigit))
                        {
                            string ssn1 = ssn.Replace("-", "");
                            string separated = new string(
                                                             ssn1.Select((x, i) => i > 0 && i % 1 == 0 ? new[] { ',', x } : new[] { x })
                                                                .SelectMany(x => x)
                                                                .ToArray()
                                                                 );
                            if (string.IsNullOrWhiteSpace(separated))
                            {
                            }
                            else
                            {
                                int[] a = separated.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

                                if (a[0] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["1"] = Convert.ToString(a[0]);
                                }
                                if (a[1] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["2"] = Convert.ToString(a[1]);
                                }
                                if (a[2] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["3"] = Convert.ToString(a[2]);
                                }
                                if (a[3] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["4"] = Convert.ToString(a[3]);
                                }
                                if (a[4] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["5"] = Convert.ToString(a[4]);
                                }
                                if (a[5] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["6"] = Convert.ToString(a[5]);
                                }
                                if (a[6] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["7"] = Convert.ToString(a[6]);
                                }
                                if (a[7] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["8"] = Convert.ToString(a[7]);
                                }
                                if (a[8] == null)
                                {
                                }
                                else
                                {
                                    formFieldMap["9"] = Convert.ToString(a[8]);
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }
                if (Session["doa"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doa"].ToString()))
                    {
                    }
                    else
                    {
                        formFieldMap["txt_doa"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        formFieldMap["txt_doaday"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("dd");
                        formFieldMap["txt_doaMonth"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM");
                        formFieldMap["txt_doaYear"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("yyyy");
                    }
                if (Session["doe"] != null)
                    if (string.IsNullOrWhiteSpace(Session["doe"].ToString()))
                    {
                    }
                    else
                    {

                        formFieldMap["txt_doe"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");

                        DateTime doe = Convert.ToDateTime(Session["doe"].ToString());

                        formFieldMap["txtdaydoe"] = Convert.ToString(doe.Day);
                        formFieldMap["txtmonthdoe"] = Convert.ToString(doe.Month);
                        formFieldMap["txtyeardoe"] = Convert.ToString(doe.Year);

                    }
                if (Session["Compensation"] != null)
                {
                    formFieldMap["txt_casetype"] = Convert.ToString(Session["Compensation"]);
                    if (Session["Compensation"].Equals("WC"))
                    { formFieldMap["txt_wc"] = "yes"; }
                    else
                    { formFieldMap["txt_wc"] = "No"; }
                    if (Session["Compensation"].Equals("NF"))
                    { formFieldMap["txt_NF"] = "yes"; }
                    else
                    { formFieldMap["txt_NF"] = "No"; }
                    if (Session["Compensation"].Equals("PI"))
                    { formFieldMap["txt_PI"] = "yes"; }
                    else
                    { formFieldMap["txt_PI"] = "No"; }

                    if (Session["Compensation"].Equals("Lien"))
                    { formFieldMap["txt_AL"] = "yes"; }
                    else
                    { formFieldMap["txt_AL"] = "No"; }

                    if (Session["Compensation"].Equals("MM"))
                    { formFieldMap["txt_MM"] = "yes"; }
                    else
                    { formFieldMap["txt_MM"] = "No"; }

                    if (Session["Compensation"].Equals("Taxi"))
                    { formFieldMap["txt_Taxi"] = "yes"; }
                    else
                    { formFieldMap["txt_Taxi"] = "No"; }

                    formFieldMap["txt_PC"] = "No";
                    formFieldMap["txt_SP"] = "No";

                }
                if (childfilename.Text.Contains("Hackensack Specialty.pdf"))
                {
                    if (!string.IsNullOrEmpty(txtproc_code_date.Text))
                {
                    formFieldMap["txt_date"] = txtproc_code_date.Text;
                }
                else
                {
                    formFieldMap["txt_date"] = "";
                }
                }
                if (childfilename.Text.Contains("NF packet.pdf") || childfilename.Text.Contains("WC packet.pdf"))
                {
                    if (Session["sex"] != null)
                        if (Session["sex"].ToString() == "Mr.")
                        {
                            if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
                            {
                                formFieldMap["txtm_ssn"] = "";
                            }
                            else
                            {
                                formFieldMap["txtm_ssn"] = Session["ssn"].ToString();
                                //formFieldMap["txt_ssn"] = "";
                            }
                            if (!string.IsNullOrEmpty(txtproc_code_date.Text))
                            {
                                formFieldMap["txtm_date"] = txtproc_code_date.Text;
                            }
                            else
                            {
                                formFieldMap["txtm_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                            }
                            formFieldMap["txtm_name"] = names.Text;
                        }
                        else if (Session["sex"].ToString() == "Ms.")
                        {
                            if (Session["ssn"].ToString().Split().Last().All(char.IsDigit))
                            {
                                formFieldMap["txtf_ssn"] = "";
                            }
                            else
                            {
                                formFieldMap["txtf_ssn"] = Session["ssn"].ToString();
                                //formFieldMap["txt_ssn"] = "";
                            }
                            if (!string.IsNullOrEmpty(txtproc_code_date.Text))
                            {
                                formFieldMap["txtf_date"] = txtproc_code_date.Text;
                            }
                            else
                            {
                                formFieldMap["txtf_date"] = Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy"));
                            }
                            formFieldMap["txtf_name"] = names.Text;
                        }
                }
                if (Convert.ToString(Session["filename"]).Equals("Accelerated.pdf"))
                {
                    if (Session["Compensation"] != null)
                        if (Session["Compensation"].Equals("WC"))
                        {
                            if (Session["doe"] != null)
                                formFieldMap["txt_doeWC"] = Convert.ToDateTime(Session["doe"].ToString()).ToString("MM/dd/yyyy");
                        }
                        else
                        {
                            if (Session["doa"] != null)
                                formFieldMap["txt_doaMVA"] = Convert.ToDateTime(Session["doa"].ToString()).ToString("MM/dd/yyyy");
                        }
                }
                var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);
                string filename = Convert.ToString(Session["filename"]);
                string filenamefinal = filename.Split('.').First();
                // lblMessage.Visible = false;
                //lblMessage.ImageUrl = "";

                if (filename == "Surgicore Booking Sheet.pdf")
                {
                    //PDFHelper.ReturnPDF(pdfContents, lpname.Text + " " + fpname.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");
                    PDFHelper.ReturnPDF(pdfContents, lpname.Text.Trim()  + ", " + fpname.Text.Trim()  +"_"+filenamefinal+ "_"+Convert.ToString(System.DateTime.Now.ToString("MMddyy"))+"-.pdf");

                }
                else if (filename == "PatientInformation.pdf")
                {
                    //PDFHelper.ReturnPDF(pdfContents, names.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");
                    PDFHelper.ReturnPDF(pdfContents, lpname.Text.Trim()  + ", " + fpname.Text.Trim()  +"_"+filenamefinal+"_"+Convert.ToString(System.DateTime.Now.ToString("MMddyy"))+ "-.pdf");
                }
                else
                {
                    //PDFHelper.ReturnPDF(pdfContents, filenamefinal + "-" + lpname.Text + "," + fpname.Text + Convert.ToString(System.DateTime.Now.ToString("MMddyy")) + "-.pdf");
                   // PDFHelper.ReturnPDF(pdfContents, lpname.Text + " " + fpname.Text +"_"+filenamefinal+ "-.pdf");
                	PDFHelper.ReturnPDF(pdfContents, lpname.Text.Trim() + ", " + fpname.Text.Trim() + "_"+ filenamefinal+"_"+Convert.ToString(System.DateTime.Now.ToString("MMddyy"))+ "-.pdf");
                }

            }
        }

        //Response.Redirect("~/Templatestorepdf.aspx");  
    }
    [WebMethod]
    public static string[] getFirstName(string prefix)
    {
        DBHelperClass db = new DBHelperClass();
        List<string> patient = new List<string>();

        if (prefix.IndexOf("'") > 0)
            prefix = prefix.Replace("'", "''");

        DataSet ds = db.selectData("select Patient_ID, LastName, FirstName from tblPatientMaster where FirstName like '%" + prefix + "%' OR LastName Like '%" + prefix + "%'");
        if (ds.Tables[0].Rows.Count > 0)
        {
            string name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["LastName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
            name = "";
            for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                name = ds.Tables[0].Rows[i]["FirstName"].ToString();
                patient.Add(string.Format("{0}-{1}", name, ds.Tables[0].Rows[i]["Patient_ID"].ToString()));
            }
        }
        return patient.ToArray();
    }

    protected void txt_name_TextChanged(object sender, EventArgs e)
    {
        string name = "";
        if (!string.IsNullOrEmpty(txt_name.Text))
        {
            name = txt_name.Text.Trim();
            LoadPatientIE("WHERE FirstName LIKE '%" + name.Trim() + "%' OR LastName LIKE '%" + name.Trim() + "%'", 1);
        }
    }
    private void bindEditData(string PatientIEid)
    {
        try
        {

            string query = "select * from View_PatientIE where PatientIE_ID=" + PatientIEid;

            DataSet ds = db.selectData(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["FirstName"].ToString()))
                {
                    Session["fname"] = " ";
                }
                else
                {
                    Session["fname"] = ds.Tables[0].Rows[0]["FirstName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["LastName"].ToString()))
                {
                    Session["lname"] = " ";
                }
                else
                {
                    Session["lname"] = ds.Tables[0].Rows[0]["LastName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["MiddleName"].ToString()))
                {
                    Session["mname"] = " ";
                }
                else
                {
                    Session["mname"] = ds.Tables[0].Rows[0]["MiddleName"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["eMail"].ToString()))
                {
                    Session["eMail"] = " ";
                }
                else
                {
                    Session["eMail"] = ds.Tables[0].Rows[0]["eMail"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOA"].ToString()))
                {
                    Session["doa"] = " ";
                }
                else
                {
                    Session["doa"] = ds.Tables[0].Rows[0]["DOA"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["DOE"].ToString()))
                {
                    Session["doe"] = " ";
                }
                else
                {
                    Session["doe"] = ds.Tables[0].Rows[0]["DOE"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["SSN"].ToString()))
                {
                    Session["ssn"] = " ";
                }
                else
                {
                    Session["ssn"] = ds.Tables[0].Rows[0]["SSN"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Address1"].ToString()))
                {
                    Session["Address"] = " ";
                }
                else
                {
                    Session["Address"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsAddress1"].ToString()))
                {
                    Session["InsAddress"] = " ";
                }
                else
                {
                    Session["InsAddress"] = ds.Tables[0].Rows[0]["Address1"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone2"].ToString()))
                {
                    Session["mob"] = " ";
                }
                else
                {
                    Session["mob"] = ds.Tables[0].Rows[0]["Phone2"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsPhone"].ToString()))
                {
                    Session["InsPhone"] = " ";
                }
                else
                {
                    Session["InsPhone"] = ds.Tables[0].Rows[0]["InsPhone"].ToString();
                }

                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["City"].ToString()))
                {
                    Session["city"] = " ";
                }
                else
                {
                    Session["city"] = ds.Tables[0].Rows[0]["City"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCity"].ToString()))
                {
                    Session["Inscity"] = " ";
                }
                else
                {
                    Session["Inscity"] = ds.Tables[0].Rows[0]["InsCity"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["State"].ToString()))
                {
                    Session["state"] = " ";
                }
                else
                {
                    Session["state"] = ds.Tables[0].Rows[0]["State"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsState"].ToString()))
                {
                    Session["Insstate"] = " ";
                }
                else
                {
                    Session["Insstate"] = ds.Tables[0].Rows[0]["InsState"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Zip"].ToString()))
                {
                    Session["zip"] = " ";
                }
                else
                {
                    Session["zip"] = ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsZip"].ToString()))
                {
                    Session["Inszip"] = " ";
                }
                else
                {
                    Session["Inszip"] = ds.Tables[0].Rows[0]["InsZip"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Phone"].ToString()))
                {
                    Session["Phone"] = " ";
                }
                else
                {
                    Session["Phone"] = ds.Tables[0].Rows[0]["Phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["work_phone"].ToString()))
                {
                    Session["work_phone"] = " ";
                }
                else
                {
                    Session["work_phone"] = ds.Tables[0].Rows[0]["work_phone"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Sex"].ToString()))
                {
                    Session["sex"] = " ";
                }
                else
                {
                    Session["sex"] = ds.Tables[0].Rows[0]["Sex"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["InsCo"].ToString()))
                {
                    Session["InsCo"] = " ";
                }
                else
                {
                    Session["InsCo"] = ds.Tables[0].Rows[0]["InsCo"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["policy_no"].ToString()))
                {
                    Session["policy_no"] = " ";
                }
                else
                {
                    Session["policy_no"] = ds.Tables[0].Rows[0]["policy_no"].ToString();
                }


                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["ClaimNumber"].ToString()))
                {
                    Session["ClaimNumber"] = " ";
                }
                else
                {
                    Session["ClaimNumber"] = ds.Tables[0].Rows[0]["ClaimNumber"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Location"].ToString()))
                {
                    Session["LocationPdf"] = " ";
                }
                else
                {
                    Session["LocationPdf"] = ds.Tables[0].Rows[0]["Location"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Attorney"].ToString()))
                {
                    Session["Attorney"] = " ";
                }
                else
                {
                    Session["Attorney"] = ds.Tables[0].Rows[0]["Attorney"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyAdd"].ToString()))
                {
                    Session["AttorneyAdd"] = " ";
                }
                else
                {
                    Session["AttorneyAdd"] = ds.Tables[0].Rows[0]["AttorneyAdd"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AttorneyPhno"].ToString()))
                {
                    Session["AttorneyPhno"] = " ";
                }
                else
                {
                    Session["AttorneyPhno"] = ds.Tables[0].Rows[0]["AttorneyPhno"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Adjuster"].ToString()))
                {
                    Session["Adjuster"] = " ";
                }
                else
                {
                    Session["Adjuster"] = ds.Tables[0].Rows[0]["Adjuster"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["Compensation"].ToString()))
                {
                    Session["Compensation"] = " ";
                }
                else
                {
                    Session["Compensation"] = ds.Tables[0].Rows[0]["Compensation"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["WCBGroup"].ToString()))
                {
                    Session["WCBGroup"] = " ";
                }
                else
                {
                    Session["WCBGroup"] = ds.Tables[0].Rows[0]["WCBGroup"].ToString();
                }
                if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["LocAdd"].ToString()))
                {
                    Session["LocAdd"] = " ";
                }
                else
                {
                    Session["LocAdd"] = ds.Tables[0].Rows[0]["LocAdd"].ToString();
                }
                //if (string.IsNullOrWhiteSpace(ds.Tables[0].Rows[0]["AGE"].ToString()))
                //{
                //Session["AGE"] = " ";
                //}
                //else
                //{
                //Session["AGE"] = ds.Tables[0].Rows[0]["AGE"].ToString();
                //}

                if (ds.Tables[0].Rows[0]["DOB"] != DBNull.Value)
                {
                    DateTime dob = Convert.ToDateTime(ds.Tables[0].Rows[0]["DOB"].ToString());
                    Session["dob"] = dob.ToString("MM/dd/yyyy");
                    Session["AGE"] = CalculateAge(dob);
                }
                else
                {
                    Session["dob"] = " ";
                    Session["AGE"] = " ";
                }
            }
        }
        catch (Exception ex)
        {
            db.LogError(ex);
        }
    }
    private static int CalculateAge(DateTime dateOfBirth)
    {
        int age = 0;
        age = DateTime.Now.Year - dateOfBirth.Year;
        if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
            age = age - 1;

        return age;
    }

    public void setPDF(string pdfpath, string pdfpathourput1, string imgpath, float[] x, float[] y)
    {
        string pdfpathourput = Server.MapPath("~/PdfForms/" + pdfpathourput1);
        using (Stream inputPdfStream = new FileStream(pdfpath, FileMode.Open, FileAccess.Read, FileShare.Read))
        using (Stream outputPdfStream = new FileStream(pdfpathourput, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            var reader = new PdfReader(inputPdfStream);
            var stamper = new PdfStamper(reader, outputPdfStream);
            PdfContentByte pdfContentByte = null;

            int c = reader.NumberOfPages;
            string fnmae = imgpath;
            iTextSharp.text.Image image = null;
            for (int i = 1; i <= c; i++)
            {
                if (x.Count() > (i - 1))
                {
                    if (x[i - 1] > 0)
                    {
                        image = iTextSharp.text.Image.GetInstance(fnmae);
                        pdfContentByte = stamper.GetOverContent(i);
                        image.ScaleAbsolute(125f, 35f);
                        image.SetAbsolutePosition(x[i - 1], y[i - 1]);
                        pdfContentByte.AddImage(image);
                    }
                }
            }
            stamper.Close();
        }

    }

    protected void lnk_openIEPdf_Click(object sender, EventArgs e)
    {
        if (TreeView1.CheckedNodes.Count > 0 && TreeView1.CheckedNodes.Count < 2)
        {
            LinkButton btn = sender as LinkButton;
            string filename = "";
            string id = btn.CommandArgument;
            foreach (TreeNode node in TreeView1.CheckedNodes)
                filename = node.Text;
            PdfGenerator pg = new PdfGenerator();
            if (File.Exists(Server.MapPath("~/TemplateStore/DownloadPdf/" + filename)))
                               pg.Stamping(Server.MapPath ("~/TemplateStore/DownloadPdf/" + filename), "PatientIE_ID", id,this.Form.FindControl("cpmain")  );

        }
    }
}