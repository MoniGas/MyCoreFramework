using MCF.Data.Orm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCF.Model
{
    /// <summary>
    /// BBS_User:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class BBS_User : EntityBase
    {
        public BBS_User()
        { }
        #region Model
        private int _id;
        private string _fullname;
        private string _certinum;
        private string _guardianname;
        private string _guardianphone;
        private string _guardiancertinum;
        private int? _userstate;
        private DateTime? _addtime;
        private DateTime? _logintime;
        private int? _subcount;
        private int? _isdel;
        private string _street;
        private int? _unsatisfactory;
        public int? Unsatisfactory
        {
            get { return _unsatisfactory; }
            set { _unsatisfactory = value; }
        }
        public string Street
        {
            get { return _street; }
            set { _street = value; }
        }
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string FullName
        {
            set { _fullname = value; }
            get { return _fullname; }
        }
        /// <summary>
        /// 证件号码
        /// </summary>
        public string CertiNum
        {
            set { _certinum = value; }
            get { return _certinum; }
        }
        /// <summary>
        /// 监护人姓名
        /// </summary>
        public string GuardianName
        {
            set { _guardianname = value; }
            get { return _guardianname; }
        }
        /// <summary>
        /// 监护人手机号
        /// </summary>
        public string GuardianPhone
        {
            set { _guardianphone = value; }
            get { return _guardianphone; }
        }
        /// <summary>
        /// 监护人证件号码
        /// </summary>
        public string GuardianCertiNum
        {
            set { _guardiancertinum = value; }
            get { return _guardiancertinum; }
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int? UserState
        {
            set { _userstate = value; }
            get { return _userstate; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime? AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LoginTime
        {
            set { _logintime = value; }
            get { return _logintime; }
        }
        /// <summary>
        /// 提问次数
        /// </summary>
        public int? SubCount
        {
            set { _subcount = value; }
            get { return _subcount; }
        }
        /// <summary>
        /// 封禁用户：1已封禁 0正常
        /// </summary>
        public int? isDel
        {
            set { _isdel = value; }
            get { return _isdel; }
        }
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        public override string GetTableName()
        {
            return "BBS_User";
        }

        /// <summary>
        /// 获取主键ID
        /// </summary>
        /// <returns></returns>
        public override string GetPkField()
        {
            return "ID";
        }

        /// <summary>
        /// 获取自增列
        /// </summary>
        /// <returns></returns>
        public override string GetIdenField()
        {
            return "ID";
        }
        #endregion Model

    }
}
