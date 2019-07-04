using Microsoft.SharePoint;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace SharePointProject2.TodoWorker
{
    public partial class TodoWorkerUserControl : UserControl
    {
        private readonly SPContext CurrentSite = SPContext.Current;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                FillData();
            }
        }

        #region Events
        /// <summary>
        /// When value has changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TasksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //get selected index
            int selectedIndex = (sender as DropDownList).SelectedIndex;
            //get items depending on selected index
            SPListItem item = CurrentSite.Web.Lists.TryGetList("TestList").Items[selectedIndex];
            ClearTextBoxes();
            //Fill textboxes
            NameText.Text = item["Task Name"].ToString();

            //If fields in tasks list is null(cause try catch is very heavy)
            if (item["Start Date"] is DateTime)
                StartDate.SelectedDate = (DateTime)item["Start Date"];

            if (item["Due Date"] is DateTime)
                DueDate.SelectedDate = (DateTime)item["Due Date"];

            if (item["% Complete"] is Double)
                CompleteBox.Text = item["% Complete"].ToString();
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = TasksList.SelectedIndex;
            CurrentSite.Web.AllowUnsafeUpdates = true;
            SPListItem item = CurrentSite.Web.Lists.TryGetList("TestList").Items[selectedIndex];

            if (!string.IsNullOrEmpty(NameText.Text))
                item["Task Name"] = NameText.Text;

            item["Start Date"] = StartDate.SelectedDate;

            item["Due Date"] = DueDate.SelectedDate;

            if (!string.IsNullOrEmpty(CompleteBox.Text))
                item["% Complete"] = double.Parse(CompleteBox.Text) * 100;

            item.Update();

            CurrentSite.Web.AllowUnsafeUpdates = false;

            FillData();
        }

        /// <summary>
        /// Fires when delete button was clicked
        /// </summary>
        /// <param name="sender"> Button </param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            int selectedIndex = TasksList.SelectedIndex;
            //get items depending on selected index
            SPListItem item = CurrentSite.Web.Lists.TryGetList("TestList").Items[selectedIndex];

            item.Delete();

            FillData();
        }

        #endregion

        #region Helpers

        protected void FillData()
        {
            //Bind List Items to Tasks List DropDown Control by id
            TasksList.DataSource = CurrentSite.Web.Lists.TryGetList("TestList").Items;
            TasksList.DataValueField = "Title";
            TasksList.DataTextField = "Title";
            TasksList.DataBind();

            SPListItem item = CurrentSite.Web.Lists.TryGetList("TestList").Items[0];
            ClearTextBoxes();

            //Fill textboxes
            NameText.Text = item["Task Name"].ToString();

            //If fields in tasks list is null(cause try catch is very heavy)
            if (item["Start Date"] is DateTime)
                StartDate.SelectedDate = (DateTime)item["Start Date"];

            if (item["Due Date"] is DateTime)
                DueDate.SelectedDate = (DateTime)item["Due Date"];

            if (item["% Complete"] is Double)
                CompleteBox.Text = item["% Complete"].ToString();
        }

        /// <summary>
        /// Hide controls
        /// </summary>
        /// <param name="hide">If <see cref="true"/> </param>
        protected void HideShowControls(bool hide)
        {

        }

        /// <summary>
        /// Clear textboxes
        /// </summary>
        protected void ClearTextBoxes()
        {
            NameText.Text = "";
            StartDate.ClearSelection();
            DueDate.ClearSelection();
            CompleteBox.Text = "";
        }

        #endregion
    }
}
