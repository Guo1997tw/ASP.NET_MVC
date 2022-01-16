using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class UserDBController : Controller
    {
        //開啟資料庫 (UserDB) 連線。
        //當每次使用完資料庫時，有開啟動作後，則必需關閉動作。不然會造成效能問題。
        private Models.MVC_UserDBContext _db = new Models.MVC_UserDBContext();

        //關閉資料庫 (UserDB) 連線。
        //原本寫的virtual => override (可覆蓋)。
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _db.Dispose(); }
            base.Dispose(disposing);
        }

        //CheckSex變數 (判斷M or F的欄位)。
        private bool CheckSex(string sex)
        {
            return sex == "M" || sex == "F";
        }

        //CheckMobilePhone (判斷開頭是否為09及後面為8碼)。
        private bool CheckMobilePhone(string MobilePhone)
        {
            //$數字結尾意思。
            //e.g. 電話號碼結尾 or 身份證結尾。
            //return System.Text.RegularExpressions.Regex.IsMatch(MobilePhone, @"09[0-9]{8}$");
            return System.Text.RegularExpressions.Regex.IsMatch(MobilePhone, @"09\d{8}$");
        }

        // GET: UserDB
        public ActionResult Index()
        {
            return View();
        }

        //主表功能 (Master)
        public ActionResult List()
        {
            //寫法一 - LINQ (IQueryable)：
            //先From _XXX (區域變數) in XXXs (實際資料庫) -> Where (查詢欄位條件) -> Select (輸出結果)。
            IQueryable<UserTable> ListAll = from _usertable in _db.UserTables select _usertable;

            //寫法二 - LINQ (var)：
            //var為可自動轉換輸出的任一值。
            //var ListAll2 = from _usertable in _db.UserTables select _usertable;

            //如果ListAll為空值話，會顯示網頁Error。否則就會回傳ListAll。
            if (ListAll == null)
            {
                return HttpNotFound();
                //return Content("此網頁錯誤，請重新回到上一頁！謝謝。");
            }
            else
            {
                return View(ListAll);
            }
        }

        [HttpGet]

        //明細功能 (Details)
        public ActionResult Details(int? _ID)
        {
            //如果沒有在網頁的URL上方輸入ID話，則會報出Error。
            if (_ID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //寫法一 - LINQ (IQueryable)：
            //先From _XXX (區域變數) in XXXs (實際資料庫) -> Where (查詢欄位條件) -> Select (輸出結果)。
            IQueryable<UserTable> ListOnlyOne = from _usertable in _db.UserTables where _usertable.UserId == _ID select _usertable;

            //如果ListOnlyOne等於空值話，則會報出Error。
            if (ListOnlyOne == null)
            {
                return HttpNotFound();
            }
            else
            {
                //在回傳值時，若沒有加上FirstOrDefault()函式話，則會報出Error。
                return View(ListOnlyOne.FirstOrDefault());
            }
        }

        //新增功能 (檢視畫面)
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")] //可將下方的Create多加上Confirm字眼，用來區別畫面及動作。
        [ValidateAntiForgeryToken] //避免CSRF攻擊。

        //將表單輸入的值回傳至資料庫 (非檢視畫面 - Action)
        //新增時，需傳入整張Table。
        public ActionResult CreateConfirm(UserTable _userTable)
        {

            //如果userTable不等於空值及表單驗證話，則會將該筆資料寫入資料庫。否則會報出Error。
            //if (((_userTable != null) && (ModelState.IsValid)) && ((_userTable.UserSex == "M") || (_userTable.UserSex == "F")))
            //{
            //    _db.UserTables.Add(_userTable);
            //    _db.SaveChanges();

            //    //當新增完成一筆資料後，則畫面將返回主表(Master)。
            //    return RedirectToAction("List");
            //}

            if (((_userTable != null) && (ModelState.IsValid)) && this.CheckSex(_userTable.UserSex) && this.CheckMobilePhone(_userTable.UserMobilePhone))
            {
                _db.UserTables.Add(_userTable);
                _db.SaveChanges();

                //當新增完成一筆資料後，則畫面將返回主表 (Master)。
                return RedirectToAction("List");
            }
            //else
            //{
            //    ModelState.AddModelError("Value1", "訊息錯誤");

            //    return View();
            //}
            return Content("欄位有誤！請返回上一頁並重新填寫。");
        }

        //刪除功能 (檢視畫面)。
        public ActionResult Delete(int? _ID)
        {
            //如果沒有在網頁的URL上方輸入ID話，則會報出Error。
            if (_ID == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //先鎖定輸入的ID。
            UserTable ut = _db.UserTables.Find(_ID);

            //如果輸入的ID沒有話，會報出Error。否則，將顯示出來。
            if (ut == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ut);
            }
        }

        [HttpPost, ActionName("Delete")] //可將下方的Delete多加上Confirm字眼，用來區別畫面及動作。
        [ValidateAntiForgeryToken] //避免CSRF攻擊。

        //刪除該筆資料的動作 (非檢視畫面 - Action)
        public ActionResult DeleteConfirm(int _ID)
        {
            //透過表單驗證，需搭配Models底下的類別檔。
            if (ModelState.IsValid)
            {
                //先鎖定輸入的ID，再進行刪除動作，最後儲存。
                UserTable ut = _db.UserTables.Find(_ID);
                _db.UserTables.Remove(ut);
                _db.SaveChanges();

                return RedirectToAction("List");
            }
            else
            {
                ModelState.AddModelError("Value1", "錯誤訊息");
                return View();
            }
        }

        //編輯功能。
        public ActionResult Edit(int? _ID)
        {
            //如果沒有在網頁的URL上方輸入ID話，則會報出Error。
            if (_ID == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }

            //先鎖定輸入的ID。
            UserTable ut = _db.UserTables.Find(_ID);

            //如果輸入的ID沒有話，會報出Error。否則，將顯示出來。
            if (ut == null) { return HttpNotFound(); }
            else { return View(ut); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //避免CSRF攻擊。

        //白名單：可以輸出需要顯示的欄位。
        //黑名單：可以輸出不需要顯示的欄位。
        public ActionResult Edit([Bind(Include = "UserId, UserName, UserSex, UserBirthDay, UserMobilePhone")] UserTable _userTable)
        {
            //如果輸入ID沒有內容話，則會報出Error。
            if (_userTable == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }

            //if(_userTable.UserSex != "M" || _userTable.UserSex != "F")
            //{
            //    return Content("Error");
            //}

            //if (this.CheckSex(_userTable.UserSex) && this.CheckMobilePhone(_userTable.UserMobilePhone))
            //{
            //    return Content("欄位有誤！請返回上一頁並重新填寫。");
            //}

            //透過表單驗證，需搭配Models底下的類別檔。
            if (ModelState.IsValid && this.CheckSex(_userTable.UserSex) && this.CheckMobilePhone(_userTable.UserMobilePhone))
            {
                _db.Entry(_userTable).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                return RedirectToAction("List");
            }
            //else
            //{
            //    return View(_userTable);
            //}
            return Content("欄位有誤！請返回上一頁並重新填寫。");
        }

        //搜尋功能。
        public ActionResult Search(string _ID)
        {
            string _SearchWord = _ID;

            //以下兩者應應從Controller => View中是存在，則到不同Action後為不存在。
            //ViewBag可儲存於變數值。
            //ViewData可儲存於字串。

            //以下者應應從Controller => View中是存在，則到不同Action後為存在。
            //TempData可儲存於字串。
            ViewData["SW"] = _SearchWord;

            //先From _XXX (區域變數) in XXXs (實際資料庫) -> Where (查詢欄位條件) -> Select (輸出結果)。
            var ListAll = from _userTable in _db.UserTables select _userTable;

            //Contains = LIKE (SQL語法)，兩者為模糊搜尋。
            if (!String.IsNullOrEmpty(_SearchWord) && ModelState.IsValid)
            {
                return View(ListAll.Where(s => s.UserName.Contains(_SearchWord)));
            }
            else
            {
                return HttpNotFound();
            }
        }

        //搜尋多項條件功能 (檢視畫面)
        public ActionResult SearchMulti()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        //搜尋多項條件功能 (非檢視畫面 - Action)
        public ActionResult SearchMulti(UserTable _userTable)
        {
            string uName = _userTable.UserName;
            string uUserMobilePhone = _userTable.UserMobilePhone;

            var ListAll = _db.UserTables.Select(s => s);

            //搜尋條件一
            if (!string.IsNullOrWhiteSpace(uName))
            {
                //使用指定的值搜尋。
                //ListAll = ListAll.Where(s => s.UserName == uName);

                //使用模糊搜尋。
                ListAll = ListAll.Where(s => s.UserName.Contains(uName));
            }

            //搜尋條件二
            if (!string.IsNullOrWhiteSpace(uUserMobilePhone))
            {
                //使用指定的值搜尋。
                //ListAll = ListAll.Where(s => s.UserMobilePhone == uUserMobilePhone);

                //使用模糊搜尋。
                ListAll = ListAll.Where(s => s.UserMobilePhone.Contains(uUserMobilePhone));
            }

            if ((_userTable != null) && (ModelState.IsValid))
            {
                return View("SearchMultiResult", ListAll.ToList());
            }
            else
            {
                return HttpNotFound();
            }
        }

        //分頁功能
        public ActionResult Page(int _ID = 1)
        {
            //網站此頁為5筆資料呈現。
            int PageSize = 5;

            //目前網站此頁觀賞為第幾頁。
            int NowPageCount = 0;

            if (_ID > 0)
            {
                //(選擇想要看的頁數-1) * 一頁所呈現的資料筆數
                NowPageCount = (int)((_ID - 1) * PageSize);
            }

            //Skip -> 從哪分頁開始，Take -> 呈現此頁面有幾筆資料。
            var ListAll = (from _userTable in _db.UserTables orderby _userTable.UserId select _userTable).Skip(NowPageCount).Take(PageSize);

            if (ListAll == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(ListAll.ToList());
            }
        }
    }
}