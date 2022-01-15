using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models.Repository
{
    public class UserTableRepository : IUserTableRepository, IDisposable
    {
        private bool disposedValue;

        //開啟資料庫連線。
        public MVC_UserDBContext _db = new MVC_UserDBContext();

        //新增 (Create) 實作。
        public bool AddUser(UserTable _userTable)
        {
            try
            {
                _db.UserTables.Add(_userTable);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

            //throw new NotImplementedException();
        }

        //刪除 (Delete) 實作。
        public bool DeleteUser(int _ID)
        {
            try
            {
                UserTable ut = _db.UserTables.Find(_ID);
                _db.UserTables.Remove(ut);
                _db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
            //throw new NotImplementedException();
        }

        //編輯 (Edit) 實作。
        public bool EditUser(UserTable userTable)
        {
            throw new NotImplementedException();
        }

        //明細 (Details) 實作。
        public UserTable GetUserById(int id)
        {
            return (_db.UserTables.Find(id));
            //throw new NotImplementedException();
        }

        //搜尋 (Search) 實作。
        public IQueryable <UserTable> GetUserByName(string id)
        {
            return (_db.UserTables.Where(s => s.UserName.Contains(id)));
            //throw new NotImplementedException();
        }

        //主表 (Master) 實作。
        public IQueryable <UserTable> ListAllUsers()
        {
            return (_db.UserTables);
            //throw new NotImplementedException();
        }

        //關閉資料庫並釋放出資源。
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                // TODO: 釋出非受控資源 (非受控物件) 並覆寫完成項
                // TODO: 將大型欄位設為 Null
                disposedValue = true;
            }
        }

        // // TODO: 僅有當 'Dispose(bool disposing)' 具有會釋出非受控資源的程式碼時，才覆寫完成項
        // ~UserTableRepository()
        // {
        //     // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}