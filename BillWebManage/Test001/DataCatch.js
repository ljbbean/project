/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.DataCatchAction = function() {  
    Test001.DataCatchAction.initializeBase(this);  
}

Test001.DataCatchAction.prototype = {  
    initialize: function() {  
        Test001.DataCatchAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.DataCatchAction.callBaseMethod(this, 'dispose');  
    },

    doSave: function (sender) {
        var form = sender.get_form();
        form.get_service().GetData(form.startDate.get_value(), form.cookies.get_value(), function (data) {
            $debug.traceDump(JSON.stringify(data));
        });
    },

    doDetails: function (sender) {
        var form = sender.get_form();
        form.get_service().GetDetailsData(form.cookies.get_value(), function () {
            alert('数据处理中');
        });
    }
}
Test001.DataCatchAction.registerClass('Test001.DataCatchAction', Sys.UI.PageAction);
