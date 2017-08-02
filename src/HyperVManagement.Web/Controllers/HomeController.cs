using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HyperVManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (PowerShell instance = PowerShell.Create())
            {
                instance.AddScript("Get-VM");
                Collection<PSObject> psOutput = instance.Invoke();
                return View(psOutput);
            }
        }

        [HttpPost]
        public ActionResult Command(string command, string name)
        {
            var realCommand = GetPsCommand(command);
            if (realCommand == null) return Json(false);
            using (var instance = PowerShell.Create())
            {
                instance.AddScript($"{realCommand} \"{name}\"");
                Collection<PSObject> psOutput = instance.Invoke();
                return Json(true);
            }
        }

        private static string GetPsCommand(string command)
        {
            switch (command)
            {
                case "restart":
                    return "restart-vm -name ";
                case "stop":
                    return "stop-vm -name ";
                case "start":
                    return "start-vm -name ";
            }
            return null;
        }
    }
}