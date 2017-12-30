using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Common
{
    public class ExceptionInfo
    {
        #region Admin

        public const string PwdError = "密码错误";
        public const string UserNotExists = "该用户不存在";
        public const string UserExists = "该用户已存在";

        #endregion

        #region Institution

        public const string InstitutionExists = "该机构已存在";
        public const string InstitutionNotExists = "该机构不存在";

        #endregion

        #region Class

        public const string ClassNotExists = "该班级不存在";

        #endregion

        #region Order

        public const string OrderRefExists = "该订单号已存在";
        public const string OrderNotExistsOrIsPaid = "该订单不存在或该订单已支付";

        #endregion

        #region TeacherAllocation

        public const string TeacherNotExists = "该教师不存在";
        public const string PleaseEnterCredit = "请输入额度";
        public const string PurchasedNoteNotExists = "购买记录不存在";
        public const string CreditOverLimit = "您输入的绘本额度过大，请重新输入";
        public const string CreditNoteNotExists = "该分配记录不存在";

        #endregion

        #region Student

        public const string StudentExists = "该学生已存在";
        public const string StudentNotExists = "该学生不存在";

        #endregion

        #region Set

        public const string SetNameExists = "该绘本名称已存在";

        #endregion
    }
}
