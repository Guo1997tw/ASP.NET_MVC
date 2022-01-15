using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication3.Models.Repository
{
    internal interface IUserTableRepository
    {
        //Interface (介面) 建置以下 Method (方法)：

        //主表 (Master) 方法。
        IQueryable <UserTable> ListAllUsers();

        //明細 (Details) 方法。
        UserTable GetUserById(int id);

        //新增 (Create) 方法。
        bool AddUser(UserTable _userTable); //成功傳True，失敗傳False。

        //修改 (Edit) 方法。
        bool EditUser(UserTable userTable); //成功傳True，失敗傳False。

        //刪除 (Delete) 方法。
        bool DeleteUser(int _ID); //成功傳True，失敗傳False。

        //搜尋 (Search) 方法。
        IQueryable <UserTable> GetUserByName(string id);
    }
}