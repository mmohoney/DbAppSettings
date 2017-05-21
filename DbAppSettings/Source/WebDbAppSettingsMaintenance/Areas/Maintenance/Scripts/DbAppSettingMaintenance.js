var DbAppSettingMaintenance = function (config) {

    var self = this;
    var model = config.page.viewModel;
    var urls = config.page.urls;
    var table = {};
    var getApplications = function() { return; };
    var getAssemblies = function () { return; };
    var getSettings = function () { return; };

    self.types = ko.observableArray(model.Types);

    //Applications
    var applicationDescription = 'Select an Application...';
    self.applications = ko.observableArray(model.Applications);
    self.selectedApplicationDescription = ko.observable(applicationDescription);
    self.applicationSelection = ko.observable(null);
    self.applicationSelect = function (obj) {
        self.applicationSelection(obj);
    };
    self.applicationSelection.subscribe(function () {
        self.assemblySelection(null);
        if (self.applicationSelection()) {
            self.selectedApplicationDescription(self.applicationSelection());
            getAssemblies();
        } else {
            self.selectedApplicationDescription(applicationDescription);
        }
    });

    //Assemblies
    var assemblyDescription = 'Select an Assembly...';
    self.assemblies = ko.observableArray(model.Assemblies);
    self.selectedAssemblyDescription = ko.observable(assemblyDescription);
    self.assemblySelection = ko.observable();
    self.assemblySelect = function (obj, e) {
        self.assemblySelection(obj);
    };
    self.assemblySelection.subscribe(function () {
        if (self.assemblySelection()) {
            self.selectedAssemblyDescription(self.assemblySelection());
            getSettings();
        } else {
            self.selectedAssemblyDescription(assemblyDescription);
        }
    });

    //Settings
    self.settings = ko.observableArray([]);

    table = $('#settings').DataTable({
        data: ko.toJS(self.settings),
        columns: [
            {
                title: "Key",
                data: "Key"
            },
            {
                title: "Type",
                data: "Type"
            },
            {
                title: "Value",
                data: "Value"
            },
        ]
    });

    $('#settings tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    //Editing setting
    self.settingModelText = ko.observable('');
    self.editingExistingKey = ko.observable(false);
    self.editSetting = ko.observable({
        Assembly: ko.observable(''),
        Key: ko.observable(''),
        Value: ko.observable(''),
        Type: ko.observable(0),

    });

    self.addSettingClick = function () {
        self.settingModelText('Add Setting');
        self.editingExistingKey(true);

        self.editSetting().Assembly(self.typeSelection());
        self.editSetting().Key('');
        self.editSetting().Value('');
        self.editSetting().Type(self.types()[0]);
    };

    self.editSettingClick = function () {
        self.settingModelText('Edit Setting');
        self.editingExistingKey(false);

        var obj = table.row('.selected').data();
        obj.Assembly = 'Test Assembly';

        self.editSetting().Assembly(ko.unwrap(obj.Assembly));
        self.editSetting().Key(ko.unwrap(obj.Key));
        self.editSetting().Value(ko.unwrap(obj.Value));
        self.editSetting().Type(ko.unwrap(obj.Type));
    };

    getApplications = function() {

    };

    getAssemblies = function () {
        table.clear().draw();
        self.settings.removeAll();
        self.assemblies.removeAll();

        if (!self.applicationSelection()) {
            return;
        }

        $.post(urls.GetAllAssembliesForApplication, { applicationKey: ko.unwrap(self.applicationSelection) })
            .fail(function (err) {
                //TODO 
                //display error
            })
            .done(function (data) {
                if (!data || data.length < 1) {
                    return;
                }

                var previousAssemblySelection;
                if (self.assemblySelection()) {
                    previousAssemblySelection = self.assemblySelection();
                }

                self.assemblies(data);

                var found = false;
                if (previousAssemblySelection !== null) {
                    ko.utils.arrayForEach(self.assemblies(), function (assm) {
                        if (ko.unwrap(assm) === previousAssemblySelection) {
                            self.assemblySelection(assm);
                            found = true;
                        }
                    });
                }
                if (previousAssemblySelection === null || !found) {
                    self.assemblySelection(null);
                }

                getSettings();
            })
            .always(function(data) {
                //TODO
                //overlay
            });
    };

    getSettings = function () {
        table.clear();
        self.settings.removeAll();

        if (!self.assemblySelection()) {
            return;
        }

        $.post(urls.GetAllDbAppSettingsForApplicationAndAssembly, { applicationKey: ko.unwrap(self.applicationSelection), assembly: ko.unwrap(self.assemblySelection) })
            .fail(function (err) {
                //TODO 
                //display error
            })
            .done(function (data) {
                if (!data || data.length < 1) {
                    return;
                }

                self.settings(data);
                table.rows.add(ko.toJS(self.settings)).draw();
            })
            .always(function (data) {
                //TODO
                //overlay
            });
    };

    self.saveSetting = function () {
        $.post(urls.SaveSetting, { model: ko.toJS(self.editSetting) })
            .fail(function (err) {
                //TODO 
                //display error
            })
            .done(function (data) {
                $('.modal').modal('hide');
                getApplications();
            })
            .always(function (data) {
                //TODO
                //overlay
            });
    };

    //self.removeSetting = function (obj) {
    //    var confirmed = function () {
    //        $.loadingOverlay("Removing Setting...");

    //        $.ajax({
    //            url: urls.RemoveKey,
    //            data: JSON.stringify({
    //                assembly: ko.unwrap(obj.Assembly),
    //                settingKey: ko.unwrap(obj.Key)
    //            }),
    //            contentType: 'application/json; charset=utf-8',
    //            type: 'POST'
    //        }).then(function (result) {
    //            if (result.Success === false) {
    //                $.loadingOverlay.close();
    //                $.dialog.showWarning(result.Message);
    //            } else {
    //                getSolutions();
    //            }
    //        });
    //    };

    //    var message = "Remove Setting, continue?";
    //    $.dialog.showConfirm(message, "Remove Setting?", confirmed);
    //};

    //Display drivers
    self.assembliesEnabled = ko.pureComputed(function () {
        return self.applicationSelection();
    });

    return self;
};
