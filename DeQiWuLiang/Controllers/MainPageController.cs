using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeQiWuLiang.SQLConn;

namespace DeQiWuLiang.Controllers
{
    public class MainPageController : Controller
    {
        BaseDao basedao = new BaseDao();
        // GET: MainPage
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Login()
        {
            ViewBag.ErrorMsg = "";
            return View("Login");
        }

        public ActionResult LoginCheck()
        {
            string UserName = Request.Form["username"] == null ? null : Request.Form["username"].ToString();
            string PassWord = Request.Form["password"] == null ? null : Request.Form["password"].ToString();

            if(UserName!=null && UserName != "" && PassWord != null&& PassWord != "")
            {
                Response.Cookies["UserName"].Value = System.Web.HttpContext.Current.Server.UrlEncode(UserName);
                Response.Cookies["UserName"].Expires = DateTime.Now.AddMonths(12);
                Response.Cookies["PassWord"].Value = System.Web.HttpContext.Current.Server.UrlEncode(PassWord);
                Response.Cookies["PassWord"].Expires = DateTime.Now.AddMonths(12);
                return RedirectToAction("MainStateView");
            }
            else
            {
                ViewBag.ErrorMsg = "账号密码错误";
                return View("Login");
            }

        }

        public ActionResult MainStateView()
        {
            return View("MainStateView");
        }

        public ActionResult TRQEfficiencyView()
        {
            return View("TRQEfficiencyView");
        }

        public ActionResult RMEfficiencyView()
        {
            return View("RMEfficiencyView");
        }

        public ActionResult RSWZEfficiencyView()
        {
            return View("RSWZEfficiencyView");
        }

        public ActionResult RYEfficiencyView()
        {
            return View("RYEfficiencyView");
        }


        public ActionResult DeviceListView()
        {
            return View("DeviceList");
        }

        public ActionResult DetailSingleView()
        {
            string ChartType = Request.QueryString["chart"] == null ? null : Request.QueryString["chart"].ToString();
            if (ChartType != null)
            {
                ViewBag.ChartType = ChartType;
            }
            return View("DetailSingleView");
        }


        public ActionResult WDWarningView()
        {
            return View("WDWarningView");
        }

        public ActionResult YLWarningView()
        {
            return View("YLWarningView");
        }

        public ActionResult EfficiencyView()
        {
            return View("EfficiencyView");
        }

        public ActionResult SafeStateView()
        {
            return View("SafeStateView");
        }

        public ActionResult MapView()
        {
            return View("MapView");
        }

        public ActionResult SettingView()
        {
            return View("SettingView");
        }
        
        public　ActionResult ForgetPasswordView()
        {
            return View("ForgetPasswordView");
        }

        public ActionResult ChartView()
        {
            string UnitParam = Request.QueryString["Unit"] == null ? null : Request.QueryString["Unit"].ToString();
            switch (UnitParam)
            {
                case "1":
                    UnitParam = "℃";
                    break;
                case "2":
                    UnitParam = "%";
                    break;
                case "3":
                    UnitParam = "Pa";
                    break;
                default:
                    UnitParam = "未知";
                    break;
            }
            ViewBag.Unit = UnitParam;
            string from = Request.QueryString["from"] == null ? null : Request.QueryString["from"].ToString();
            string fromid = Request.QueryString["fromId"] == null ? null : Request.QueryString["fromId"].ToString();

            if(from!=null && fromid != null)
            {
                ViewBag.From = from;
                ViewBag.FromId = fromid;
            }

            string[] xAxis = new string[6];
            for (int i = 6; i > 0; i--)
            {
                xAxis[6-i]=DateTime.Now.AddMinutes(0 - i * 15).ToString("HH:mm");
            }
            ViewBag.xAxis = xAxis;

            return View("ChartView");
        }

        public ActionResult DetailView()
        {
            return View("DetailView");
        }

    }
}