using System;
using Imagine.BookManager.Core.Entity;

namespace Imagine.BookManager.Dto.Admin
{
    public class AdminLoginResult
    {
        public Guid UerId { get; set; }
        public int ErrorCount { get; set; }
        public LoginResult LoginResult { get; set; }
        public int LockMintues { get; set; }
        public UserType UserType { get; set; }
    }

    public enum LoginResult
    {
        Success = 1,
        AccountLock = 2,
        PwdError = 3
    }
}
