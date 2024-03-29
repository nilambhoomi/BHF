﻿using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ViewPatientMaster : System.Web.UI.Page
{
    ILog log = log4net.LogManager.GetLogger(typeof(ViewPatientMaster));
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["pageindex"] = "1";
            ViewState["pagesize"] = "10";
            ViewState["_query"] = "";
            bindPatientDetails(1);
        }
    }

    public DataSet bindPatientDetails(int pageIndex = 1, string sortorder = "asc", string columnname = "FirstName")
    {
        DataSet lds = null;
        int totalrecords = 0;

        using (SqlConnection gConn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_V3"].ConnectionString))
        {
            try
            {
                int pagesize = Convert.ToInt16(ViewState["pagesize"]);
                SqlCommand gComm = new SqlCommand("nusp_GetPatientMaster", gConn);

                gComm.CommandType = CommandType.StoredProcedure;
                gComm.Parameters.AddWithValue("@PageIndex", pageIndex);
                gComm.Parameters.AddWithValue("@cnd", ViewState["_query"].ToString());

                gComm.Parameters.AddWithValue("@ordercolumn", columnname);
                gComm.Parameters.AddWithValue("@sortorder", sortorder);

                gComm.Parameters.AddWithValue("@PageSize", pagesize);
                gComm.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                gComm.Parameters["@RecordCount"].Direction = ParameterDirection.Output;

                gConn.Open();
                SqlDataAdapter lda = new SqlDataAdapter(gComm);
                lds = new DataSet();
                lda.Fill(lds);


                totalrecords = Convert.ToInt32(gComm.Parameters["@RecordCount"].Value);
                gConn.Close();

                if (totalrecords > 0)
                {
                    gvPatientDetails.DataSource = lds;
                    gvPatientDetails.DataBind();
                }
                else
                {
                    gvPatientDetails.DataSource = null;
                    gvPatientDetails.DataBind();
                }


                PopulatePager(totalrecords, pageIndex);
            }

            catch (Exception ex)
            {

                gConn.Close();
                log.Error(ex.Message);
            }
        }
        return lds;
    }

    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        ViewState["pageindex"] = pageIndex;
        this.bindPatientDetails(pageIndex);
    }

    private void PopulatePager(int recordCount, int currentPage)
    {
        try
        {

            int pagesize = Convert.ToInt16(ViewState["pagesize"]);

            if (pagesize > 0)
            {
                List<ListItem> pages = new List<ListItem>();
                int startIndex, endIndex;
                int pagerSpan = 5;

                //Calculate the Start and End Index of pages to be displayed.
                double dblPageCount = (double)((decimal)recordCount / Convert.ToDecimal(pagesize));
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
                    div_page.Style.Add("display", "block");
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
            else
            {
                div_page.Style.Add("display", "none");

                rptPager.DataSource = null;
                rptPager.DataBind();
            }
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        DBHelperClass db = new DBHelperClass();
        try
        {
            LinkButton lnkdelete = sender as LinkButton;



            Response.Redirect("DeleteConfirmMessage.aspx?id=" + lnkdelete.CommandArgument);



        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string cnd = "";
        ViewState["_query"] =null;
        if (!string.IsNullOrEmpty(txtSearch.Text))
        {
            cnd = " where Patient_ID=" + hfPatientId.Value;
        }
        ViewState["_query"] = cnd;
        bindPatientDetails();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
         txtSearch.Text = "";
        ViewState["_query"] =null;
        Response.Redirect("~/ViewPatientMaster.aspx");
    }

    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["pagesize"] = ddlPage.SelectedItem.Value;
        bindPatientDetails(1);
    }
}