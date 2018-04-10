/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.FundsManager'); 

Test001.FundsManager.BackSectionManagerAction = function() {  
    Test001.FundsManager.BackSectionManagerAction.initializeBase(this);  
}

Test001.FundsManager.BackSectionManagerAction.prototype = {  
    initialize: function() {  
        Test001.FundsManager.BackSectionManagerAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.FundsManager.BackSectionManagerAction.callBaseMethod(this, 'dispose');  
    },

    doSure: function (sender) {
        var form = sender.get_form();
        if (confirm("保存后将无法通过修改， 是否确认保存？")) {
            form.get_service().Save(form.saveData(), function () {
                alert("保存成功");
                form.doOk();
            });
        }
    }
}
Test001.FundsManager.BackSectionManagerAction.registerClass('Test001.FundsManager.BackSectionManagerAction', Sys.UI.PageAction);
