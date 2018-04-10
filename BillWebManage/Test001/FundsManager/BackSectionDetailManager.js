/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.FundsManager'); 

Test001.FundsManager.BackSectionDetailManagerAction = function() {  
    Test001.FundsManager.BackSectionDetailManagerAction.initializeBase(this);  
}

Test001.FundsManager.BackSectionDetailManagerAction.prototype = {  
    initialize: function() {  
        Test001.FundsManager.BackSectionDetailManagerAction.callBaseMethod(this, 'initialize');  
    },

    dispose: function() {  
        Test001.FundsManager.BackSectionDetailManagerAction.callBaseMethod(this, 'dispose');  
    },

    doSure: function (sender) {
        var form = sender.get_form();
        var data = form.saveData();
        data.tmonth = form.month.get_text();
        if (data.tmonth == '' || data.tmonth == 'undefined') {
            alert('请选择月份');
            return;
        }
        form.get_service().Save(data, function () {
            alert("保存成功");
            form.doOk();
        });
    },

    doMonthChange: function (sender) {
        var value = sender.get_text();
        if (value == '' || value == 'undefined') {
            return;
        }
        var form = sender.get_form();
        form.get_service().GetTotal(value, function (data) {
            if (!data) {
                form.all.set_text(0);
                form.pay.set_text(0);
                return;
            }
            form.all.set_text(data.total);
            form.pay.set_text(data.paytotal);
        });
    }
}
Test001.FundsManager.BackSectionDetailManagerAction.registerClass('Test001.FundsManager.BackSectionDetailManagerAction', Sys.UI.PageAction);
