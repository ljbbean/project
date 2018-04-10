/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.Logistics');

Test001.Logistics.LogisticsInformationAction = function () {
    Test001.Logistics.LogisticsInformationAction.initializeBase(this);
}

Test001.Logistics.LogisticsInformationAction.prototype = {
    initialize: function() {  
        Test001.Logistics.LogisticsInformationAction.callBaseMethod(this, 'initialize');
    },

    dispose: function() {  
        Test001.Logistics.LogisticsInformationAction.callBaseMethod(this, 'dispose');
    },

    doSure: function(sender) {
        var form = sender.get_form();
        form.get_service().DoSure(form.tid, function () {
            form.isSure = true;
            form.doOk();
        });
    }
}
Test001.Logistics.LogisticsInformationAction.registerClass('Test001.Logistics.LogisticsInformationAction', Sys.UI.PageAction);
