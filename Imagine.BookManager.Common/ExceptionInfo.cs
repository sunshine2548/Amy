using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagine.BookManager.Common
{
    public class ExceptionInfo
    {
        #region TeacherAllocation

        public const string TeacherNotExists = "该教师不存在";
        public const string PleaseEnterCredit = "请输入额度";
        public const string PurchasedNoteNotExists = "购买记录不存在";
        public const string CreditOverLimit = "您输入的绘本额度过大，请重新输入";
        public const string CreditNoteNotExists = "该分配记录不存在";

        #endregion
    }
}
