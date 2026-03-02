using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Diagnostics;
using eWorld.UI;


namespace DataGridExt.ItemTemplate
{

	/// <summary>
	/// Summary description for BoeingItemTemplate.
	/// </summary>
	/// 

	public class ItemTemplateEx : ITemplate
	{
		protected string m_columnName;
		protected string m_format = "";
		public string DataField
		{
			get { return m_columnName; }
			set { m_columnName = value; }
		}

		public string DataFormat
		{
			get { return m_format; }
			set { m_format = value; }
		}

		public virtual void InstantiateIn(Control container)
		{

			// Do not implementation.
		}
	}



	public class HyperLinkItem : ItemTemplateEx
	{
		private string m_fieldNavigateUrl;
		private string m_urlParam;
		private bool m_IsURL = true;

		public HyperLinkItem(string columnName, string urlParam, string fieldNavigateUrl, bool IsURL)
		{
			this.m_columnName = columnName;
			this.m_urlParam = urlParam;
			this.m_fieldNavigateUrl = fieldNavigateUrl;
			this.m_IsURL = IsURL;
		}

		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			HyperLink ctrl = new HyperLink();
			ctrl.ForeColor = System.Drawing.Color.MediumBlue;
			ctrl.ID = m_columnName;

			ctrl.DataBinding += new EventHandler(this.BindData);

			container.Controls.Add(ctrl);
		}

