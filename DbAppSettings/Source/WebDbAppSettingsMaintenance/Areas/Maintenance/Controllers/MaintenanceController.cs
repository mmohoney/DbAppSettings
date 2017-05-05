using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo;
using DbAppSettings.Maintenance.Model.Service;
using DbAppSettings.Maintenance.Model.Service.Interfaces;
using DbAppSettings.Model.DataTransfer;
using WebDbAppSettingsMaintenance.Areas.Maintenance.Models;

namespace WebDbAppSettingsMaintenance.Areas.Maintenance.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IDbAppSettingMaintenanceService _dbAppSettingMaintenanceService;

        public MaintenanceController()
        {
            _dbAppSettingMaintenanceService = new DbAppSettingMaintenanceService(new DemoDbAppSettingMaintenanceDao());
        }

        // GET: Maintenance/Maintenance
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetAllApplications()
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll();
            List<string> applications = allSettings.Select(s => s.ApplicationKey).Distinct().OrderBy(a => a).ToList();
            return new JsonResult() {Data = applications};
        }

        [HttpPost]
        public ActionResult GetAllAssembliesForApplication(string applicationKey)
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll();
            List<DbAppSettingDto> settingsForApplications = allSettings.Where(a => a.ApplicationKey == applicationKey).ToList();
            List<string> assembliesForSolution = settingsForApplications.Select(s => s.Assembly).Distinct().OrderBy(a => a).ToList();
            return new JsonResult() { Data = assembliesForSolution };
        }

        [HttpPost]
        public ActionResult GetAllDbAppSettingsForApplicationAndAssemblie(string applicationKey, string assembly)
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll();
            List<DbAppSettingDto> settingsForApplications = allSettings.Where(a => a.ApplicationKey == applicationKey).ToList();
            List<DbAppSettingDto> settingsForAppAndAssembly = settingsForApplications.Where(s => s.Assembly == assembly).OrderBy(a => a.Key).ToList();
            List<DbAppSettingModel> settingModels = settingsForAppAndAssembly.Select(DbAppSettingModel.FromDto).ToList();
            return new JsonResult() { Data = settingModels };
        }

        [HttpPost]
        public ActionResult SaveSetting(DbAppSettingModel model)
        {
            if (model == null)
                throw new ValidationException("model cannot be null");

            DbAppSettingDto toSave = model.ToDto();

            //TODO: Validate

            _dbAppSettingMaintenanceService.SaveDbAppSetting(toSave);

            return new JsonResult();
        }

        [HttpPost]
        public ActionResult RemoveSetting(DbAppSettingModel model)
        {
            if (model == null)
                throw new ValidationException("model cannot be null");

            DbAppSettingDto toRemove = model.ToDto();

            //TODO: Validate

            _dbAppSettingMaintenanceService.DeleteDbAppSetting(toRemove);

            return new JsonResult();
        }
    }
}