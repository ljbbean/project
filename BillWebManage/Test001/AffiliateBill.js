/// <reference path="docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001'); 

Test001.AffiliateBillAction = function() {  
    Test001.AffiliateBillAction.initializeBase(this);  
}

Test001.AffiliateBillAction.prototype = {  
    initialize: function() {  
        Test001.AffiliateBillAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.AffiliateBillAction.callBaseMethod(this, 'dispose');  
    },

    doSave: function(sender) {
        var form = sender.get_form();
        form.get_service().Save(form.saveData(), function () {
            alert('发货处理已提交');
            form.doOk();
        });
    }
}
Test001.AffiliateBillAction.registerClass('Test001.AffiliateBillAction', Sys.UI.PageAction);
