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
    self.selected = ko.observableArray([]);
    self.firstSelected = ko.pureComputed(function() {
        if (self.selected && self.selected().length > 0) {
            return self.selected()[0];
        }
        return null;
    });

    table = $('#settings').DataTable({
        data: ko.toJS(self.settings),
        columns: [
            {
                title: "Key",
                data: "DisplayKey"
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
        self.selected.removeAll();
        
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        }
        else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }

        var selected = table.row('.selected');
        if (!selected || selected.length < 1) {
            return;
        }

        self.selected.push(selected.data());
    });

    //Editing setting
    self.settingModelText = ko.observable('');
    self.editingExistingKey = ko.observable(false);
    self.editSetting = ko.observable({
        Application: ko.observable(''),
        Assembly: ko.observable(''),
        Key: ko.observable(''),
        DisplayKey: ko.observable(''),
        Value: ko.observable(''),
        Type: ko.observable(''),
    });

    self.addSettingClick = function () {
        self.settingModelText('Add Setting');
        self.editingExistingKey(true);

        self.editSetting().Application(self.applicationSelection());
        self.editSetting().Assembly(self.assemblySelection());
        self.editSetting().Key('');
        self.editSetting().DisplayKey('');
        self.editSetting().Value('');
        self.editSetting().Type(self.types()[0]);
    };

    self.editSettingClick = function () {
        self.settingModelText('Edit Setting');
        self.editingExistingKey(false);

        if (!self.firstSelected()) {
            return;
        }
            
        var obj = self.firstSelected();
        self.editSetting().Application(ko.unwrap(obj.Application));
        self.editSetting().Assembly(ko.unwrap(obj.Assembly));
        self.editSetting().Key(ko.unwrap(obj.Key));
        self.editSetting().DisplayKey(ko.unwrap(obj.DisplayKey));
        self.editSetting().Value(ko.unwrap(obj.Value));
        self.editSetting().Type(ko.unwrap(obj.Type));
    };

    getApplications = function () {
        self.applications.removeAll();
        self.assemblies.removeAll();
        self.settings.removeAll();
        table.clear().draw();

        $.post(urls.GetAllApplications)
            .fail(function (err) {
                //TODO 
                //display error
            })
            .done(function (data) {
                if (!data || data.length < 1) {
                    return;
                }

                var previousApplicationSelection;
                if (self.applicationSelection()) {
                    previousApplicationSelection = self.applicationSelection();
                }

                self.applications(data);

                var found = false;
                if (previousApplicationSelection !== null) {
                    ko.utils.arrayForEach(self.applications(), function (app) {
                        if (ko.unwrap(app) === previousApplicationSelection) {
                            self.applicationSelection(app);
                            found = true;
                        }
                    });
                }
                if (previousApplicationSelection === null || !found) {
                    self.applicationSelection(null);
                }

                getAssemblies();
            })
            .always(function (data) {
                //TODO
                //overlay
            });
    };

    getAssemblies = function () {
        self.assemblies.removeAll();
        self.settings.removeAll();
        table.clear().draw();

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
        self.settings.removeAll();
        table.clear();

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

    self.removeSetting = function () {
        if (!self.firstSelected()) {
            return;
        }

        var obj = self.firstSelected();

        $.post(urls.RemoveSetting, { model: obj })
            .fail(function (err) {
                //TODO 
                //display error
            })
            .done(function (data) {
                getApplications();
            })
            .always(function (data) {
                //TODO
                //overlay
            });
    };

    //Display drivers
    self.assembliesEnabled = ko.pureComputed(function () {
        return self.applicationSelection();
    });
    self.editEnabled = ko.pureComputed(function () {
        return self.firstSelected();
    });

    self.removeEnabled = ko.pureComputed(function () {
        return self.firstSelected();
    });

    return self;
};

ko.bindingHandlers.limitCharacters = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var allowedNumberOfCharacters = valueAccessor();
        var currentValue = allBindingsAccessor.get('value');
        var cutText = ko.unwrap(currentValue).substr(0, allowedNumberOfCharacters);
        currentValue(cutText);
    }
};

ko.bindingHandlers.numeric = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).on("keydown", function (event) {
            // Allow: backspace, delete, tab, escape, and enter
            if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 27 || event.keyCode == 13 ||
                // Allow: Ctrl+A
                (event.keyCode == 65 && event.ctrlKey === true) ||
                // Allow: . ,
                (event.keyCode == 188 || event.keyCode == 190 || event.keyCode == 110) ||
                // Allow: home, end, left, right
                (event.keyCode >= 35 && event.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            else {
                // Ensure that it is a number and stop the keypress
                if (event.shiftKey || (event.keyCode < 48 || event.keyCode > 57) && (event.keyCode < 96 || event.keyCode > 105)) {
                    event.preventDefault();
                }
            }
        });

        $(element).change(function () {
            value($(element).val());
        });
    }
};