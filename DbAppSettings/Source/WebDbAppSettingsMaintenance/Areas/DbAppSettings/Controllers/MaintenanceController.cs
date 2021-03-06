﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DbAppSettings.Maintenance.Model.DataAccess.Maintenance.Demo;
using DbAppSettings.Maintenance.Model.Service;
using DbAppSettings.Maintenance.Model.Service.Interfaces;
using DbAppSettings.Model.DataTransfer;
using WebDbAppSettingsMaintenance.Areas.DbAppSettings.Models;

namespace WebDbAppSettingsMaintenance.Areas.DbAppSettings.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly IDbAppSettingMaintenanceService _dbAppSettingMaintenanceService;

        public MaintenanceController()
        {
            _dbAppSettingMaintenanceService = new DbAppSettingMaintenanceService(new DemoDbAppSettingMaintenanceDao());
        }

        // GET: Maintenance/Maintenance
        [HttpGet]
        public ActionResult Index()
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll(HttpContext.Session.SessionID);
            List<string> applications = allSettings.Select(s => s.ApplicationKey).Distinct().OrderBy(a => a).ToList();
            return View(new DbAppSettingsViewModel() { Applications = applications});
        }

        [HttpPost]
        public ActionResult GetAllApplications()
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll(HttpContext.Session.SessionID);
            List<string> applications = allSettings.Select(s => s.ApplicationKey).Distinct().OrderBy(a => a).ToList();
            return new JsonResult() {Data = applications};
        }

        [HttpPost]
        public ActionResult GetAllAssembliesForApplication(string applicationKey)
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll(HttpContext.Session.SessionID);
            List<DbAppSettingDto> settingsForApplications = allSettings.Where(a => a.ApplicationKey == applicationKey).ToList();
            List<string> assembliesForSolution = settingsForApplications.Select(s => s.Assembly).Distinct().OrderBy(a => a).ToList();
            return new JsonResult() { Data = assembliesForSolution };
        }

        [HttpPost]
        public ActionResult GetAllDbAppSettingsForApplicationAndAssembly(string applicationKey, string assembly)
        {
            List<DbAppSettingDto> allSettings = _dbAppSettingMaintenanceService.GetAll(HttpContext.Session.SessionID);
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

            bool isValid = _dbAppSettingMaintenanceService.ValidateValueForType(model.Value, model.Type);
            if (!isValid)
                return new JsonResult() {Data = false};

            _dbAppSettingMaintenanceService.SaveDbAppSetting(HttpContext.Session.SessionID, toSave);

            return new JsonResult() { Data = true};
        }

        [HttpPost]
        public ActionResult RemoveSetting(DbAppSettingModel model)
        {
            if (model == null)
                throw new ValidationException("model cannot be null");

            DbAppSettingDto toRemove = model.ToDto();

            //TODO: Validate

            _dbAppSettingMaintenanceService.DeleteDbAppSetting(HttpContext.Session.SessionID, toRemove);

            return new JsonResult() { Data = true};
        }
    }
}