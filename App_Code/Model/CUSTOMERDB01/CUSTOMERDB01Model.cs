using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model.CUSTOMERDB01
{
	public class CUSTOMERDB01Model
    {

		public class CustomerGeneralModel
		{
			public int ID { get; set; }
			public int CustomerTypeID { get; set; }
			public DateTime BirthDate { get; set; }
			public decimal CISNumber { get; set; }
			public int TitleID { get; set; }
			public string NameInENG { get; set; }
			public string SurnameInENG { get; set; }
			public string NickName { get; set; }
			public string NameInTHAI { get; set; }
			public string SurnameInTHAI { get; set; }
			public int SexID { get; set; }
			public int MaritalStatusID { get; set; }
			public int CardTypeID { get; set; }
			public string IDCard { get; set; }
			public string IDCardIssued { get; set; }
			public DateTime IDCardExpiredDate { get; set; }
			public string EmailAddress { get; set; }
			public int ResidentalStatusID { get; set; }
			public int ResidentalYear { get; set; }
			public int ResidentalMonth { get; set; }
			public int NoOfChildren { get; set; }
			public int NoOfFamily { get; set; }
			public string RecordStatus { get; set; }
			public string Application { get; set; }
			public string CreateBy { get; set; }
			public DateTime CreateDate { get; set; }
			public string UpdateBy { get; set; }
			public DateTime UpdateDate { get; set; }
			public string IsDelete { get; set; }
			public DateTime SysStartTime { get; set; }
			public DateTime SysEndTime { get; set; }
			public string ContactTime { get; set; }

		}

	}
  
}