		public void BindData(object sender, EventArgs e)
		{
			HyperLink ctrl = (HyperLink)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			try
			{
				ctrl.Text = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
				if (m_IsURL)
				{
					//ctrl.NavigateUrl = HttpContext.Current.Request.Path + m_urlParam + "&oid=";
					ctrl.NavigateUrl = m_urlParam + "&oid=";
					ctrl.NavigateUrl += (DataBinder.Eval(container.DataItem, m_fieldNavigateUrl)).ToString();
				}
				else
				{

					string strParam = m_urlParam
						+ String.Format(@"?paramDetail={0}"
						, (DataBinder.Eval(container.DataItem, "CNT")).ToString());
					/*
					+ String.Format(@"?paramDetail={0};{1};{2};{3};{4};{5};{6}",
					(DataBinder.Eval(container.DataItem, "CSN")).ToString()
					,(DataBinder.Eval(container.DataItem, "CNT")).ToString()
					,(DataBinder.Eval(container.DataItem, "DETAIL")).ToString()
					,(DataBinder.Eval(container.DataItem, "STS")).ToString()
					,String.Format("{0:N}",DataBinder.Eval(container.DataItem, "ISA"))
					,String.Format("{0:N}",DataBinder.Eval(container.DataItem, "PNA"))
					,String.Format("{0:N}",DataBinder.Eval(container.DataItem, "OTHOR")));
					*/

					ctrl.Attributes["href"] = "#";
					ctrl.Attributes["onclick"] = "javascript:OpenWindowDetail('" + strParam + "')";
					//ctrl.Attributes["onclick"] = "javascript:CallPagePopup('" + m_urlParam + "')";  
				}
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:HyperLinkColumnItem");
			}
		}
	}



	public class CheckBoxItem : ItemTemplateEx
	{
		private bool m_ReadOnly = true;

		public CheckBoxItem(string columnName, bool readOnly)
		{
			this.m_columnName = columnName;
			this.m_ReadOnly = readOnly;
		}

		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			CheckBox ctrl = new CheckBox();
			ctrl.ID = "cbxClmnEx" + m_columnName;
			ctrl.Enabled = !m_ReadOnly;

			ctrl.DataBinding += new EventHandler(BindData);
			container.Controls.Add(ctrl);
		}

		public void BindData(object sender, EventArgs e)
		{
			CheckBox ctrl = (CheckBox)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			string strVal = "";

			try
			{
				strVal = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:CheckBoxColumnItem");
			}

			ctrl.Checked = (strVal == "T" || strVal == "Y" || strVal == "1" || strVal.Trim().ToUpper() == "TRUE" || strVal.Trim().ToLower() == "true");
		}

	}





	public class LiteralItem : ItemTemplateEx
	{
		public LiteralItem(string columnName)
		{
			this.m_columnName = columnName;
		}

		public LiteralItem(string columnName, string dataFormat)
		{
			this.m_columnName = columnName;
			this.m_format = dataFormat;
		}

		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			Literal ctrl = new Literal();
			ctrl.ID = "bndClmnEx" + m_columnName;
			ctrl.DataBinding += new EventHandler(BindData);
			container.Controls.Add(ctrl);
		}

		public void BindData(object sender, EventArgs e)
		{
			Literal ctrl = (Literal)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			try
			{
				if (m_format.Trim().Length > 0)
					ctrl.Text = DataBinder.Eval(container.DataItem, m_columnName, m_format);
				else
					ctrl.Text = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:LiteralColumnItem");
			}
		}

	}

	public class NumberTextBoxItem : ItemTemplateEx
	{
		public NumberTextBoxItem(string columnName)
		{
			this.m_columnName = columnName;
		}

		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			NumericBox ctrl = new NumericBox();
			ctrl.ID = "tbxClmnEx" + m_columnName;
			ctrl.TextAlign = eWorld.UI.HorizontalAlignment.Right;
			ctrl.RealNumber = false;
			ctrl.PositiveNumber = true;
			ctrl.Width = Unit.Percentage(100);
			ctrl.MaxLength = 13;
			ctrl.DataBinding += new EventHandler(this.BindData);
			container.Controls.Add(ctrl);



		}

		public void BindData(object sender, EventArgs e)
		{
			NumericBox ctrl = (NumericBox)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			try
			{
				ctrl.Text = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:LiteralColumnItem");
			}
		}
	}


	public class RealTextBoxItem : ItemTemplateEx
	{
		private NumericBox ctlNumber = null;
		private int MaxLengthValue = 13;
		private int DecimalPlacesValue = 2;
		private eWorld.UI.HorizontalAlignment TextAlignValue = eWorld.UI.HorizontalAlignment.Right;

		#region Properties
		public int MaxLength
		{
			set { MaxLengthValue = value; }
			get { return MaxLengthValue; }
		}
		public int DecimalPlaces
		{
			set { DecimalPlacesValue = value; }
			get { return DecimalPlacesValue; }
		}
		public eWorld.UI.HorizontalAlignment TextAlign
		{
			set { TextAlignValue = value; }
			get { return TextAlignValue; }
		}

		#endregion

		public RealTextBoxItem(string columnName)
		{
			this.m_columnName = columnName;
		}
		public RealTextBoxItem(string columnName, string dataFormat)
		{
			this.m_columnName = columnName;
			this.m_format = dataFormat;
		}


		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			ctlNumber = new NumericBox();
			ctlNumber.ID = "tbxClmnEx" + m_columnName;
			ctlNumber.RealNumber = true;
			ctlNumber.PositiveNumber = true;
			ctlNumber.Width = Unit.Percentage(100);

			ctlNumber.TextAlign = TextAlignValue;
			ctlNumber.MaxLength = MaxLengthValue;
			ctlNumber.DecimalPlaces = DecimalPlacesValue;
			ctlNumber.DataBinding += new EventHandler(this.BindData);
			container.Controls.Add(ctlNumber);

		}
		public NumericBox GetControl
		{
			get
			{
				return ctlNumber;
			}
		}
		public void BindData(object sender, EventArgs e)
		{
			NumericBox ctrl = (NumericBox)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			try
			{
				ctrl.Text = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:LiteralColumnItem");
			}
		}
	}


	public class TextBoxItem : ItemTemplateEx
	{
		public TextBoxItem(string columnName)
		{
			this.m_columnName = columnName;
		}

		public override void InstantiateIn(Control container)
		{
			base.InstantiateIn(container);

			TextBox ctrl = new TextBox();
			ctrl.ID = "tbxClmnEx" + m_columnName;
			ctrl.Width = Unit.Percentage(100);
			ctrl.DataBinding += new EventHandler(this.BindData);
			container.Controls.Add(ctrl);

			//			RequiredFieldValidator rfv = new RequiredFieldValidator();
			//			rfv.Text = "Please Answer";
			//			rfv.ControlToValidate = ctrl.ID;
			//			rfv.Display = ValidatorDisplay.Dynamic;
			//			rfv.ID = "validate" + ctrl.ID;
			//			container.Controls.Add(rfv);		

		}

		public void BindData(object sender, EventArgs e)
		{
			TextBox ctrl = (TextBox)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			try
			{
				ctrl.Text = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:LiteralColumnItem");
			}
		}
	}



	public class DropDownListItem : ItemTemplateEx
	{
		//private string	m_curValue = "";
		private ArrayList m_Elements;
		public DropDownListItem(string columnName, ArrayList arrList)
		{

			this.m_columnName = columnName;
			this.m_Elements = arrList;
		}
		public override void InstantiateIn(Control container)
		{
			DropDownList ctrl = new DropDownList();
			ctrl.ID = "ddlClmnEx" + m_columnName;
			ctrl.Width = Unit.Percentage(100);

			ctrl.DataBinding += new EventHandler(this.BindData);
			container.Controls.Add(ctrl);
		}

		public void BindData(object sender, EventArgs e)
		{
			DropDownList ctrl = (DropDownList)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;


			// Add elements to dropdownlist.
			//			foreach( ListItem item in m_Elements )
			//			{
			//				if((string)DataBinder.Eval(container.DataItem , "STS")=="W/O"
			//				|| (string)DataBinder.Eval(container.DataItem , "STS")=="L/C")
			//				{
			//					if(item.Value !="1")	
			//						ctrl.Items.Add( item );
			//				}
			//				else
			//					ctrl.Items.Add( item );
			//			}
			//			ctrl.SelectedIndex = ctrl.Items.Count -1; 
			//			// Add elements to dropdownlist.
			foreach (ListItem item in m_Elements)
				ctrl.Items.Add(item);


			// Set selected index.
			/*
			try
			{ 
				m_curValue = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
			}
			catch ( Exception eEx )
			{
				Debug.WriteLine( eEx.Message, "DataGrid:LiteralColumnItem" );
			}
			ctrl.ClearSelection();  
			ctrl.SelectedIndex = ctrl.Items.Count -1;
			if(ctrl.Items.FindByValue(m_curValue)!= null)
			{
				ctrl.ClearSelection();
				ctrl.Items.FindByValue(m_curValue).Selected = true;
			}
			*/
		}
	}

	public class DropDownListItem2 : ItemTemplateEx
	{
		private string m_curValue = "";
		private string m_strTextField;
		private string m_strValueField;
		private DataTable m_dvDataSource;
		public DropDownListItem2(string columnName)
		{
			this.m_columnName = columnName;
		}
		public string TextField
		{
			set { m_strTextField = value; }
		}
		public string ValueField
		{
			set { m_strValueField = value; }
		}
		public DataTable DataSource
		{
			set { m_dvDataSource = value; }
		}

		public override void InstantiateIn(Control container)
		{
			try
			{
				DropDownList ctrl = new DropDownList();
				ctrl.ID = "ddlClmnEx" + m_columnName;
				ctrl.Width = Unit.Percentage(100);
				foreach (DataRow dr in m_dvDataSource.Rows)
					ctrl.Items.Add(new ListItem(dr[m_strTextField].ToString(),
							dr[m_strValueField].ToString()));

				ctrl.DataBinding += new EventHandler(BindData);
				container.Controls.Add(ctrl);
			}
			catch
			{ }

		}

		public void BindData(object sender, EventArgs e)
		{
			DropDownList ctrl = (DropDownList)sender;
			DataGridItem container = (DataGridItem)ctrl.NamingContainer;

			// Set selected index.
			try
			{
				m_curValue = (DataBinder.Eval(container.DataItem, m_columnName)).ToString();
				ctrl.SelectedIndex = ctrl.Items.IndexOf(ctrl.Items.FindByValue(m_curValue));
			}
			catch (Exception eEx)
			{
				Debug.WriteLine(eEx.Message, "DataGrid:LiteralColumnItem");
			}

		}
	}

	public class TextDateTimeColumn : ItemTemplateEx
	{
		private string strImgPath;
		private string strCalendarPath;
		public TextDateTimeColumn(string columnName, string strImageURLPath, string strCalendarPopPath)
		{
			this.m_columnName = columnName;
			this.strImgPath = strImageURLPath;
			this.strCalendarPath = strCalendarPopPath;
		}

		public override void InstantiateIn(Control container)
		{
			TextBox tb = new TextBox();
			tb.ID = "txtClmnEx" + m_columnName;
			tb.Width = Unit.Percentage(90);
			tb.DataBinding += new EventHandler(this.BindData);
			container.Controls.Add(tb);
			string strImgID = "img" + m_columnName;
			StringBuilder stb = new StringBuilder();
			stb.Append("<a href=# onclick=\"javascript:CallCalendarPagePopup('");
			stb.Append(strCalendarPath);
			stb.Append("','dd/MM/yyyy','");
			stb.Append(tb.ID);
			stb.Append("',1,this");
			stb.Append(");\">");
			stb.Append("<IMG ID=\"");
			stb.Append(strImgID);
			stb.Append("\"");
			stb.Append(" SRC=\"");
			stb.Append(strImgPath);
			stb.Append(@"\SmallCalendar.gif");
			stb.Append("\" border=\"0\" align=\"absmiddle\"></a>");
			container.Controls.Add(new LiteralControl("&nbsp;"));
			container.Controls.Add(new LiteralControl(stb.ToString()));
		}

		public void BindData(object sender, EventArgs e)
		{
			TextBox tb = (TextBox)sender;
			DataGridItem container = (DataGridItem)tb.NamingContainer;
			tb.Text = DataBinder.Eval(container.DataItem, m_columnName).ToString();

		}
	}


	////////////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// Summary description for TempateColumn: 
	/// DeleteColumn, EditImageColumn and UpdateCancelColumn.
	/// </summary>
	public class DeleteColumn : ITemplate
	{
		private bool m_isDelete = true;
		private string m_strImageURL;
		private bool m_IsImageButton = false;
		public DeleteColumn()
		{ }
		public DeleteColumn(bool isDel)
		{
			m_isDelete = isDel;
		}
		public DeleteColumn(bool isDel, string strImageURL)
		{
			m_isDelete = isDel;
			m_IsImageButton = true;
			m_strImageURL = strImageURL;
		}
		public bool IsButtonImage
		{
			set { m_IsImageButton = value; }
		}
		public void InstantiateIn(Control container)
		{
			if (m_isDelete)
			{
				if (m_IsImageButton)
				{
					ImageButton imbBtn = new ImageButton();
					imbBtn.ImageUrl = m_strImageURL + "/Delete.gif"; ;
					imbBtn.Attributes["onclick"] = "return confirm('Are you sure want to delete this item?');";  //"return ConfirmDelete();";
					imbBtn.CommandName = "Delete";
					imbBtn.ToolTip = "Delete this row";
					container.Controls.Add(imbBtn);
				}
				else
				{
					LinkButton lnkBtn = new LinkButton();
					lnkBtn.Attributes["onclick"] = "return confirm('Are you sure want to delete this item?');"; //"return ConfirmDelete();" ;
					lnkBtn.Text = "Delete";
					lnkBtn.CommandName = "Delete";
					container.Controls.Add(lnkBtn);
				}
			}
			else
			{
				Literal lit = new Literal();
				lit.Text = "No Delete";
				container.Controls.Add(lit);
			}
		}
	}


	/// <summary>
	/// Summary description for TempateColumn: 
	/// DeleteColumn, EditImageColumn and UpdateCancelColumn.
	/// </summary>
	public class EditColumn : ITemplate
	{
		private bool m_IsLinkURL = false;
		private string m_strURL;
		private string m_strImageURL;

		private bool m_IsImageButton = false;
		public EditColumn()
		{
		}
		public EditColumn(string strURL)
		{
			InitailizeConstruct(true, strURL, false, "");
		}

		public EditColumn(bool IsLinkURL, string strURL, bool IsImageButton, string strImageURL)
		{
			InitailizeConstruct(IsLinkURL, strURL, IsImageButton, strImageURL);
		}

		private void InitailizeConstruct(bool IsLinkURL, string strURL, bool IsImageButton, string strImageURL)
		{
			m_IsLinkURL = IsLinkURL;
			m_strURL = strURL;
			m_strImageURL = strImageURL;
			m_IsImageButton = IsImageButton;
		}
		public bool IsButtonImage
		{
			set { m_IsImageButton = value; }
		}
		public void InstantiateIn(Control container)
		{
			if (m_IsImageButton)
			{
				ImageButton imbBtn = new ImageButton();
				imbBtn.ImageUrl = m_strImageURL + "/Edit.gif";
				imbBtn.ToolTip = "Edit this row";
				if (!m_IsLinkURL)
					imbBtn.CommandName = "Edit";
				else
				{
					imbBtn.Attributes["href"] = "#";
					imbBtn.Attributes["onclick"] = "javascript:CallPagePopup('" + m_strURL + "')";
				}
				container.Controls.Add(imbBtn);
			}
			else
			{
				LinkButton lnkBtn = new LinkButton();
				lnkBtn.Text = "Edit";
				if (!m_IsLinkURL)
					lnkBtn.CommandName = "Edit";
				else
				{
					lnkBtn.Attributes["href"] = "#";
					lnkBtn.Attributes["onclick"] = "javascript:CallPagePopup('" + m_strURL + "')";
				}
				container.Controls.Add(lnkBtn);
			}
		}
	}

	public class UpdateCancelColumn : ITemplate
	{
		private string m_strImageURL;
		private bool m_IsImageButton = false;
		public UpdateCancelColumn()
		{
		}
		public UpdateCancelColumn(string strImageURL)
		{
			m_IsImageButton = true;
			m_strImageURL = strImageURL;
		}
		public void InstantiateIn(Control container)
		{
			if (m_IsImageButton)
			{
				ImageButton imbBtn = new ImageButton();
				imbBtn.ID = "imgbtnOKX";
				imbBtn.ImageUrl = m_strImageURL + "/OK.gif";
				imbBtn.CommandName = "Update";
				imbBtn.ToolTip = "Update this row";
				imbBtn.Attributes["href"] = "#";
				container.Controls.Add(imbBtn);
				container.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;"));

				imbBtn = new ImageButton();
				imbBtn.ID = "imgbtnCancelX";
				imbBtn.ImageUrl = m_strImageURL + "/Cancel.gif";
				imbBtn.CommandName = "Cancel";
				imbBtn.ToolTip = "Cancel for edit";
				imbBtn.Attributes["href"] = "#";
				container.Controls.Add(imbBtn);

			}
			else
			{
				LinkButton lnkBtn = new LinkButton();
				lnkBtn.Text = "Update";
				lnkBtn.CommandName = "Update";
				container.Controls.Add(lnkBtn);
				container.Controls.Add(new LiteralControl("&nbsp;"));

				lnkBtn = new LinkButton();
				lnkBtn.Text = "Cancel";
				lnkBtn.CommandName = "Cancel";
				container.Controls.Add(lnkBtn);
			}
		}
	}


	/// <summary>
	/// Summary description for TempateColumn: 
	/// DeleteColumn, EditImageColumn and UpdateCancelColumn.
	/// </summary>
	public class CustomColumn : ITemplate
	{
		private bool m_IsLinkURL = false;
		private string m_strURL = "";
		private string m_strImageURL = "";
		private string m_Name = "";
		private string m_Command = "";
		private bool m_IsImageButton = false;
		public CustomColumn()
		{
		}
		public CustomColumn(string strName, string Command)
		{
			InitailizeConstruct(strName, Command);
		}

		public CustomColumn(bool IsLinkURL, string strURL, bool IsLinkButton, string Text)
		{
			m_IsLinkURL = IsLinkURL;
			m_strURL = strURL;

			m_IsImageButton = IsLinkButton;
			m_Name = Text;
			InitailizeConstruct(m_Name, "");
		}

		private void InitailizeConstruct(string strName, string Command)
		{

			m_Name = strName;
			m_Command = Command;
		}
		public bool IsButtonImage
		{
			set { m_IsImageButton = value; }
		}
		public void InstantiateIn(Control container)
		{
			if (m_IsImageButton)
			{
				LinkButton imbBtn = new LinkButton();
				//imbBtn.ImageUrl = m_strImageURL + "/New.gif";
				imbBtn.ToolTip = m_Name;
				imbBtn.Text = m_Name;
				if (!m_IsLinkURL)
					imbBtn.CommandName = "New";
				else
				{
					imbBtn.Attributes["href"] = "#";
					imbBtn.Attributes["onclick"] = "javascript:OpenNewWindow('" + m_strURL + "')";
				}
				container.Controls.Add(imbBtn);
			}
			else
			{
				LinkButton lnkBtn = new LinkButton();
				lnkBtn.ID = "lnkBtnCol_" + m_Name;
				lnkBtn.Text = m_Name;
				if (!m_IsLinkURL)
					lnkBtn.CommandName = m_Command;
				else
				{
					lnkBtn.Attributes["href"] = "#";
					lnkBtn.Attributes["onclick"] = "javascript:CallPagePopup('" + m_strURL + "')";
				}
				container.Controls.Add(lnkBtn);
			}
		}
	}

}
