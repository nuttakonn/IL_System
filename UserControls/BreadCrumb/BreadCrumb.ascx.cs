using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BreadCrumb : System.Web.UI.UserControl
{
    //Variable holding the Link name of the page
    private string _tailName = "";
    //Variable holding the level of the page
    private int _level = 0;
    //The pagecrumb object of the current page
    private PageCrumb _pageCrumb = new PageCrumb();
    //We will use a sorted list as we can use the level as key 
    private SortedList _crumbList;



    //Each page has a level. The page should declare its level
    public int Level
    {
        //' TO DO : We can check for some constraints here
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
    }


    //'Each page needs a meaningful name of it. Let them declare it
    public string TailName
    {
        //   ' TO DO : We can check for some constraints here
        get
        {
            return _tailName;
        }
        set
        {
            _tailName = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["breadcrumb"] != null)
            Session["BreadCrumbs"] = ((string)Request.QueryString["breadcrumb"]).Replace("@", " -->");

        //string[] s = ((string)Session["BreadCrumbs"]).Split('@');
        //for(int i=0;i<s.Length;++i)
        //{
        //    string sTag = "<>";

        //}
        lblTrail.Text = (string)Session["BreadCrumbs"];


        ////   'Minimum level is 1
        //   if(_level <= 0) 
        //       _level = 1;


        //   //'If no friendly name gives Untitled as default
        //   if (_tailName == "")
        //       _tailName = "Untitled";


        //   //'Create a Crumb object based on the properties of this page
        //   _pageCrumb = new PageCrumb(_level, Request.RawUrl, _tailName);

        //   //'Check our Crumb is there in the session...if not create and add it...else get it    
        //   if(Session["HASH_OF_CRUMPS"] == null)
        //   {
        //       _crumbList = new SortedList();
        //       Session.Add("HASH_OF_CRUMPS", _crumbList);
        //   }else
        //       _crumbList = (SortedList)Session["HASH_OF_CRUMPS"];


        //   //'Now modify the List of the breadcrumb
        //   ModifyList();
        //   //' Put the breadcrumb from the session of sortlist
        //   PutBreadCrumbs();



    }

    private void ModifyList()
    {
        //   'Remove all Entries from the list which is higher or equal in level
        //   'Because at a level there can be max 1 entry in the list
        RemoveLowerLevelCrumbs();
        //'If level is 1 set the Crumb as home
        if (_pageCrumb.Level == 1)
        {
            _crumbList.Clear();
            _crumbList.Add(1, new PageCrumb(1, "/Home.aspx", "Home"));
        }
        else
        {
            //If nothing in the list adds the home link first
            if (_crumbList.Count == 0)
                _crumbList.Add(1, new PageCrumb(1, "/Home.aspx", "Home"));

            //'Now add the present list also no other check is required here as we have cleaned up the 
            //'List at the start of the function
            _crumbList.Add(_level, _pageCrumb);
        }
    }

    //Function will remove all the entries from the list which is higher or equal to the
    //present level
    private void RemoveLowerLevelCrumbs()
    {

        ArrayList removalList = new ArrayList(_crumbList.Count);
        foreach (int level in _crumbList.Keys)
        {
            if (level >= _level)
                removalList.Add(level);

        }
        //'Now remove all keys in the list
        foreach (int level in removalList)
            _crumbList.Remove(level);

    }


    private void PutBreadCrumbs()
    {
        StringBuilder linkString = new StringBuilder();

        PageCrumb pageCrumb = new PageCrumb();
        int index = 0;
        for (index = 0; index < _crumbList.Count - 2; ++index)
        {
            pageCrumb = (PageCrumb)_crumbList.GetByIndex(index);
            linkString.Append(String.Format("<a href = {0} >{1} </a>", pageCrumb.Url, pageCrumb.LinkName));
            linkString.Append(" > ");
        }
        //'Add the tail also
        pageCrumb = (PageCrumb)_crumbList.GetByIndex(index);
        linkString.Append(pageCrumb.LinkName);

        lblTrail.Text = linkString.ToString();

    }
}

public struct PageCrumb
{
    private int _level;
    private string _url;
    private string _linkName;

    //We are setting all the properties at the time of construction

    public PageCrumb(int level, string url, string linkName)
    {
        _level = level;
        _url = url;
        _linkName = linkName;
    }

    //We are making all the properties as read-only.
    //We are not expecting it to change once it is set.
    public int Level
    {
        get { return _level; }
    }

    public string Url
    {
        get { return _url; }
    }

    public string LinkName
    {
        get { return _linkName; }
    }


}