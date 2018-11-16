
using System;

namespace employee_api.Models
{
     public class CommonProperties 
    {
        public object DBReturnResult {  get; set; }
        public string EmpDOB { get; set; }
        public string EmpAlternateEmail { get; set; }
        public string EmpMaritalStatus { get; set; }
        public int _OperationFlag { get; set; }
        public bool _OperationType { get; set; }
        public int _OperationMode { get; set; }
        public int OperationMode { get; set; }
        public string _ReturnValue { get; set; }
        public int _D2Ktimestamp { get; set; }
        public string _ApprovedBy { get; set; }
        public string _CreatedBy { get; set; }
        public string _ModifyBy { get; set; }
        public DateTime _dtDateCreated { get; set; }
        public DateTime _dtDateModified { get; set; }
        public DateTime _dtStatusDate { get; set; }
        public DateTime _dateCreatedmodified { get; set; }
        public string _CreatetedModifiedBy { get; set; }
        public int _EffectiveFromTimeKey { get; set; }
        public int _EffectiveToTimeKey { get; set; }
        public string _DelSta { get; set; }

        public int _AccountEntityId { get; set; }
        public string _AuthMode { get; set; }
        public string _glpr_BlnSCD2 { get; set; }
        public int _TimeKey { get; set; }
        public int _LastQtrDateKey { get; set; }
        public int MocFromTimeKey { get; set; }


        public string _AuthorisationStatus { get; set; }
        public int _MenuID { get; set; }
        public string _Remark { get; set; }
        public string _ChangedFields { get; set; }

        //User Login Deails Properties

        public string _active { get; set; }
        public string _allowLogin { get; set; }
        public Int16 _expiredUserDay { get; set; }
        public Int16 _maxUserLogin { get; set; }
        public string _suspended { get; set; }
        public string _expired { get; set; }
        public string _passwordChngd { get; set; }
        public string _roleDesc { get; set; }
        public string _userLocation { get; set; }
        public string _userLocationCode { get; set; }
        public string _userLocationName { get; set; }
        public Int16 _userLoginCount { get; set; }
        public string _userName { get; set; }
        public string _userLoginId { get; set; }
        public Int16 _userRoleKey { get; set; }
        public Int16 _userAltKey { get; set; }
        public string _LastLoginTime { get; set; }
        public string _MonthFirstDate { get; set; }
        public DateTime _MonthLastDate { get; set; }
        public DateTime _StartDate { get; set; }
        public string _MonthName { get; set; }
        public string _MonthYear { get; set; }
        public string _Year { get; set; }
        public string _Month { get; set; }
        public short _MonthEndTimeKey { get; set; }
        public Nullable<short> _PrvTimeKey { get; set; }
        public string _CurrentStatus { get; set; }
        public Nullable<System.DateTime> _DataEffectiveToDate { get; set; }
        public string _CurrentDate { get; set; }

        //Branch Properties

        public string _BranchName { get; set; }
        public string _BranchCode { get; set; }
        public string _BranchType { get; set; }
        public int _SelBranchKey { get; set; }
        public string _RegionName { get; set; }
        public string _ZoneName { get; set; }
        public string _DistrictName { get; set; }
        public string _StateName { get; set; }
        public int _AreaAltKey { get; set; }
        public int _AreaStatus { get; set; }
        public string _AreaDescription { get; set; }
        public string _MOCLock { get; set; }
        public string _MnthFreez { get; set; }
        public string _Mechanize { get; set; }
        public int _MonthUpd { get; set; }
        public string _FinYear { get; set; }
        public string _CurrentFinYear { get; set; }
        public string _BusiRuleFlag { get; set; }
        public char _AllowPreDisb { get; set; }
        public string _BranchBankKey { get; set; }
        public string _ParentBranchBankKey { get; set; }
        public int _RegionKey { get; set; }
        public int _RegionAltKey { get; set; }
        public int _ZoneKey { get; set; }
        public int _ZoneAltKey { get; set; }
        public int _DistrictKey { get; set; }
        public string _DistrictAltKey { get; set; }
        public int _StateKey { get; set; }
        public int _BranchKey { get; set; }
        public string _stateAltKey { get; set; }
        public string _UtilFlag { get; set; }
        public string _RIName { get; set; }
        public int _InspecCenterAlt_Key { get; set; }

     
    }
      public class sysSolutionParam
    {
        public string TierValue { get; set; }
        public string ZoneCap { get; set; }
        public string RegionCap { get; set; }
        public string BranchCap { get; set; }
        public string CommercialMemberID { get; set; }
        public string CommercialMemberName { get; set; }
        public string CommercialMemberPwd { get; set; }
        public string ConsumerMemberID { get; set; }
        public string ConsumerMemberName { get; set; }
        public string ConsumerMemberPwd { get; set; }
        public string BranchDisplayCount { get; set; }
        public string SanctionPeriod { get; set; }
        public string RevalidationPeriod { get; set; }
        public string LimitMargin { get; set; }
        public string PrefIntRate { get; set; }
        public string AllowUserMaintToAdmin { get; set; }
        public string ZonalAuthorisationLimit { get; set; }
        public string ClientName { get; set; }
        public string SecurityParamater { get; set; }
        public string GovtGuarantee { get; set; }
        public string OverdueComputation { get; set; }
        public string DEGUPG { get; set; }
        public string APPAVAIL { get; set; }
        public string NPADataUpdate { get; set; }
        public string MKAPPAVAIL { get; set; }
        public string BankEmpIdUIdSame { get; set; }
        public string QtrDefinition { get; set; }
        public string DispRejectedRecordinDays { get; set; }
        public string ReportServerUserID { get; set; }
        public string ReportServerPassword { get; set; }
        public string ReportServer { get; set; }
        public string ClientFullName { get; set; }
        public string AuthType { get; set; }
        public string DefaultPasswordAllow { get; set; }
        public string ExternalRatingAppl { get; set; }
        public string BaselIITermLogic { get; set; }
        public string HOBranchCode { get; set; }
        public string InternalRatingApplicability { get; set; }
        public string InternalRatingType { get; set; }
        public string RatingScoreApplicability { get; set; }
        public string ReportFolderName { get; set; }
        public string DefaultPassword { get; set; }
        public string UnderMaintenance { get; set; }
        public string RenderReport { get; set; }
        public string CreateUserFolder { get; set; }
        public string Version { get; set; }
        public string PackageName { get; set; }
    }
}