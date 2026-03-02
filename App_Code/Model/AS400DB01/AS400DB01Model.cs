using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ILSystem.App_Code.Model.AS400DB01
{
	public class AS400DB01Model
	{
		public class GNTB16
		{
			public string GN16CD { get; set; }
			public string GN16DT { get; set; }
			public string GN16DE { get; set; }
			public string GN16FL { get; set; }
			public decimal GN16UD { get; set; }
			public decimal GN16UT { get; set; }
			public string GN16US { get; set; }
			public string GN16WS { get; set; }
			public string GN16DL { get; set; }
		}
		public class GNTB61
		{
			public string GN61CD { get; set; }
			public string GN61DT { get; set; }
			public string GN61DE { get; set; }
			public string GN61FL { get; set; }
			public decimal GN61UD { get; set; }
			public decimal GN61UT { get; set; }
			public string GN61US { get; set; }
			public string GN61WS { get; set; }
			public string GN61DL { get; set; }
		}
		public class GNTS16
		{
			public string GS16CD { get; set; }
			public string GS16DT { get; set; }
			public string GS16DE { get; set; }
			public string GS16FL { get; set; }
			public decimal GS16UD { get; set; }
			public decimal GS16UT { get; set; }
			public string GS16US { get; set; }
			public string GS16WS { get; set; }
			public string GS16DL { get; set; }
		}
		public class ILTB40
		{
			public string T40LTY { get; set; }
			public decimal T40TYP { get; set; }
			public string T40DES { get; set; }
			public string T40RSV { get; set; }
			public decimal T40UDD { get; set; }
			public decimal T40UDT { get; set; }
			public string T40PGM { get; set; }
			public string T40USR { get; set; }
			public string T40DSP { get; set; }
			public string T40DEL { get; set; }
		}
		public class CustomerResponse
		{
			public string csn { get; set; }
			public string Title { get; set; }
			public string IDCard { get; set; }
			public string name { get; set; }
			public string surname { get; set; }
			public string appdate { get; set; }
			public string birthdate { get; set; }
			public string expiredate { get; set; }
			public string gender { get; set; }
			public string Mobile { get; set; }
			public string P1APNO { get; set; }
			public decimal P1CSNO { get; set; }
			public decimal P1VDID { get; set; }
			public decimal Vendor { get; set; }
			public decimal P1CAMP { get; set; }
			public decimal P1ITEM { get; set; }
			public string P1RESN { get; set; }
			public string BureauResult { get; set; }
			public string P10NAM { get; set; }
			public string P10FI1 { get; set; }
			public string C01CNM { get; set; }
			public string T44DES { get; set; }
			public string G25DES { get; set; }
			public string G101ED { get; set; }
			public string P1PROG { get; set; }
			public string P1TERM { get; set; }
			public string faxdate { get; set; }
			public decimal faxtime { get; set; }
			public string m00cst { get; set; }
			public string t40des { get; set; }
		}
		public class RLMS02
		{
			public decimal P2BRN { get; set; }
			public decimal P2CSNO { get; set; }
			public decimal P2CONT { get; set; }
			public decimal P2CUST { get; set; }
			public string P2LNTY { get; set; }
			public string P2GRUP { get; set; }
			public decimal P2APNO { get; set; }
			public decimal P2APV { get; set; }
			public decimal P2APYR { get; set; }
			public string P2CRCD { get; set; }
			public string P2ATCD { get; set; }
			public string P2LOCA { get; set; }
			public string P2CAMP { get; set; }
			public decimal P2RCLA { get; set; }
			public decimal P2RCLB { get; set; }
			public string P2DEFE { get; set; }
			public string P2PRCR { get; set; }
			public decimal P2PRC { get; set; }
			public string P2RCCD { get; set; }
			public string P2VDID { get; set; }
			public decimal P2TERM { get; set; }
			public decimal P2DTE1 { get; set; }
			public decimal P2CUTD { get; set; }
			public decimal P2APDT { get; set; }
			public decimal P2RFDT { get; set; }
			public decimal P2CNDT { get; set; }
			public decimal P2BKDT { get; set; }
			public string P2CTTY { get; set; }
			public decimal P2FEE { get; set; }
			public decimal P2FEEB { get; set; }
			public decimal P2DUTY { get; set; }
			public decimal P2DUTB { get; set; }
			public decimal P2NCBA { get; set; }
			public decimal P2NCBB { get; set; }
			public decimal P2FEED { get; set; }
			public decimal P2INFR { get; set; }
			public decimal P2INTR { get; set; }
			public decimal P2CRUR { get; set; }
			public decimal P2TOAM { get; set; }
			public decimal P2OSAM { get; set; }
			public decimal P2PCAM { get; set; }
			public decimal P2PCBL { get; set; }
			public decimal P2PRIN { get; set; }
			public decimal P2PRIB { get; set; }
			public decimal P21DUE { get; set; }
			public decimal P2FDAM { get; set; }
			public decimal P2INST { get; set; }
			public decimal P2MAMT { get; set; }
			public decimal P2CRUA { get; set; }
			public decimal P2CRUB { get; set; }
			public decimal P2INTA { get; set; }
			public decimal P2UIDA { get; set; }
			public decimal P2UIDB { get; set; }
			public decimal P2UBAS { get; set; }
			public decimal P2UTIN { get; set; }
			public decimal P2UINT { get; set; }
			public decimal P2UCR { get; set; }
			public decimal P2UPNT { get; set; }
			public decimal P2LSTM { get; set; }
			public decimal P2ICSD { get; set; }
			public decimal P2PCBD { get; set; }
			public decimal P2LPCD { get; set; }
			public decimal P2LPCB { get; set; }
			public decimal P2TINC { get; set; }
			public decimal P2ATRM { get; set; }
			public decimal P2AINC { get; set; }
			public decimal P2AINT { get; set; }
			public decimal P2ACR { get; set; }
			public decimal P2LADT { get; set; }
			public decimal P2WLET { get; set; }
			public decimal P2ODTM { get; set; }
			public decimal P2ODAM { get; set; }
			public decimal P2ODDY { get; set; }
			public decimal P2SODD { get; set; }
			public decimal P2PENL { get; set; }
			public decimal P2SIAD { get; set; }
			public string P2STIA { get; set; }
			public string P2LGFG { get; set; }
			public string P2PTFG { get; set; }
			public string P2ACFG { get; set; }
			public string P2STFG { get; set; }
			public string P2RESN { get; set; }
			public decimal P2SDTE { get; set; }
			public string P2INFG { get; set; }
			public string P2MTRF { get; set; }
			public decimal P2TRFD { get; set; }
			public string P2TRMY { get; set; }
			public decimal P2LICD { get; set; }
			public decimal P2LPPD { get; set; }
			public decimal P2LPDT { get; set; }
			public decimal P2LMVD { get; set; }
			public string P2AUTO { get; set; }
			public string P2COL { get; set; }
			public string P2TEAM { get; set; }
			public string P2CIC { get; set; }
			public string P2FILL { get; set; }
			public decimal P2UPDT { get; set; }
			public decimal P2UPTM { get; set; }
			public string P2USER { get; set; }
			public string P2DDSP { get; set; }
			public string P2UPGM { get; set; }
			public string P2REST { get; set; }
			public decimal P2RFCN { get; set; }
			public decimal P2RFIR { get; set; }
			public decimal P2RFCR { get; set; }
			public decimal P2RFPB { get; set; }
			public decimal P2RFIB { get; set; }
			public decimal P2RFCB { get; set; }
			public decimal P2HBRN { get; set; }
			public decimal P2PBRN { get; set; }
			public string P2PSPA { get; set; }
			public decimal P2SBRN { get; set; }
			public string P2SSPA { get; set; }
			public decimal P2RAAM { get; set; }
			public string P2DEL { get; set; }
		}
		public class ILMS02
		{
			public decimal P2BRN { get; set; }
			public decimal P2CONT { get; set; }
			public string P2LNTY { get; set; }
			public decimal P2CSNO { get; set; }
			public decimal P2APNO { get; set; }
			public string P2APPT { get; set; }
			public string P2CRCD { get; set; }
			public string P2ATCD { get; set; }
			public string P2LOCA { get; set; }
			public string P2PRCR { get; set; }
			public decimal P2PRC {get;set;}
			public decimal P2VDID { get; set; }
			public decimal P2MKID { get; set; }
			public decimal P2CAMP { get; set; }
			public decimal P2CMSQ { get; set; }
			public string P2CMCT { get; set; }
			public decimal P2ITEM { get; set; }
			public decimal P2PRIC { get; set; }
			public decimal P2QTY { get; set; }
			public decimal P2PURC { get; set; }
			public decimal P2VATR { get; set; }
			public decimal P2VATA { get; set; }
			public decimal P2DOWN { get; set; }
			public decimal P2DISC { get; set; }
			public decimal P2TERM { get; set; }
			public decimal P2RANG { get; set; }
			public decimal P2NDUE { get; set; }
			public decimal P2DTE1 { get; set; }
			public decimal P2CNDT { get; set; }
			public decimal P2BKDT { get; set; }
			public decimal P2BFAM { get; set; }
			public decimal P2BFBL { get; set; }
			public decimal P2BFWV { get; set; }
			public decimal P2HFAM { get; set; }
			public decimal P2HFBL { get; set; }
			public decimal P2HFWV { get; set; }
			public decimal P2LNDR { get; set; }
			public decimal P2DUTR { get; set; }
			public decimal P2INFR { get; set; }
			public decimal P2INTR { get; set; }
			public decimal P2CRUR { get; set; }
			public decimal P2TOAM { get; set; }
			public decimal P2OSAM { get; set; }
			public decimal P2PCAM { get; set; }
			public decimal P2PCBL { get; set; }
			public decimal P21DUE { get; set; }
			public decimal P2FDAM { get; set; }
			public decimal P2DIFF { get; set; }
			public decimal P2DIFB { get; set; }
			public decimal P2FRTM { get; set; }
			public decimal P2FRDT { get; set; }
			public decimal P2FRAM { get; set; }
			public decimal P2CYCC { get; set; }
			public decimal P2DUTY { get; set; }
			public decimal P2DUTB { get; set; }
			public decimal P2DTWV { get; set; }
			public decimal P2FEE { get; set; }
			public decimal P2FEEB { get; set; }
			public decimal P2UFEB { get; set; }
			public decimal P2FEIB { get; set; }
			public decimal P2CRUA { get; set; }
			public decimal P2CRUB { get; set; }
			public decimal P2UCRB { get; set; }
			public decimal P2UCIB { get; set; }
			public decimal P2UIDA { get; set; }
			public decimal P2INTB { get; set; }
			public decimal P2UBAS { get; set; }
			public decimal P2UIDB { get; set; }
			public decimal P2MBAS { get; set; }
			public decimal P2MAMT { get; set; }
			public decimal P2MBAL { get; set; }
			public decimal P2MINB { get; set; }
			public decimal P2VBAS { get; set; }
			public decimal P2VAMT { get; set; }
			public decimal P2VBAL { get; set; }
			public decimal P2VINB { get; set; }
			public decimal P2CMBS { get; set; }
			public decimal P2CMAM { get; set; }
			public decimal P2CMBL { get; set; }
			public decimal P2CEXB { get; set; }
			public decimal P2WLET { get; set; }
			public decimal P2ODTM { get; set; }
			public decimal P2ODAM { get; set; }
			public decimal P2ODPC { get; set; }
			public decimal P2SODT { get; set; }
			public decimal P2ODT2 { get; set; }
			public decimal P2ODA2 { get; set; }
			public decimal P2ODP2 { get; set; }
			public decimal P2SOD2 { get; set; }
			public decimal P2ODDY { get; set; }
			public decimal P2SODD { get; set; }
			public decimal P2MXOD { get; set; }
			public decimal P2MOD2 { get; set; }
			public decimal P2PENL { get; set; }
			public decimal P2PNRC { get; set; }
			public decimal P2PNWV { get; set; }
			public decimal P2SIAD { get; set; }
			public string P2STIA { get; set; }
			public string P2LGFG { get; set; }
			public string P2PTFG { get; set; }
			public string P2ACFG { get; set; }
			public decimal P2ACDT { get; set; }
			public decimal P2STAD { get; set; }
			public string P2MKAC { get; set; }
			public decimal P2MADT { get; set; }
			public decimal P2MSDT { get; set; }
			public string P2VDAC { get; set; }
			public decimal P2VADT { get; set; }
			public decimal P2VSDT { get; set; }
			public string P2EXAC { get; set; }
			public decimal P2EADT { get; set; }
			public decimal P2ESDT { get; set; }
			public string P2STBL { get; set; }
			public string P2STDC { get; set; }
			public string P2SUSR { get; set; }
			public string P2RJLB { get; set; }
			public string P2RJST { get; set; }
			public decimal P2SDTE { get; set; }
			public string P2RESN { get; set; }
			public string P2INFG { get; set; }
			public decimal P2LPPD { get; set; }
			public decimal P2LSTM { get; set; }
			public string P2LSTY { get; set; }
			public decimal P2LPDT { get; set; }
			public decimal P2LPTM { get; set; }
			public decimal P2LMVD { get; set; }
			public decimal P2SOST { get; set; }
			public decimal P2SOSD { get; set; }
			public decimal P2SOSA { get; set; }
			public decimal P2SPCP { get; set; }
			public decimal P2SINT { get; set; }
			public decimal P2SCRU { get; set; }
			public decimal P2SFEE { get; set; }
			public string P2COL { get; set; }
			public string P2TEAM { get; set; }
			public string P2CIC { get; set; }
			public decimal P2TMDT { get; set; }
			public string P2TMNO { get; set; }
			public string P2FILL { get; set; }
			public decimal P2UPDT { get; set; }
			public decimal P2UPTM { get; set; }
			public string P2PROG { get; set; }
			public string P2USER { get; set; }
			public string P2DDSP { get; set; }
			public string P2DEL { get; set; }

		}
		public class ILMS10
		{
			public decimal P10VEN { get; set; }
			public string P10TIC { get; set; }
			public string P10TNM { get; set; }
			public string P10NAM { get; set; }
			public string P10ADR { get; set; }
			public string P10VIL { get; set; }
			public string P10BIL { get; set; }
			public string P10BUD { get; set; }
			public string P10ROM { get; set; }
			public string P10FLO { get; set; }
			public string P10SOI { get; set; }
			public string P10ROD { get; set; }
			public string P10MOO { get; set; }
			public decimal P10TMC { get; set; }
			public decimal P10AMC { get; set; }
			public decimal P10PVC { get; set; }
			public decimal P10ZIP { get; set; }
			public string P10AD2 { get; set; }
			public string P10VI2 { get; set; }
			public string P10BI2 { get; set; }
			public string P10BU2 { get; set; }
			public string P10RM2 { get; set; }
			public string P10FL2 { get; set; }
			public string P10SO2 { get; set; }
			public string P10RD2 { get; set; }
			public string P10MO2 { get; set; }
			public decimal P10TM2 { get; set; }
			public decimal P10AM2 { get; set; }
			public decimal P10PV2 { get; set; }
			public decimal P10ZI2 { get; set; }
			public string P10A31 { get; set; }
			public string P10A32 { get; set; }
			public string P10A33 { get; set; }
			public string P10STS { get; set; }
			public string P10GRD { get; set; }
			public decimal P10MOU { get; set; }
			public string P10REG { get; set; }
			public string P10TAX { get; set; }
			public string P10TE1 { get; set; }
			public string P10TLR { get; set; }
			public string P10EXT { get; set; }
			public string P10TE2 { get; set; }
			public string P10TR2 { get; set; }
			public string P10EX2 { get; set; }
			public decimal P10FX1 { get; set; }
			public string P10F1T { get; set; }
			public decimal P10FX2 { get; set; }
			public string P10F2T { get; set; }
			public string P10RES { get; set; }
			public string P10POT { get; set; }
			public string P10RDP { get; set; }
			public string P10RE2 { get; set; }
			public string P10PO2 { get; set; }
			public string P10RP2 { get; set; }
			public decimal P10FJD { get; set; }
			public decimal P10JDT { get; set; }
			public decimal P10EDT { get; set; }
			public decimal P10PVN { get; set; }
			public decimal P10BPY { get; set; }
			public string P10TXR { get; set; }
			public string P10PYE { get; set; }
			public string P10PTY { get; set; }
			public string P10BCD { get; set; }
			public string P10BNO { get; set; }
			public string P10BRG { get; set; }
			public string P10DLV { get; set; }
			public string P10HED { get; set; }
			public string P10DTX { get; set; }
			public string P10SFG { get; set; }
			public string P10CLD { get; set; }
			public string P10RF1 { get; set; }
			public decimal P10CRD { get; set; }
			public string P10MKD { get; set; }
			public string P10TAV { get; set; }
			public decimal P10LTM { get; set; }
			public string P10SAL { get; set; }
			public string P10CTY { get; set; }
			public string P10DTY { get; set; }
			public decimal P10CPD { get; set; }
			public decimal P10BRN { get; set; }
			public decimal P10DT1 { get; set; }
			public decimal P10DT2 { get; set; }
			public string P10FI1 { get; set; }
			public string P10FIL { get; set; }
			public decimal P10UPD { get; set; }
			public decimal P10TIM { get; set; }
			public string P10USR { get; set; }
			public string P10PGM { get; set; }
			public string P10DSP { get; set; }
			public string P10DEL { get; set; }
			public string P10ATS { get; set; }
			public string P10FIX { get; set; }
			public string P10SPC { get; set; }
		}
		public class DDCampaign
        {
			public decimal c01cmp { get; set; }
			public string c01cnm { get; set; }
			public decimal c02csq { get; set; }
		}
		
		public class DDProduct
        {
			public decimal t44itm { get; set; }
			public string t44des { get; set;}
			public string t40des { get; set; }
        }
		public class GNTS17
		{
			public string GS17CD { get; set; }
			public string GS17SC { get; set; }
			public string GS17DT { get; set; }
			public string GS17DE { get; set; }
			public string GS17FL { get; set; }
			public decimal GS17UD { get; set; }
			public decimal GS17UT { get; set; }
			public string GS17US { get; set; }
			public string GS17WS { get; set; }
			public string GS17DL { get; set; }
		}
		public class GNTB24
		{
			public string G24ACD { get; set; }
			public string G24DES { get; set; }
			public string G24FIL { get; set; }
			public decimal G24UDT { get; set; }
			public decimal G24UTM { get; set; }
			public string G24USR { get; set; }
			public string G24DSP { get; set; }
			public string G24DEL { get; set; }
		}
		public class GNTB25
		{
			public string G25RCD { get; set; }
			public string G25DES { get; set; }
			public string G25FIL { get; set; }
			public decimal G25UDT { get; set; }
			public decimal G25UTM { get; set; }
			public string G25USR { get; set; }
			public string G25DSP { get; set; }
			public string G25DEL { get; set; }
		}
		public class CustomerTelephone
		{
			public int ID { get; set; }
			public int CustID { get; set; }
			public int CustRefID { get; set; }
			public int AddressCodeID { get; set; }
			public int TelephoneTypeID { get; set; }
			public string TelephoneNumber { get; set; }
			public string ExtensionNumber { get; set; }
			public int Sequence { get; set; }
			public string RecordStatus { get; set; }
			public string Application { get; set; }
			public string CreateBy { get; set; }
			public DateTime CreateDate { get; set; }
			public string UpdateBy { get; set; }
			public DateTime UpdateDate { get; set; }
			public string IsDelete { get; set; }
			public DateTime SysStartTime { get; set; }
			public DateTime SysEndTime { get; set; }
			public int Seq { get; set; }
			public int TelephoneTypeOtherID { get; set; }
		}

		public class ILTB01
		{
			public decimal T1BRN { get; set; }
			public string T1BNME { get; set; }
			public string T1BNMT { get; set; }
			public decimal T1CMCD { get; set; }
			public string T1EAD1 { get; set; }
			public string T1EAD2 { get; set; }
			public string T1TAD1 { get; set; }
			public string T1TAD2 { get; set; }
			public decimal T1ESDT { get; set; }
			public decimal T1OPDT { get; set; }
			public string T1TEL { get; set; }
			public string T1FAX1 { get; set; }
			public string T1FAX2 { get; set; }
			public string T1AREA { get; set; }
			public string T1CTNM { get; set; }
			public string T1CTEL { get; set; }
			public string T1LCCD { get; set; }
			public string T1ACFG { get; set; }
			public decimal T1UPDD { get; set; }
			public decimal T1UPTM { get; set; }
			public string T1USER { get; set; }
			public string T1FILL { get; set; }
			public string T1DEL { get; set; }
		}

		public class ResValidate
		{
			public bool Status { get; set; }
			public string ErrorMsg { get; set; }
		}
        public class ILCP01
        {
            public decimal C01CMP { get; set; }      // numeric(13, 0)
            public string C01LTY { get; set; }       // char(2)
            public decimal C01BRN { get; set; }      // numeric(3, 0)
            public string C01STY { get; set; }       // char(1)
            public string C01SBT { get; set; }       // char(1)
            public string C01CTY { get; set; }       // char(1)
            public string C01RNG { get; set; }       // char(1)
            public string C01CNM { get; set; }       // char(200)
            public decimal C01VDC { get; set; }      // numeric(12, 0)
            public decimal C01MKC { get; set; }      // numeric(12, 0)
            public string C01PTY { get; set; }       // char(150)
            public decimal C01SDT { get; set; }      // numeric(8, 0)
            public decimal C01EDT { get; set; }      // numeric(8, 0)
            public decimal C01CAD { get; set; }      // numeric(8, 0)
            public decimal C01CLD { get; set; }      // numeric(8, 0)
            public decimal C01NXD { get; set; }      // numeric(2, 0)
            public decimal C01FIN { get; set; }      // numeric(3, 0)
            public decimal C01SRT { get; set; }      // numeric(4, 2)
            public decimal C01TRG { get; set; }      // numeric(2, 0)
            public string C01CST { get; set; }       // char(1)
            public decimal C01INV { get; set; }      // numeric(8, 0)
            public string C01MKT { get; set; }       // char(10)
            public string C01WDT { get; set; }       // char(1)
            public decimal C01UDT { get; set; }      // numeric(8, 0)
            public decimal C01UTM { get; set; }      // numeric(8, 0)
            public string C01UUS { get; set; }       // char(10)
            public string C01UPG { get; set; }       // char(10)
            public string C01UWS { get; set; }       // char(10)
            public string C01RST { get; set; }       // char(1)
            public string C01VTY { get; set; }       // char(1)
            public decimal C01VCR { get; set; }      // numeric(3, 0)
            public string C01PMT { get; set; }       // char(150)
        }
        public class ILCP02
        {
            public decimal C02CMP { get; set; }      // numeric(13, 0)
            public decimal C02CSQ { get; set; }      // numeric(2, 0)
            public decimal C02RSQ { get; set; }      // numeric(2, 0)
            public decimal C02FMT { get; set; }      // numeric(2, 0)
            public decimal C02TOT { get; set; }      // numeric(2, 0)
            public decimal C02AIR { get; set; }      // numeric(5, 3)
            public decimal C02ACR { get; set; }      // numeric(5, 3)
            public decimal C02INR { get; set; }      // numeric(4, 2)
            public decimal C02CRR { get; set; }      // numeric(4, 2)
            public decimal C02IFR { get; set; }      // numeric(4, 2)
            public decimal C02INS { get; set; }      // numeric(7, 0)
            public decimal C02SPR { get; set; }      // numeric(13, 2)
            public decimal C02EPR { get; set; }      // numeric(13, 2)
            public decimal C02TTR { get; set; }      // numeric(2, 0)
            public decimal C02TTM { get; set; }      // numeric(2, 0)
            public decimal C02UDT { get; set; }      // numeric(8, 0)
            public decimal C02UTM { get; set; }      // numeric(6, 0)
            public string C02UUS { get; set; }       // char(10)
            public string C02UPG { get; set; }       // char(10)
            public string C02UWS { get; set; }       // char(10)
            public string C02RST { get; set; }       // char(1)
        }
        public class ILCP04
        {
            public decimal C04CMP { get; set; }      // numeric(13, 0)
            public decimal C04PTY { get; set; }      // numeric(2, 0)
            public decimal C04PCD { get; set; }      // numeric(3, 0)
            public decimal C04PIT { get; set; }      // numeric(6, 0)
            public decimal C04UDT { get; set; }      // numeric(8, 0)
            public decimal C04UTM { get; set; }      // numeric(6, 0)
            public string C04UUS { get; set; }       // char(10)
            public string C04UPG { get; set; }       // char(10)
            public string C04UWS { get; set; }       // char(10)
            public string C04RST { get; set; }       // char(1)
        }
        public class ILCP05
        {
            public decimal C05CMP { get; set; }      // numeric(13, 0)
            public decimal C05CSQ { get; set; }      // numeric(2, 0)
            public decimal C05RSQ { get; set; }      // numeric(2, 0)
            public string C05PAR { get; set; }       // char(1)
            public decimal C05PCD { get; set; }      // numeric(12, 0)
            public string C05SBT { get; set; }       // char(1)
            public decimal C05SIR { get; set; }      // numeric(4, 2)
            public decimal C05SCR { get; set; }      // numeric(4, 2)
            public decimal C05SFR { get; set; }      // numeric(4, 2)
            public decimal C05STR { get; set; }      // numeric(4, 2)
            public decimal C05SFM { get; set; }      // numeric(3, 0)
            public decimal C05STO { get; set; }      // numeric(3, 0)
            public decimal C05SST { get; set; }      // numeric(3, 0)
            public decimal C05EST { get; set; }      // numeric(3, 0)
            public decimal C05STM { get; set; }      // numeric(3, 0)
            public decimal C05UDT { get; set; }      // numeric(8, 0)
            public decimal C05UTM { get; set; }      // numeric(6, 0)
            public string C05UUS { get; set; }       // char(10)
            public string C05UPG { get; set; }       // char(10)
            public string C05UWS { get; set; }       // char(10)
            public string C05RST { get; set; }       // char(1)
        }
        public class ILCP06
        {
            public decimal C06CMP { get; set; }      // numeric(13, 0)
            public string C06APT { get; set; }       // char(2)
            public decimal C06UDT { get; set; }      // numeric(8, 0)
            public decimal C06UTM { get; set; }      // numeric(6, 0)
            public string C06UUS { get; set; }       // char(10)
            public string C06UPG { get; set; }       // char(10)
            public string C06UWS { get; set; }       // char(10)
            public string C06RST { get; set; }       // char(1)
        }
        public class ILCP07
        {
            public decimal C07CMP { get; set; }      // numeric(13, 0)
            public decimal C07CSQ { get; set; }      // numeric(2, 0)
            public string C07LNT { get; set; }       // char(2)
            public decimal C07PIT { get; set; }      // numeric(6, 0)
            public string C07FIX { get; set; }       // char(1)
            public decimal C07PRC { get; set; }      // numeric(13, 2)
            public decimal C07MIN { get; set; }      // numeric(13, 2)
            public decimal C07MAX { get; set; }      // numeric(13, 2)
            public decimal C07DOW { get; set; }      // numeric(13, 2)
            public decimal C07UDT { get; set; }      // numeric(8, 0)
            public decimal C07UTM { get; set; }      // numeric(6, 0)
            public string C07UUS { get; set; }       // char(10)
            public string C07UPG { get; set; }       // char(10)
            public string C07UWS { get; set; }       // char(10)
            public string C07RST { get; set; }       // char(1)
        }
        public class ILCP08
        {
            public decimal C08CMP { get; set; }      // numeric(13, 0)
            public decimal C08VEN { get; set; }      // numeric(12, 0)
            public decimal C08UDT { get; set; }      // numeric(8, 0)
            public decimal C08UTM { get; set; }      // numeric(6, 0)
            public string C08UUS { get; set; }       // char(10)
            public string C08UPG { get; set; }       // char(10)
            public string C08UWS { get; set; }       // char(10)
            public string C08RST { get; set; }       // char(1)
        }
        
        public class ILCP09
        {
            public decimal C09CMP { get; set; }      // numeric(13, 0)
            public decimal C09BRN { get; set; }      // numeric(3, 0)
            public decimal C09UDT { get; set; }      // numeric(8, 0)
            public decimal C09UTM { get; set; }      // numeric(6, 0)
            public string C09UUS { get; set; }       // char(10)
            public string C09UPG { get; set; }       // char(10)
            public string C09UWS { get; set; }       // char(10)
            public string C09RST { get; set; }       // char(1)
        }
        public class ILCP11
        {
            public decimal C11CMP { get; set; }      // numeric(13, 0)
            public decimal C11NSQ { get; set; }      // numeric(3, 0)
            public decimal C11LSQ { get; set; }      // numeric(4, 0)
            public string C11NOT { get; set; }       // char(70)
            public decimal C11UDT { get; set; }      // numeric(8, 0)
            public decimal C11UTM { get; set; }      // numeric(6, 0)
            public string C11UUS { get; set; }       // char(10)
            public string C11UPG { get; set; }       // char(10)
            public string C11UWS { get; set; }       // char(10)
            public string C11RST { get; set; }       // char(1)
        }
        public class ILCP99
        {
            public decimal C99CMP { get; set; }      // numeric(13, 0)
            public decimal C99BRN { get; set; }      // numeric(3, 0)
            public string C99CBR { get; set; }       // char(1)
            public string C99EDT { get; set; }       // char(1)
            public string C99UST { get; set; }       // char(1)
            public string C99SPM { get; set; }       // char(10)
            public decimal C99UDT { get; set; }      // numeric(8, 0)
            public decimal C99UTM { get; set; }      // numeric(6, 0)
            public string C99UUS { get; set; }       // char(10)
            public string C99UPG { get; set; }       // char(10)
            public string C99UWS { get; set; }       // char(10)
            public string C99RST { get; set; }       // char(1)
        }
        
        public class ILMS99
        {
            public decimal P99BRN { get; set; }      // numeric(3, 0)
            public string P99LNT { get; set; }       // char(2)
            public string P99REC { get; set; }       // char(3)
            public string P99DES { get; set; }       // char(30)
            public decimal P99RUN { get; set; }      // numeric(16, 0)
            public string P99FIL { get; set; }       // char(7)
            public decimal P99UPD { get; set; }      // numeric(8, 0)
            public decimal P99TIM { get; set; }      // numeric(6, 0)
            public string P99UPG { get; set; }       // char(10)
            public string P99USR { get; set; }       // char(10)
            public string P99DSP { get; set; }       // char(10)
            public string P99DEL { get; set; }       // char(1)
        }
    }
}
