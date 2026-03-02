using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ESB.WebAppl.ILSystem.Models
{
    public class ResponseModel
    {
        public int status { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class ResponseSSOAuthen
    {
        public bool isAccess { get; set; }
        public string accessKey { get; set; }
        public List<string> listPortal { get; set; }
        public string message { get; set; }
    }

    public class ResponseGetCustomer
    {
        public int id { get; set; }
        public int custTypeID { get; set; }
        public int customerTypeID { get; set; }
        public string custDescTypeTH { get; set; }
        public string custDescTypeEN { get; set; }
        public string birthDate { get; set; }
        public string cisNumber { get; set; }
        public string prefixTH { get; set; }
        public string firstNameTH { get; set; }
        public string lastNameTH { get; set; }
        public string prefixEN { get; set; }
        public string firstNameEN { get; set; }
        public string lastNameEN { get; set; }
        public string emailAddress { get; set; }
        public string createBy { get; set; }
        public string createDate { get; set; }
        public string sexTH { get; set; }
        public string sexEN { get; set; }
        public string mariDescTH { get; set; }
        public string mariDescEN { get; set; }
        public string idCard { get; set; }
        public string cardDescTH { get; set; }
        public string cardDescEN { get; set; }
        public string telTypeTH { get; set; }
        public string telTypeEN { get; set; }
        public string telephoneNumber { get; set; }
    }

    public class ResponseGetCustomerAddress
    {
        public string addressCode { get; set; }
        public string addressTypeTH { get; set; }
        public string addressTypeEN { get; set; }
        public string buildingTitleCode { get; set; }
        public string buildingTitleTH { get; set; }
        public string buildingTitleEN { get; set; }
        public string aumphurTH { get; set; }
        public string aumphurEN { get; set; }
        public string tambolTH { get; set; }
        public string tambolEN { get; set; }
        public string provinceTH { get; set; }
        public string provinceEN { get; set; }
        public int id { get; set; }
        public int custID { get; set; }
        public int custRefID { get; set; }
        public int addressCodeID { get; set; }
        public string addressNumber { get; set; }
        public string village { get; set; }
        public int buildingTitleID { get; set; }
        public string buildingName { get; set; }
        public string floor { get; set; }
        public string room { get; set; }
        public string moo { get; set; }
        public string road { get; set; }
        public string soi { get; set; }
        public int amphurID { get; set; }
        public int tambolID { get; set; }
        public int provinceID { get; set; }
        public int postalAreaCode { get; set; }
        public string isShipTo { get; set; }
        public string recordStatus { get; set; }
        public string application { get; set; }
        public string createBy { get; set; }
        public string createDate { get; set; }
        public string updateBy { get; set; }
        public string updateDate { get; set; }
        public string isDelete { get; set; }
        public string sysStartTime { get; set; }
        public string sysEndTime { get; set; }
        public string changedBy { get; set; }
    }

    public class ResponseGetCustomerReference
    {
        public string titleTH { get; set; }
        public string titleEN { get; set; }
        public string referenceTH { get; set; }
        public string referenceEN { get; set; }
        public string relationTH { get; set; }
        public string relationEN { get; set; }
        public string occupationTH { get; set; }
        public string occupationEN { get; set; }
        public int id { get; set; }
        public int custID { get; set; }
        public int referenceID { get; set; }
        public int titleID { get; set; }
        public string nameInENG { get; set; }
        public string surnameInENG { get; set; }
        public string nameInTHAI { get; set; }
        public string surnameInTHAI { get; set; }
        public int relationID { get; set; }
        public int occupationID { get; set; }
        public decimal salaryAMT { get; set; }
        public int sequence { get; set; }
        public string recordStatus { get; set; }
        public string application { get; set; }
        public string createBy { get; set; }
        public string createDate { get; set; }
        public string updateBy { get; set; }
        public string updateDate { get; set; }
        public string isDelete { get; set; }
        public string sysStartTime { get; set; }
        public string sysEndTime { get; set; }
        public string changedBy { get; set; }
    }

    public class ResponseGetTelephoneGrouping
    {
        public string custID { get; set; }
        public List<CustRefLevel> custRefLevel { get; set; }
    }

    public class CustRefLevel
    {
        public string custRefID { get; set; }
        public List<AddressTypeLevel> addressTypeLevel { get; set; }
    }

    public class AddressTypeLevel
    {
        public string addressCodeID { get; set; }
        public string addressCode { get; set; }
        public string addressTypeEN { get; set; }
        public string createBy { get; set; }
        public string createDate { get; set; }
        public string updateBy { get; set; }
        public string updateDate { get; set; }
        public List<TelephoneTypeLevel> telephoneTypeLevel { get; set; }
    }

    public class TelephoneTypeLevel
    {
        public string telephoneTypeID { get; set; }
        public string telephoneCode { get; set; }
        public string telephoneType { get; set; }
        public List<Summary> summary { get; set; }
    }

    public class Summary
    {
        public string telephoneTypeID { get; set; }
        public string telephoneType { get; set; }
        public string telephoneCode { get; set; }
        public int number { get; set; }
        public string telephoneNumber { get; set; }
        public string telephoneRange { get; set; }
        public string extension { get; set; }
    }
}