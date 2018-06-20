/// <reference path="../docs/carpa-vsdoc.js" />

Type.registerNamespace('Test001.DataHandler'); 

Test001.DataHandler.GoodsMatchRateAction = function() {  
    Test001.DataHandler.GoodsMatchRateAction.initializeBase(this);  
}

Test001.DataHandler.GoodsMatchRateAction.prototype = {
    initialize: function () {
        Test001.DataHandler.GoodsMatchRateAction.callBaseMethod(this, 'initialize');
        this.socket = null;
    },
    doButtonSave: function (sender) {
        var form = sender.get_form();
        var data = form.saveData();
        
        if (confirm("����Ϣ��Ӱ��ֳɱ�������������ֱ��Ӱ��, ȷ�ϱ���������Ϣ��")) {
            form.get_service().Save(data.user, data.grid, function () {
                form.close();
            })
        }
    }
}
Test001.DataHandler.GoodsMatchRateAction.registerClass('Test001.DataHandler.GoodsMatchRateAction', Sys.UI.PageAction);
