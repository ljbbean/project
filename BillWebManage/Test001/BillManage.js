/// <reference path="docs/carpa-vsdoc.js" />
 
Type.registerNamespace('Test001');

Test001.BillManageAction = function() {
    Test001.BillManageAction.initializeBase(this);
}

Test001.BillManageAction.prototype = {
    initialize: function() {
        Test001.BillManageAction.callBaseMethod(this, 'initialize');
    },
    
    dispose: function() {
        Test001.BillManageAction.callBaseMethod(this, 'dispose');
    },
    
    doSave: function(sender) {
        var form = sender.get_form();
        var data = form.saveData();
        form.get_service().DoSave(data, function () {
            form._data = data;
            form.doOk();
        });
    }
}
Test001.BillManageAction.registerClass('Test001.BillManageAction', Sys.UI.PageAction);